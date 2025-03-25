using System.Collections;
using UnityEngine;
using DG.Tweening;
using Klang.Seed.Audio;

public class Player : MonoBehaviour
{
[Header("Movement Settings")]
public float walkSpeed = 5f;
public float sprintMultiplier = 1.5f;

[Header("Hunger Settings")]
public float hungerNeed = 100f;
public float hungerDepletionRate = 1f;
public float hungerFillAmount = 20f;

[Header("Attack Settings")]
public float attackRange = 2f;
public float attackDuration = 0.3f;
public float attackScaleMultiplier = 2f;

// Cached components and state
private Renderer rend;
private Vector3 originalScale;
private Color originalColor;
private Tween moveTween;
private bool isAttacking = false;

// Reference to the GameManager (to update score on enemy “eaten”)
private GameManager gameManager;

void Start()
{
    originalScale = transform.localScale;
    rend = GetComponent<Renderer>();
    if (rend != null)
        originalColor = rend.material.color;
        
    gameManager = FindAnyObjectByType<GameManager>();

    AudioManager.Instance.Play3DAudio(AudioEvent.PlayerMovement, transform);
}

void Update()
{
    // Continuously decrease hungerNeed
    hungerNeed -= hungerDepletionRate * Time.deltaTime;
    if (hungerNeed < 0) hungerNeed = 0f; // GameManager will check for game over

    // Read input for horizontal and vertical movement (top-down on XZ)
    // float h = Input.GetAxis("Horizontal");
    // float v = Input.GetAxis("Vertical");
    var horizontalValue = 0;
    var verticalValue = 0;
    if (Input.GetKey(KeyCode.LeftArrow))
        horizontalValue = -1;
    else if (Input.GetKey(KeyCode.RightArrow))
        horizontalValue = 1;
    if (Input.GetKey(KeyCode.UpArrow))
        verticalValue = 1;
    else if (Input.GetKey(KeyCode.DownArrow))
        verticalValue = -1;
    Vector3 direction = new Vector3(horizontalValue, 0, verticalValue).normalized;
    
    // Determine sprinting – hold Left Shift to sprint, which increases speed (and “noise”)
    bool isSprinting = Input.GetKey(KeyCode.LeftShift);
    float speed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);

    // Move player if there is input and not attacking
    if (direction.sqrMagnitude > 0.01f && !isAttacking)
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        // If not already running, start running animation tween
        if (moveTween == null)
        {
            moveTween = transform.DOScale(originalScale * 1.1f, 0.3f)
                                 .SetLoops(-1, LoopType.Yoyo);
            AudioManager.Instance.PlayRunSound();
        }
    }
    else
    {
        // Stop running animation when idle
        if (moveTween != null)
        {
            moveTween.Kill();
            moveTween = null;
            transform.localScale = originalScale;
            AudioManager.Instance.StopRunSound();
        }
    }

    // Attack when Space key is pressed
    if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
    {
        StartCoroutine(Attack());
    }
}

IEnumerator Attack()
{
    isAttacking = true;
    // Stop any running tween so the attack animation takes precedence
    if (moveTween != null)
    {
        moveTween.Kill();
        moveTween = null;
    }
    AudioManager.Instance.Play3DAudio(AudioEvent.PlayerAttack, transform);

    // Attack animation: quickly scale up and change color to red then back to normal
    if (rend != null)
    {
        Sequence seq = DOTween.Sequence();
        var attackScale = new Vector3(originalScale.x * attackScaleMultiplier, originalScale.y, originalScale.z * attackScaleMultiplier);
        seq.Append(transform.DOScale(attackScale, attackDuration / 2))
           .Join(rend.material.DOColor(Color.red, attackDuration / 2))
           .Append(transform.DOScale(originalScale, attackDuration / 2))
           .Join(rend.material.DOColor(originalColor, attackDuration / 2));
        yield return seq.WaitForCompletion();
    }
    else
    {
        yield return new WaitForSeconds(attackDuration);
    }
    
    // Check for enemies within attackRange using a sphere overlap
    Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
    foreach (Collider hit in hits)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die();
            hungerNeed += hungerFillAmount;
            if (gameManager != null)
                gameManager.AddScore(1);
        }
    }
    isAttacking = false;
}

// Optional: visualize the attack range in the Scene view
void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
}
}
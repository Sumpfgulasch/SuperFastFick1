using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
enum State { Idle, Flee, Recover }
private State currentState;

[Header("Path and Speed Settings")]
public float idleSpeed = 3.5f;
public float fleeSpeed = 7f;

[Header("Detection and Timing")]
public float detectionRadius = 10f;
public float fleeMinTime = 5f;
public float recoverTime = 5f;

private int currentWaypointIndex = 0;
private float fleeTimer = 0f;
private float recoverTimer = 0f;

private NavMeshAgent agent;
private Transform playerTransform;
private Tween scaleTween;
private Vector3 originalScale;
private Transform[] waypoints;

void Start() {
    waypoints = WayPoints.instance.GetRandomPath();
    agent = GetComponent<NavMeshAgent>();
    originalScale = transform.localScale;
    ChangeState(State.Idle);

    // Find the player (make sure your Player GameObject is tagged "Player")
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
        playerTransform = playerObj.transform;

    // Set a random capsule color not too similar to the player’s color
    Renderer rend = GetComponent<Renderer>();
    if (rend != null)
    {
        Color playerColor = Color.white;
        if (playerTransform != null)
        {
            Renderer playerRenderer = playerTransform.GetComponent<Renderer>();
            if (playerRenderer != null)
                playerColor = playerRenderer.material.color;
        }
        Color enemyColor;
        int attempts = 0;
        do
        {
            enemyColor = Random.ColorHSV();
            attempts++;
        } while (Vector3.Distance(new Vector3(enemyColor.r, enemyColor.g, enemyColor.b),
                                    new Vector3(playerColor.r, playerColor.g, playerColor.b)) < 0.3f && attempts < 10);
        rend.material.color = enemyColor;
    }
}

void Update()
{
    switch (currentState)
    {
        case State.Idle:
            if (waypoints != null && waypoints.Length > 0)
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
            // If the player comes into view, switch to Flee
            if (playerTransform != null && InPlayerSight())
                ChangeState(State.Flee);
            break;

        case State.Flee:
            fleeTimer += Time.deltaTime;
            if (playerTransform != null)
            {
                // Calculate a destination in the opposite direction from the player
                Vector3 fleeDirection = (transform.position - playerTransform.position).normalized;
                Vector3 fleeDestination = transform.position + fleeDirection * 10f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(fleeDestination, out hit, 10f, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);
                // else {
                //     Debug.LogWarning("fleeing, no navmesh point found");
                // }
            }
            // Ensure flee lasts at least fleeMinTime until player is no longer in sight
            if (fleeTimer >= fleeMinTime && !InPlayerSight())
                ChangeState(State.Recover);
            break;

        case State.Recover:
            recoverTimer += Time.deltaTime;
            agent.ResetPath(); // Stay in place
            if (recoverTimer >= recoverTime)
                ChangeState(State.Idle);
            // If the player suddenly is detected again, switch back to Flee immediately
            if (playerTransform != null && InPlayerSight())
                ChangeState(State.Flee);
            break;
    }
}

// Checks whether the player is within detectionRadius and unobstructed by obstacles.
private bool InPlayerSight()
{
    if (playerTransform == null)
        return false;
    float distance = Vector3.Distance(transform.position, playerTransform.position);
    if (distance > detectionRadius)
        return false;
    Vector3 direction = (playerTransform.position - transform.position).normalized;
    RaycastHit hit;
    if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
    {
        if (hit.transform == playerTransform)
            return true;
    }
    return false;
}

private void ChangeState(State newState)
{
    currentState = newState;
    switch (newState)
    {
        case State.Idle:
            agent.speed = idleSpeed;
            if (waypoints != null && waypoints.Length > 0)
            {
                // Choose the nearest waypoint as starting destination
                float minDist = Mathf.Infinity;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    float dist = Vector3.Distance(transform.position, waypoints[i].position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        currentWaypointIndex = i;
                    }
                }
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            StartScaleTween(1f, 0.3f); // slower scale tween animation for idle/walk
            break;

        case State.Flee:
            agent.speed = fleeSpeed;
            fleeTimer = 0f;
            StartScaleTween(1f, 0.15f); // faster pulsing while fleeing
            break;

        case State.Recover:
            recoverTimer = 0f;
            agent.speed = idleSpeed;
            StartScaleTween(1f, 0.3f);
            break;
    }
}

// Uses DOTween to create a looping ping-pong scale animation on the capsule.
private void StartScaleTween(float scaleMultiplier, float duration)
{
    if (scaleTween != null && scaleTween.IsActive())
        scaleTween.Kill();
    scaleTween = transform.DOScale(originalScale * scaleMultiplier * 1.1f, duration)
                          .SetLoops(-1, LoopType.Yoyo);
}

// Called when the enemy is “eaten” by the player.
public void Die()
{
    Destroy(gameObject);
}

// Optional: visualize the detection radius in the Scene view
void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);
}
}
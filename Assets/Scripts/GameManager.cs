/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
[Header("Enemy Spawn Settings")]
public GameObject enemyPrefab;
public int maxEnemies = 3;
public float mapMinX = -10f, mapMaxX = 10f;
public float mapMinZ = -10f, mapMaxZ = 10f;

[Header("UI")]
public GameObject gameOverScreen;  // Assign a Game Over panel with a restart button
public Text highscoreText;

[Header("Player Reference")]
public Player player;

private int score = 0;

void Start()
{
    // Spawn initial enemies so that there are always maxEnemies present
    for (int i = 0; i < maxEnemies; i++)
        SpawnEnemy();
    
    if (gameOverScreen != null)
        gameOverScreen.SetActive(false);
}

void Update()
{
    // Ensure that there are always (up to) maxEnemies in the scene
    Enemy[] enemies = FindObjectsOfType<Enemy>();
    if (enemies.Length < maxEnemies)
        SpawnEnemy();

    // If the player’s hunger runs out, trigger game over
    if (player.hungerNeed <= 0)
        GameOver();

    // Update the highscore UI
    if (highscoreText != null)
        highscoreText.text = "Score: " + score;
}

void SpawnEnemy()
{
    // Spawn enemy at a random point along the edge of the map.
    Vector3 spawnPos = Vector3.zero;
    int edge = Random.Range(0, 4);
    switch (edge)
    {
        case 0: // top edge
            spawnPos = new Vector3(Random.Range(mapMinX, mapMaxX), 0, mapMaxZ);
            break;
        case 1: // bottom edge
            spawnPos = new Vector3(Random.Range(mapMinX, mapMaxX), 0, mapMinZ);
            break;
        case 2: // left edge
            spawnPos = new Vector3(mapMinX, 0, Random.Range(mapMinZ, mapMaxZ));
            break;
        case 3: // right edge
            spawnPos = new Vector3(mapMaxX, 0, Random.Range(mapMinZ, mapMaxZ));
            break;
    }
    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
}

public void AddScore(int amount)
{
    score += amount;
}

public void GameOver()
{
    if (gameOverScreen != null)
        gameOverScreen.SetActive(true);
    Time.timeScale = 0f;
}

public void RestartGame()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}*/
using UnityEngine;
using TMPro;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;


    void Start()
    {
        
            SpawnBall();
        
    }

    public void SpawnBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("BallSpawner: Ball prefab not assigned!");
            return;
        }

        // Get the center position of the spawned Gamefield object
        Vector3 objectCenter = spawnPoint.transform.position;

        // Add a small offset to the Y-axis to spawn the ball slightly above the Gamefield
        float yOffset = 0.2f; // Adjust this value as needed
        Vector3 ballPosition = objectCenter + Vector3.up * yOffset;

        // Log the ball spawn position
        Debug.Log("Ball spawn position: " + ballPosition);

        // Spawn the ball at the adjusted position
        Instantiate(ballPrefab, ballPosition, Quaternion.identity);

        Debug.Log("Ball spawned slightly above Gamefield at: " + ballPosition);
    }
}

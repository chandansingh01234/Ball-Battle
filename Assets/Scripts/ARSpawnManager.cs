using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ARSpawnManager : MonoBehaviour
{
    public static ARSpawnManager Instance { get; private set; }

    public GameObject Gamefield; // The prefab to spawn
    public GameObject ballPrefab; // Assign your ball prefab in the Inspector
    public ARCameraManager arCameraManager; // Assign your AR camera in the Inspector
    public ARSpawnManager ARP;
    public GameObject ARcamera;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Perform a raycast from the touch position
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                // Get the pose (position and rotation) of the hit
                Pose hitPose = hits[0].pose;

                // Adjust the spawn position to be 1 unit above the detected plane
                Vector3 spawnPosition = hitPose.position + Vector3.up * 1f;

                // Spawn the Gamefield object at the adjusted position
                GameObject spawnedObject = Instantiate(Gamefield, spawnPosition, hitPose.rotation);

                // Check if the Gamefield object was instantiated correctly
                if (spawnedObject == null)
                {
                    Debug.LogError("Gamefield object not instantiated!");
                    return;
                }

                // Log the position of the Gamefield object
                Debug.Log("Gamefield position: " + spawnedObject.transform.position);

                // Start a coroutine to spawn the ball after 1 second
                //SpawnBallAboveGamefield(spawnedObject);

                // Disable the ARSpawnManager script after spawning
                ARP.enabled = false;
            }
        }
    }



    public void SpawnBallAboveGamefield(GameObject spawnedObject)
    {
        if (ballPrefab == null)
        {
            Debug.LogError("BallSpawner: Ball prefab not assigned!");
            return;
        }

        // Get the center position of the spawned Gamefield object
        Vector3 objectCenter = spawnedObject.transform.position;

        // Add a small offset to the Y-axis to spawn the ball slightly above the Gamefield
        float yOffset = 0.2f; // Adjust this value as needed
        Vector3 ballPosition = objectCenter + Vector3.up * yOffset;

        // Log the ball spawn position
        Debug.Log("Ball spawn position: " + ballPosition);

        // Spawn the ball at the adjusted position
        Instantiate(ballPrefab, ballPosition, Quaternion.identity);

        Debug.Log("Ball spawned slightly above Gamefield at: " + ballPosition);
    }
    public void OpenmainScene()
    {
        ARcamera.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Components")]
    public AudioSource passSound;
    public TrailRenderer trailRenderer;
    
    private GameObject currentHolder;

    void Start()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (currentHolder != null)
        {
            transform.position = currentHolder.transform.position;
        }
    }

    public void SetHolder(GameObject holder)
    {
        if (currentHolder != null && holder != currentHolder)
        {
            PlayPassEffects();
        }

        currentHolder = holder;
        Debug.Log($"Ball is now held by: {holder.name}");

        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
        
        GameEvents.BallPickup(holder);
    }

    public void Release()
    {
        currentHolder = null;
        Debug.Log("Ball has been released!");

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        
        GameEvents.BallDrop(gameObject);
    }

    private void PlayPassEffects()
    {

        if (passSound != null)
        {
            passSound.Play();
        }
    }

    public GameObject GetCurrentHolder()
    {
        return currentHolder;
    }
}

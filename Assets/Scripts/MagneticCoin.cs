using UnityEngine;

public class MagneticCoin : MonoBehaviour
{
    public int scoreValue = 10;
    public float moveSpeed = 5f;
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[MagneticCoin] Trigger entered with: {other.gameObject.name} (Tag: {other.tag})");
        AttemptCollect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[MagneticCoin] Collision enter with: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        AttemptCollect(collision.gameObject);
    }

    private void AttemptCollect(GameObject user)
    {
        if (user.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
                Debug.Log($"[MagneticCoin] Coin collected! Added {scoreValue} score.");
            }
            else
            {
                Debug.LogError("[MagneticCoin] CRITICAL: GameManager.Instance is NULL! Is the GameManager in the scene?");
            }
            
            // Destroy regardless of GameManager to prevent infinite collisions, but warn above
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[MagneticCoin] Object is not Player. Ignoring.");
        }
    }
}

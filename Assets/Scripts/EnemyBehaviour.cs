using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float maxDetectionDistance = 5.0f;
    [SerializeField, Range(0.0f, 180.0f)] float angleOfDetection = 180.0f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float firingInterval = 1.0f;

    private float t1 = 0.0f;

    void Start()
    {
        Debug.Assert(player != null);
        Debug.Assert(bulletPrefab != null);
    }

    void Update()
    {
        Vector3 delta = player.transform.position - transform.position;

        // Player is out of range.
        if (delta.magnitude > maxDetectionDistance)
        {
            Debug.Log("Player is out of range");
            t1 = 0.0f; // Reset the firing timer so we don't instantly shoot on re-acquiring the player.
            return;
        }

        Debug.Log("Player is within range, calculating angle required...");

        // Find the angle required to turn towards player.
        float angleToPlayer = Vector3.Angle(transform.forward, delta);

        // Snap look at the player if the angle is within the detection cone.
        if (angleToPlayer <= angleOfDetection)
        {
            transform.rotation = Quaternion.LookRotation(delta);

            // Timing logic using firingInterval.
            t1 += Time.deltaTime;
            if (t1 >= firingInterval)
            {
                spawnBullet();
                t1 = 0.0f;
            }
        }
        else
        {
            // Not aiming at the player yet, so don't accumulate towards the next shot.
            t1 = 0.0f;
        }
    }

    private void spawnBullet()
    {
        // Spawn a bullet from bulletPrefab facing the player.
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        Debug.Log("Shooting now!");
    }
}

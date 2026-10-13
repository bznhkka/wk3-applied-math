using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField, Range(0.00f, 50f)] private float speed = 5.0f;
    [SerializeField][Min(0.1f)] float lifetimeSeconds = 5.0f;

    private float age = 0.0f;

    void Update()
    {
        // Dumb, physics-free movement: just walk the transform forward each frame.
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        age += Time.deltaTime;
        if (age >= lifetimeSeconds)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour enemy = other.GetComponentInParent<EnemyBehaviour>();
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            Destroy(gameObject);
            return;
        }

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.GameOver();
            Destroy(gameObject);
        }
    }
}

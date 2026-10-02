using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField, Range(0.00f, 50f)] private float speed = 5.0f;

    void Update()
    {
        // Dumb, physics-free movement: just walk the transform forward each frame.
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.GameOver();
            Destroy(gameObject);
        }
    }
}

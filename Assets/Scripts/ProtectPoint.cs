using UnityEngine;

public class ProtectPoint : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void Start()
    {
        Debug.Assert(gameManager != null);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            gameManager.DecrementHP(1);
        }
    }
}

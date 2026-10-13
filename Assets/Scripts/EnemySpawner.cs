using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField][Min(0.0f)] float spawnIntervalSeconds = 2.0f;
    [SerializeField] bool isQuadraticBezier = false; // otherwise cubic

    private float t1 = 0.0f; // for spawning

    void Start()
    {
        if (targetPoint == null)
        {
            ProtectPoint protectPoint = FindFirstObjectByType<ProtectPoint>();
            if (protectPoint != null)
            {
                targetPoint = protectPoint.transform;
            }
        }

        Debug.Assert(targetPoint != null);
        Debug.Assert(enemyPrefab != null);
        Debug.Assert(enemyPrefab.GetComponent<EnemyBehaviour>() != null);

        // Spawn the first enemy immediately, then keep the interval afterwards.
        t1 = spawnIntervalSeconds;
    }

    void Update()
    {
        t1 += Time.deltaTime;

        if (t1 >= spawnIntervalSeconds)
        {
            // Subtract instead of resetting to 0 so leftover time carries over
            // and the average spawn rate doesn't drift.
            t1 -= spawnIntervalSeconds;
            spawnEnemy();
        }
    }

    private void spawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        // Tell the spawned enemy where to go.
        enemy.GetComponent<EnemyBehaviour>().SetTarget(targetPoint, isQuadraticBezier);
    }
}

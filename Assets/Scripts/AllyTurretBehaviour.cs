using System.Collections.Generic;
using UnityEngine;

public class AllyTurretBehaviour : MonoBehaviour
{
    [SerializeField] Transform protectionPoint;
    [SerializeField] float maxDetectionDistance = 5.0f;
    [SerializeField, Range(0.0f, 180.0f)] float angleOfDetection = 180.0f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float firingInterval = 1.0f;

    private float t1 = 0.0f;

    void Start()
    {
        Debug.Assert(protectionPoint != null);
        Debug.Assert(bulletPrefab != null);
    }

    void Update()
    {
        // Get array of enemies existing in scene.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0)
        {
            return; // nothing to do
        }

        // Filter enemies by both detection range and detection angle (both measured from the turret).
        // Squared distances are compared to avoid a sqrt per enemy.
        float maxSqrDistance = maxDetectionDistance * maxDetectionDistance;
        List<GameObject> filtered_enemies = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            Vector3 toEnemy = enemy.transform.position - transform.position;

            if (toEnemy.sqrMagnitude > maxSqrDistance)
            {
                continue; // out of range
            }

            if (Vector3.Angle(transform.forward, toEnemy) > angleOfDetection)
            {
                continue; // outside the detection cone
            }

            filtered_enemies.Add(enemy);
        }

        if (filtered_enemies.Count <= 0)
        {
            t1 = 0.0f; // Nothing to shoot at, so don't carry over a partially charged shot.
            return;
        }

        // Pick the target closest to the protectionPoint (the thing we're defending).
        GameObject target = null;
        float closestSqrDistance = float.MaxValue;
        foreach (GameObject enemy in filtered_enemies)
        {
            float sqrDistance = (enemy.transform.position - protectionPoint.position).sqrMagnitude;
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                target = enemy;
            }
        }

        Vector3 delta = target.transform.position - transform.position;

        // Target is out of range.
        if (delta.magnitude > maxDetectionDistance)
        {
            Debug.Log("Enemy is out of range");
            t1 = 0.0f; // Reset the firing timer so we don't instantly shoot on re-acquiring the target.
            return;
        }

        Debug.Log("Enemy is within range, calculating angle required...");

        // Find the angle required to turn towards target.
        float angleToPlayer = Vector3.Angle(transform.forward, delta);

        // Snap look at the target if the angle is within the detection cone.
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
            // Not aiming at the target yet, so don't accumulate towards the next shot.
            t1 = 0.0f;
        }
    }

    private void spawnBullet()
    {
        // Spawn a bullet from bulletPrefab facing the target.
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        Debug.Log("Shooting now!");
    }
}

using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField][Min(0.01f)] float travelTimeSeconds = 5.0f;

    // Max sideways distance of the control points from the straight line to the target.
    [SerializeField][Min(0.0f)] float curveOffset = 3.0f;

    // The curve bends sideways within the plane that has this normal.
    // Vector3.up = ground plane (3D top-down), (0, 0, 1) = XY plane (2D games).
    [SerializeField] Vector3 curveNormal = Vector3.up;

    private Transform target;
    private bool isQuadratic = false;
    private bool hasTarget = false;
    private bool hasReachedProtectionPoint = false;

    private Vector3 start;
    private Vector3 control1;
    private Vector3 control2; // cubic only
    private Vector3 end;

    private float t = 0.0f; // curve progress, 0..1

    public void SetTarget(Transform newTarget, bool isQuadraticBezier)
    {
        Debug.Assert(newTarget != null);

        target = newTarget;
        isQuadratic = isQuadraticBezier;

        start = transform.position;
        end = target.position;

        Vector3 toEnd = end - start;

        // Direction perpendicular to the path, used to push control points sideways.
        Vector3 side = Vector3.Cross(toEnd, curveNormal);
        side = side.sqrMagnitude > 1e-6f ? side.normalized : Vector3.right;

        if (isQuadratic)
        {
            // P0 = start, P1 = control1, P2 = end
            control1 = start + toEnd * 0.5f + side * randomOffset();
        }
        else
        {
            // P0 = start, P1 = control1, P2 = control2, P3 = end
            control1 = start + toEnd * (1.0f / 3.0f) + side * randomOffset();
            control2 = start + toEnd * (2.0f / 3.0f) + side * randomOffset();
        }

        t = 0.0f;
        hasTarget = true;
    }

    void Update()
    {
        if (!hasTarget) return;

        // Follow the target if it moves; if it was destroyed, head to its last known position.
        if (target != null)
        {
            end = target.position;
        }

        t = Mathf.Min(t + Time.deltaTime / travelTimeSeconds, 1.0f);

        transform.position = isQuadratic
            ? quadraticBezier(start, control1, end, t)
            : cubicBezier(start, control1, control2, end, t);

        if (t >= 1.0f)
        {
            hasTarget = false;
            ReachProtectionPoint();
        }
    }

    public void ReachProtectionPoint()
    {
        if (hasReachedProtectionPoint)
        {
            return;
        }

        hasReachedProtectionPoint = true;
        hasTarget = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecrementHP(1);
        }

        Destroy(gameObject);
    }

    private float randomOffset()
    {
        float sign = Random.value < 0.5f ? -1.0f : 1.0f;
        return sign * Random.Range(0.5f, 1.0f) * curveOffset;
    }

    private static Vector3 quadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1.0f - t;
        return u * u * p0
             + 2.0f * u * t * p1
             + t * t * p2;
    }

    private static Vector3 cubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1.0f - t;
        return u * u * u * p0
             + 3.0f * u * u * t * p1
             + 3.0f * u * t * t * p2
             + t * t * t * p3;
    }
}

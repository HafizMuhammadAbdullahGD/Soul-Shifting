using UnityEngine;

public class Finder
{
    public static Transform FindFirstTargetInRange(Vector3 position, float radius, LayerMask targetLayer)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, targetLayer);
        if (colliders?.Length > 0)
            return colliders[0].transform;
        return null;

    }
}
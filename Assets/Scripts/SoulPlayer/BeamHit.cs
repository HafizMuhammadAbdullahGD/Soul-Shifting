using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHit : MonoBehaviour
{
    [SerializeField] private float _damageRate;
    private void OnTriggerEnter(Collider hit)
    {

        IDamage targetDamage;
        if (hit.TryGetComponent<IDamage>(out targetDamage))
        {
            targetDamage.OnDamage(_damageRate);
        }
    }
}

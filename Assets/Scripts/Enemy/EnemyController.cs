using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamage
{
    //Logic of enemy will implement here later
    private Health _enemyHealth;
    private void Awake()
    {
        _enemyHealth = GetComponent<Health>();
    }
    public void OnDamage(float damageRate)
    {
        //update enemy health bar
        _enemyHealth.Damage(damageRate);
        if (_enemyHealth.IsDead())
        {
            DisableEnemy();
        }
    }
    void DisableEnemy()
    {
        GetComponent<Animator>().SetLayerWeight(1, 1);
        GetComponent<Animator>().SetBool("isDead", true);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        Destroy(this.gameObject, 5f);
    }
}

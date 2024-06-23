using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    [SerializeField] private float _damgeRatePerSecond;

    private float _currentTime;

    private Vector3 _startPosition;

    private Health _soulHealth;

    private void Awake()
    {
        _startPosition = this.transform.localPosition;
        _soulHealth = GetComponent<Health>();
    }
    private void OnEnable()
    {
        //Set the health when player soul activate
        _soulHealth.SetHealth(100);
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
        //current time is elaped 1 seconds, then reset time
        if (_currentTime >= 1)
        {
            _currentTime = 0;
            _soulHealth.Damage(_damgeRatePerSecond);
            if (_soulHealth.IsDead())
            {
                RespawnSould();
            }
        }
    }

    private void RespawnSould()
    {
        _soulHealth.SetHealth(100);
        //Character controller not allow to change position directly, have to disable it when assign new position
        GetComponent<CharacterController>().enabled = false;
        this.transform.localPosition = _startPosition;
        GetComponent<CharacterController>().enabled = true;



    }
}

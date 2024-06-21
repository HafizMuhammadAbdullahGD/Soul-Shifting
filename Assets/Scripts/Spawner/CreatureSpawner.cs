using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] Transform _creatureVampire;
    [SerializeField] Transform _creatureAlien;
    [SerializeField] Transform _currentCreature_1;
    [SerializeField] Transform _currentCreature_2;
    private void Update()
    {
        if (!_currentCreature_1)
        {
            _currentCreature_1 = SpawnCreature(_creatureVampire);
        }
        if (!_currentCreature_2)
        {
            _currentCreature_2 = SpawnCreature(_creatureAlien);
        }

    }
    Transform SpawnCreature(Transform toSpawn)
    {
        return Instantiate(toSpawn);
    }

}

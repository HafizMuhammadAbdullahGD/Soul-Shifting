using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private CreatureType _type;
    public CreatureType GetCreatureType()
    {
        return _type;
    }
}
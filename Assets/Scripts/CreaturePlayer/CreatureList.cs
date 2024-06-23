using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CreatureType
{
    Vampire, Alien
}
[System.Serializable]
public class CreaturesData
{
    public CreatureType CreatureType;
    public Transform CreatureMesh;
    public Transform CreatureSkeleton;
    public Avatar CreatureAvatar;
}
public class CreatureList : MonoBehaviour
{

    [SerializeField] private List<CreaturesData> _creatures;
    public CreaturesData GetCreature(CreatureType type)
    {
        foreach (var creature in _creatures)
        {
            if (creature.CreatureType == type)
                return creature;
        }
        print("creature not found!");
        return null;
    }
}

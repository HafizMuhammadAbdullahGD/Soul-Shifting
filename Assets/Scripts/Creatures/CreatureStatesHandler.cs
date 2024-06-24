using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CreatureStateTypes
{
    Attack,
}
[System.Serializable]
public struct CreatureStates
{
    public CreatureStateTypes Type;
    public StateMachine State;

}
public class CreatureStatesHandler : MonoBehaviour
{
    [SerializeField] private List<CreatureStates> _creatureStates;
    public List<CreatureStates> GetCreatureStates()
    {
        return _creatureStates;
    }

}

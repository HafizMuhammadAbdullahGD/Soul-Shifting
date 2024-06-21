using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureStatesController : MonoBehaviour
{
    [SerializeField] private List<CreatureStates> _creatureStates;

    [SerializeField] private float _attackCoolDownInterval;
    private float _currentTime = 0;

    [SerializeField] private CoolDownUI _attackCoolDownUI;
    private StateMachine _currentState;
    private bool isAttacked;
    private bool isCoolDown;
    void Start()
    {
        isAttacked = false;
        isCoolDown = true;
    }
    private void Update()
    {
        StateKeys();

        _currentState = _currentState?.Execute();
    }
    void StateKeys()
    {
        if (!CanAttack()) return;

        //left punch
        if (Input.GetMouseButtonDown(0))
        {
            _currentState = GetState(CreatureStateTypes.Attack);
            isAttacked = true;
            isCoolDown = false;
            _currentTime = 0;
        }
    }
    bool CanAttack()
    {
        //if one state executing
        if (_currentState) return false;

        if (isAttacked)
        {
            StartCoroutine(CoolDownInterval());
            isAttacked = false;
        }
        //return until attack cool down
        if (!isCoolDown) return false;

        return true;
    }


    public StateMachine GetState(CreatureStateTypes type)
    {
        foreach (var creature in _creatureStates)
        {
            if (creature.Type == type)
                return creature.State;
        }
        return null;
    }
    private IEnumerator CoolDownInterval()
    {
        //Initialize
        _attackCoolDownUI.TxtCoolDown.text = _attackCoolDownInterval.ToString() + ".00";
        _attackCoolDownUI.ImgLoading.fillAmount = 1;


        _attackCoolDownUI.Panel.gameObject.SetActive(true);
        float timeElapsed = _attackCoolDownInterval;
        while (timeElapsed >= 0)
        {
            timeElapsed -= Time.deltaTime;
            float roundedTime = Mathf.Round(timeElapsed * 10) / 10;
            _attackCoolDownUI.TxtCoolDown.text = roundedTime.ToString();
            _attackCoolDownUI.ImgLoading.fillAmount = timeElapsed / _attackCoolDownInterval;
            yield return null;
        }
        _attackCoolDownUI.Panel.gameObject.SetActive(false);
        isCoolDown = true;


    }
    //-------------------For Future Use---------------------
    public void LoadStates(CreatureStatesHandler states)
    {
        _creatureStates = states.GetCreatureStates();
    }
    //-------------------For Future Use---------------------

}

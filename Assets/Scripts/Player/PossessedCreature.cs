using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedCreature : MonoBehaviour
{
    [SerializeField] private Transform _attackCoolDownUI;
    [SerializeField] private CoolDownUI _creatureLeavingUI;
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private float _creatureLeaveTime;
    [SerializeField] private float _capturingRadius;

    [SerializeField] private float _creatureCaputringMinHealth;
    [SerializeField] private float _creatureCaputringMaxHealth;
    private float _currentTime;
    [SerializeField] private Transform _playerSoul;
    [SerializeField] private Transform _selectedCreature;
    [SerializeField] private Transform _panelTargetCapture;
    [SerializeField] private CreatureList _ceatures;


    public static event Func<Transform> EventCapturedAlready;

    //Reset everything when enable

    [SerializeField] private Transform _targetCreature;
    public static event Action EventLeavePossessedCreature;
    private void Awake()
    {
        _currentTime = _creatureLeaveTime;
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        _playerSoul.transform.gameObject.SetActive(true);

    }
    public void SelectCreature(Transform targetCreature)
    {
        _selectedCreature = targetCreature;
        CreaturesData possessedCreature = _ceatures.GetCreature(targetCreature.GetComponent<Creature>().GetCreatureType());
        GetComponent<Animator>().avatar = possessedCreature.CreatureAvatar;
        possessedCreature.CreatureMesh.gameObject.SetActive(true);
        possessedCreature.CreatureSkeleton.SetSiblingIndex(0);
        possessedCreature.CreatureSkeleton.gameObject.SetActive(true);

    }
    void Update()
    {

        _targetCreature = FindTargetInRange();
        if (_targetCreature)
        {

            Health creatureHealth;
            if (_targetCreature.TryGetComponent<Health>(out creatureHealth))
            {
                float health = creatureHealth.GetHealth();
                if (health >= _creatureCaputringMinHealth && health <= _creatureCaputringMaxHealth)
                {
                    _panelTargetCapture.gameObject.SetActive(true);
                    CaptureCreature();
                }
            }

        }
        else if (Input.GetKey(KeyCode.E))
        {
            //Start leaving creature
            LeaveCreature();
            return;
        }
        else
        {
            if (_panelTargetCapture.gameObject.activeSelf)
                _panelTargetCapture.gameObject.SetActive(false);
        }
        //Reset 

        CancelCreatureLeaving();
    }
    void CaptureCreature()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _currentTime = 0;
            LeaveCreature();
            EventLeavePossessedCreature?.Invoke();
        }
    }
    void LeaveCreature()
    {
        _attackCoolDownUI.gameObject.SetActive(false);
        _creatureLeavingUI.Panel.gameObject.SetActive(true);
        _currentTime -= Time.deltaTime;
        float timeElapsed = Mathf.Round(_currentTime * 10) / 10;
        _creatureLeavingUI.TxtCoolDown.text = timeElapsed.ToString();
        _creatureLeavingUI.ImgLoading.fillAmount = _currentTime / _creatureLeaveTime;
        if (_currentTime <= 0)
        {

            //leave creature
            _playerSoul.position = this.transform.position;
            _playerSoul.localRotation = this.transform.localRotation;

            _playerSoul.gameObject.SetActive(false);

            _playerSoul.transform.root.gameObject.SetActive(true);

            _currentTime = _creatureLeaveTime;


            _selectedCreature.position = this.transform.position;
            _selectedCreature.localRotation = this.transform.localRotation;
            _selectedCreature.gameObject.SetActive(true);

            CreaturesData possessedCreature = _ceatures.GetCreature(_selectedCreature.GetComponent<Creature>().GetCreatureType());
            GetComponent<Animator>().avatar = null;
            possessedCreature.CreatureMesh.gameObject.SetActive(false);
            possessedCreature.CreatureSkeleton.gameObject.SetActive(false);

            _selectedCreature.GetComponent<EnemyController>().Died();

            this.transform.root.gameObject.SetActive(false);

        }

    }
    void CancelCreatureLeaving()
    {
        _currentTime = _creatureLeaveTime;


        if (_creatureLeavingUI.Panel.gameObject.activeSelf)
        {
            _creatureLeavingUI.Panel.gameObject.SetActive(false);

        }
    }
    private Transform FindTargetInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _capturingRadius, _targetLayer);
        if (colliders?.Length > 0)
            return colliders[0].transform;
        return null;

    }
}

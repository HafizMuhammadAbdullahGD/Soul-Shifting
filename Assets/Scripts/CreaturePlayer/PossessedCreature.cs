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
    [SerializeField] private Transform _panelTargetCapture;
    [SerializeField] private CreatureList _ceatures;


    private Transform _selectedCreature;
    private Transform _targetCreature;
    public static event Action EventLeavePossessedCreature;
    private void OnEnable()
    {
        _currentTime = _creatureLeaveTime;

    }
    private void OnDisable()
    {
        if (_playerSoul)
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

        _targetCreature = Finder.FindFirstTargetInRange(this.transform.position, _capturingRadius, _targetLayer);

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
        if (Input.GetKey(KeyCode.E))
        {
            //Start leaving creature
            LeaveCreature();
            return;
        }
        else if (!_targetCreature)
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

}

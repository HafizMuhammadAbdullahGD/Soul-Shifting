using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedCreature : MonoBehaviour
{
    [SerializeField] private Transform _attackCoolDownUI;
    [SerializeField] private CoolDownUI _creatureLeavingUI;
    [SerializeField] private float _creatureLeaveTime;
    private float _currentTime;
    [SerializeField] private Transform _playerSoul;
    [SerializeField] private Transform _selectedCreature;
    [SerializeField] private CreatureList _ceatures;
    //Reset everything when enable
    private void Awake()
    {
        _currentTime = _creatureLeaveTime;
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
        if (Input.GetKey(KeyCode.E))
        {
            //Start leaving creature
            LeaveCreature();
            return;
        }
        //Reset 

        CancelCreatureLeaving();
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

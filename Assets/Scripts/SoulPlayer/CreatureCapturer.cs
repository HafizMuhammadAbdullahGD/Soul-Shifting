using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class CreatureCapturer : MonoBehaviour
{


    [SerializeField] private float _creatureCaputringMinHealth;
    [SerializeField] private float _creatureCaputringMaxHealth;
    [SerializeField] private float _capturingRadius;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Transform _panelTargetCapture;
    [SerializeField] Transform _playerCreature;

    private Transform _targetCreature;
    bool isPossessedState;

    private void OnEnable()
    {
        PossessedCreature.EventLeavePossessedCreature += StartPossession;

    }
    private void OnDisable()
    {
        PossessedCreature.EventLeavePossessedCreature -= StartPossession;

        //Enable Player Armature, *necessory because Controller will not work 
        if (_playerCreature)
            _playerCreature.transform.gameObject.SetActive(true);
        _panelTargetCapture.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isPossessedState) return;

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
        else
        {
            if (_panelTargetCapture.gameObject.activeSelf)
                _panelTargetCapture.gameObject.SetActive(false);

        }


    }
    void CaptureCreature()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartPossession();
        }
    }
    void StartPossession()
    {
        _targetCreature = Finder.FindFirstTargetInRange(this.transform.position, _capturingRadius, _targetLayer);

        StartCoroutine(PossessionState());

    }
    IEnumerator PossessionState()
    {
        isPossessedState = true;


        // will enable in ThirdPersonController script when gameobject enable
        GetComponent<ThirdPersonController>().CanMove = false;
        GetComponent<CharacterController>().enabled = false;
        //----------------------------------------------------------------

        Transform creatureCapturePoint = _targetCreature.Find("CreatureCapturePoint");
        if (!creatureCapturePoint)
        {
            print("Creature Capture Point Not Found!");

        }
        while (Vector3.Distance(this.transform.position, creatureCapturePoint.position) >= 0.001f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, creatureCapturePoint.position, 3 * Time.deltaTime);
            yield return null;


        }


        _playerCreature.GetComponent<CharacterController>().enabled = false;
        _playerCreature.transform.position = _targetCreature.position;
        _playerCreature.transform.localRotation = _targetCreature.localRotation;
        _playerCreature.transform.root.gameObject.SetActive(true);

        _playerCreature.GetComponent<PossessedCreature>().SelectCreature(_targetCreature);

        _targetCreature.gameObject.SetActive(false);

        _playerCreature.transform.gameObject.SetActive(false);
        isPossessedState = false;
        //no need of creature as player already have creature information
        this.transform.root.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, _capturingRadius);
    }
#endif
}
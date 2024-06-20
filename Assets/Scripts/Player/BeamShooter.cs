using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct BeamCoolDownUI
{
    public Transform Panel;
    public TextMeshProUGUI TxtCoolDown;
    public Image ImgLoading;
}
public class BeamShooter : MonoBehaviour
{
    [SerializeField] AnimationCurve _bulletSpeed;
    [SerializeField] private Transform _bullet;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _bulletCoolDownInterval;

    [SerializeField] private float _bulletMaxDistance;

    private float _currentTime = 0;

    [SerializeField] private BeamCoolDownUI _beamCoolDownUI;



    private Transform _targetToShoot;


    private void Awake()
    {
        _currentTime = Time.time - _bulletCoolDownInterval;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }
    // Use to shoot the target
    private void Shoot()
    {
        //Cool down interval
        if (Time.time - _bulletCoolDownInterval <= _currentTime) { return; }
        //when shooting start, then it can not shoot until cool down interval passed!
        _currentTime = Time.time;

        //Start Releasing Beam
        StartCoroutine(ReleasedBeam());

    }
    private IEnumerator CoolDownInterval()
    {
        //Initialize
        _beamCoolDownUI.TxtCoolDown.text = _bulletCoolDownInterval.ToString() + ".00";
        _beamCoolDownUI.ImgLoading.fillAmount = 1;


        _beamCoolDownUI.Panel.gameObject.SetActive(true);
        float timeElapsed = _bulletCoolDownInterval;
        while (timeElapsed >= 0)
        {
            timeElapsed -= Time.deltaTime;
            float roundedTime = Mathf.Round(timeElapsed * 10) / 10;
            _beamCoolDownUI.TxtCoolDown.text = roundedTime.ToString();
            _beamCoolDownUI.ImgLoading.fillAmount = timeElapsed / _bulletCoolDownInterval;
            yield return null;
        }
        _beamCoolDownUI.Panel.gameObject.SetActive(false);


    }
    //beam shoot logic
    private IEnumerator ReleasedBeam()
    {


        float newLength;
        float currentTime = 0;
        while (_bulletMaxDistance - _bullet.transform.localScale.y > 0.01f)
        {
            FindAndLookAtTarget();
            currentTime += Time.deltaTime;
            newLength = Mathf.Lerp(_bullet.transform.localScale.y, _bulletMaxDistance, _bulletSpeed.Evaluate(currentTime) * Time.deltaTime);
            _bullet.transform.localScale = new Vector3(_bullet.transform.localScale.x, newLength, _bullet.transform.localScale.z);
            yield return null;
        }
        newLength = 0;
        _bullet.transform.localScale = new Vector3(_bullet.transform.localScale.x, newLength, _bullet.transform.localScale.z);

        //reset Time for cool down
        _currentTime = Time.time;
        _targetToShoot = null;

        StartCoroutine(CoolDownInterval());

    }
    // look at target while attacking
    void FindAndLookAtTarget()
    {
        _targetToShoot = FindTargetInRange();

        if (_targetToShoot)
            this.transform.LookAt(_targetToShoot);

    }
    //Find target in give radius of target layer 
    Transform FindTargetInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _attackRadius, _targetLayer);
        if (colliders?.Length > 0)
            return colliders[0].transform;
        return null;

    }
    //will run if game execute in untiy editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);
    }
#endif
}

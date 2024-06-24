using System.Collections;
using UnityEngine;
public class BeamShooter : MonoBehaviour
{
    [SerializeField] AnimationCurve _bulletSpeed;
    [SerializeField] private Transform _beam;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _bulletCoolDownInterval;

    [SerializeField] private float _bulletMaxDistance;

    private float _currentTime = 0;

    [SerializeField] private CoolDownUI _beamCoolDownUI;



    private Transform _targetToShoot;


    private void OnEnable()
    {
        _currentTime = Time.time - _bulletCoolDownInterval;
    }
    private void OnDisable()
    {
        //Turn off CoolDown UI if enable
        _beamCoolDownUI.Panel.gameObject.SetActive(false);

    }
    private void Update()
    {
        //if left mouse click
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
        _beam.GetComponentInChildren<CapsuleCollider>().enabled = true;
        //Start Releasing Beam
        StartCoroutine(ReleasedBeam());

    }

    //beam shoot logic
    private IEnumerator ReleasedBeam()
    {


        float newLength;
        float currentTime = 0;
        while (_bulletMaxDistance - _beam.transform.localScale.y > 0.01f)
        {
            FindAndLookAtTarget();
            currentTime += Time.deltaTime;
            newLength = Mathf.Lerp(_beam.transform.localScale.y, _bulletMaxDistance, _bulletSpeed.Evaluate(currentTime) * Time.deltaTime);
            _beam.transform.localScale = new Vector3(_beam.transform.localScale.x, newLength, _beam.transform.localScale.z);
            yield return null;
        }
        newLength = 0;
        _beam.transform.localScale = new Vector3(_beam.transform.localScale.x, newLength, _beam.transform.localScale.z);

        //reset Time for cool down
        _currentTime = Time.time;
        _targetToShoot = null;
        _beam.GetComponentInChildren<CapsuleCollider>().enabled = false;

        StartCoroutine(CoolDownInterval());

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
    // look at target while attacking
    void FindAndLookAtTarget()
    {
        //Find target in give radius of target layer 
        _targetToShoot = Finder.FindFirstTargetInRange(this.transform.position, _attackRadius, _targetLayer);
        if (_targetToShoot)
            this.transform.LookAt(_targetToShoot);

    }

    //will run if game execute in untiy editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);
    }
#endif
}

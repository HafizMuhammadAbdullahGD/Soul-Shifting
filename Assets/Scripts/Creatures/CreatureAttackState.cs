using UnityEngine;

public class CreatureAttackState : StateMachine
{
    [SerializeField] private int _animationLayerIndex;
    [SerializeField] private string _animationCondition;
    [SerializeField] private float _targetInRadius;
    [SerializeField] private float _attackDamageRate;
    [SerializeField] private LayerMask _targetLayer;
    private int _animationID;
    private Animator _animator;
    private Transform _player;
    bool isDamage;
    private void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = _player.GetComponent<Animator>();
        _animationID = Animator.StringToHash(_animationCondition);
    }
    protected override void Enter()
    {
        isDamage = false;
        _animator.SetBool(_animationID, true);
        _animator.SetLayerWeight(_animationLayerIndex, 1);
        base.Tick();
    }
    protected override void Tick()
    {
        if (IsPunched() && SmoothlyInterpolateLayer())
        {
            base.Exit();
        }
    }
    protected override void Exit()
    {
        _animator.SetLayerWeight(_animationLayerIndex, 0);
        //reset the transition to enter for next time
        base.Enter();
    }
    bool SmoothlyInterpolateLayer()
    {
        float newWeight = Mathf.Lerp(_animator.GetLayerWeight(_animationLayerIndex), 0, 35 * Time.deltaTime);
        _animator.SetLayerWeight(_animationLayerIndex, newWeight);
        return newWeight <= 0.01f;
    }
    bool IsPunched()
    {
        float _animationProgress = _animator.GetCurrentAnimatorStateInfo(_animationLayerIndex).normalizedTime;
        //if animation done about 90%
        if (_animationProgress >= 0.9f)
        {
            _animator.SetBool(_animationID, false);
            return true;
        }
        if (_animationProgress >= 0.70 && !isDamage)
        {
            //attack on near target if found
            FindTargetInRange()?.GetComponent<IDamage>()?.OnDamage(_attackDamageRate);
            isDamage = true;
        }
        return false;
    }
    private Transform FindTargetInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(_player.transform.position, _targetInRadius, _targetLayer);
        if (colliders?.Length > 0)
            return colliders[0].transform;
        return null;

    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_player.transform.position, _targetInRadius);
    }
#endif
}
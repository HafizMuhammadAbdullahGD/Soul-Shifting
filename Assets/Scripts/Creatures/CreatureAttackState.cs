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
    bool isAttacked;
    private void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = _player.GetComponent<Animator>();
        _animationID = Animator.StringToHash(_animationCondition);
    }
    protected override void Enter()
    {
        isAttacked = false;
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
        float animationProgress = _animator.GetCurrentAnimatorStateInfo(_animationLayerIndex).normalizedTime;
        Transform target = null;
        //if animation done about 90%
        if (animationProgress >= 0.9f)
        {
            _animator.SetBool(_animationID, false);
            return true;
        }
        if (animationProgress >= 0.30)
        {
            target = Finder.FindFirstTargetInRange(_player.transform.position, _targetInRadius, _targetLayer);

            if (target)
                _player.transform.LookAt(target);

        }
        if (animationProgress >= 0.70 && !isAttacked)
        {
            //attack on near target if found
            target?.GetComponent<IDamage>()?.OnDamage(_attackDamageRate);
            isAttacked = true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (_player)
            Gizmos.DrawWireSphere(_player.transform.position, _targetInRadius);
    }
#endif
}
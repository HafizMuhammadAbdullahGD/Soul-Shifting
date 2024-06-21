using UnityEngine;

public class CreatureAttackState : StateMachine
{
    [SerializeField] private int _animationLayerIndex;
    [SerializeField] private string _animationCondition;
    private int _animationID;
    private Animator _animator;
    private Transform _player;
    private void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = _player.GetComponent<Animator>();
        _animationID = Animator.StringToHash(_animationCondition);
    }
    protected override void Enter()
    {

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
        //if animation done about 90%
        if (_animator.GetCurrentAnimatorStateInfo(_animationLayerIndex).normalizedTime >= 0.9f)
        {
            _animator.SetBool(_animationID, false);
            return true;
        }
        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Enter, Tick, Exit
}
public class StateMachine : MonoBehaviour
{
    private State _currentState;
    StateMachine _currentStateMachine;
    protected virtual void Enter() { _currentState = State.Enter; }
    protected virtual void Tick() { _currentState = State.Tick; }
    protected virtual void Exit() { _currentState = State.Exit; }
    public StateMachine Execute()
    {
        if (_currentState == State.Enter)
        {
            Enter();
        }
        else if (_currentState == State.Tick)
        {
            Tick();
        }
        else if (_currentState == State.Exit)
        {
            _currentStateMachine = null;
            Exit();

        }
        return _currentStateMachine;
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState CurrentState { get; private set; }
    public event Action<BaseState> OnStateChanged;
    
    public void SetStates(Dictionary<Type, BaseState> states)
    {
        availableStates = states;
    }

    
    void Update()
    {
        if(CurrentState == null)
        {
            CurrentState = availableStates.Values.First();
        }
        var nextState = CurrentState?.Tick();

        if(nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState);
        }
    }
    private void SwitchToNewState(Type nextState)
    {
        CurrentState = availableStates[nextState];
        OnStateChanged?.Invoke(CurrentState);

    }
}

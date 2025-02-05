using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocitySM
{
    private StateNode _currentStateNode;
    private Dictionary<Type, StateNode> _stateNodes = new();

    //check for any Transition to a new State; invoke the Update() function of the current State
    public void Update()
    {
        var transition = GetTransition();

        if (transition != null)
        {
            ChangeState(transition.TargetState);
        }
        
        _currentStateNode.State?.Update();
    }

    //Invoke the FixedUpdate() function of the current State
    public void FixedUpdate()
    {
        _currentStateNode.State?.FixedUpdate();
    }

    public void SetState(IState targetState)
    {
        _currentStateNode = _stateNodes[targetState.GetType()];
        _currentStateNode.State?.OnStateEnter();
        
    }

    private void ChangeState(IState targetState)
    {
        if (targetState == _currentStateNode.State)
        {
            return;
        }
        
        var previousState = _currentStateNode.State;
        var nextState = _stateNodes[targetState.GetType()].State;
        
        previousState?.OnStateExit();
        nextState?.OnStateEnter();
        _currentStateNode = _stateNodes[targetState.GetType()];
    }

    private ITransition GetTransition()
    {
        foreach (var transition in _currentStateNode.Transitions)
        {
            if (transition.Condition.Evaluate())
            {
                return transition;
            }
        }
        
        return null;
    }

    public void AddNormalTransition(IState fromState, IState targetState, IPredicate condition)
    {
        GetOrAddStateNode(fromState).AddNormalTransition(GetOrAddStateNode(targetState).State, condition);
    }

    //function to get a State Node; if value is null, creates a State Node instead
    private StateNode GetOrAddStateNode(IState state)
    {
        var stateNode = _stateNodes.GetValueOrDefault(state.GetType());

        if (stateNode == null)
        {
            stateNode = new StateNode(state);
            _stateNodes.Add(state.GetType(), stateNode);
        }
        
        return stateNode;
    }
    
    //This is basically just a State with all its valid Transitions
    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddNormalTransition(IState nextState, IPredicate condition)
        {
            Transitions.Add(new NormalTransition(nextState, condition));
        }
    }
}

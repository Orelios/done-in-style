using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private StateNode _currentStateNode;
    public IState CurrentState => _currentStateNode.State;
    private Dictionary<Type, StateNode> _stateNodes = new();
    private HashSet<ITransition> _anyTransitions = new();

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

    public IState GetCurrentState()
    {
        return _currentStateNode.State;
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
        foreach (var anyTransition in _anyTransitions)
        {
            if (anyTransition.Condition.Evaluate())
            {
                return anyTransition;
            }
        }
        
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
        GetOrAddStateNode(fromState).AddTransition(GetOrAddStateNode(targetState).State, condition);
    }

    public void AddAnyTransition(IState targetState, IPredicate condition)
    {
        _anyTransitions.Add(new Transition(GetOrAddStateNode(targetState).State, condition));
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

        public void AddTransition(IState nextState, IPredicate condition)
        {
            Transitions.Add(new Transition(nextState, condition));
        }
    }
}

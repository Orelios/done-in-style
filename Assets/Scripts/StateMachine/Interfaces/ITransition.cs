using UnityEngine;

//for transitioning States 
public interface ITransition
{
    //what State to transition to
    IState TargetState { get; }
    //what condition/s to check for a valid transition
    IPredicate Condition { get; }
}

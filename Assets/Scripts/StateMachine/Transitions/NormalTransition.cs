using UnityEngine;

public class NormalTransition : ITransition
{
    public IState TargetState { get; }
    public IPredicate Condition { get; }

    public NormalTransition(IState targetState, IPredicate condition)
    {
        TargetState = targetState;
        Condition = condition;
    }
}

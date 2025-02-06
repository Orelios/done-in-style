using UnityEngine;

//for checking conditions when transitioning States
public interface IPredicate
{
    //the condition checker in question
    bool Evaluate();
}

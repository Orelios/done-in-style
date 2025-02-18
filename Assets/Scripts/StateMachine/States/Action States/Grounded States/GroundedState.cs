using System.Collections.Generic;
using UnityEngine;

public class GroundedState : ActionState
{
    public GroundedState(Player player) : base(player)
    {
        
    }

    /*public override void OnStateEnter()
    {
        Debug.Log($"Entering {GetType().BaseType.Name} -> {GetType().Name}");
        SuperStateName = GetType().BaseType.Name;
        SubStateName = GetType().Name;
    }*/
}

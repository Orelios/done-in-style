using System.Collections.Generic;
using UnityEngine;

public class SpecialState : ActionState
{
    public SpecialState(Player player, params IState[] subStates) : base(player)
    {

    }
}

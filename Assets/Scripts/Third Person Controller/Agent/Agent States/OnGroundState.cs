using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnGroundState : AgentState
{
    RaycastHit rayHit;

    protected OnGroundState(GameObject gameObject) : base(gameObject)
    {
        transitionsTo.Add(new Transition(typeof(Crouching), Crouch, Not(Move)));
        transitionsTo.Add(new Transition(typeof(Jumping), Jump, OnGround, Not(Block), () => stamina.CurrentStamina >= agentStats.jumpCost));
        transitionsTo.Add(new Transition(typeof(Rolling), Block, Jump, Move));
    }

    protected void KeepGrounded()
    {
        if (OnGround() && Not(Jump)())
        {
            self.SetVerticalVelocity(-10f);
        }
    }
}

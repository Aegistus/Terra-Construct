using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : AgentState
{

    public Crouching(GameObject gameObject) : base(gameObject)
    {
        animationHash = Animator.StringToHash("Crouching");
        transitionsTo.Add(new Transition(typeof(Idling), Not(Crouch)));
    }

    public override void AfterExecution()
    {
        anim.SetBool(animationHash, false);
        //transform.position += Vector3.down * crouchHeight;
    }

    public override void BeforeExecution()
    {
        Debug.Log("Crouching");
        anim.SetBool(animationHash, true);
        self.SetHorizontalVelocity(Vector3.zero);
        //transform.position -= Vector3.down * crouchHeight;
    }

    public override void DuringExecution()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipping : AgentState
{
    private bool animationDone = false;

    public Equipping(GameObject gameObject) : base(gameObject)
    {
        animationHash = Animator.StringToHash("Equipping");
        transitionsTo.Add(new Transition(typeof(Idling), () => animationDone));
        weapons = gameObject.GetComponent<AgentWeapons>();
        animEvents = gameObject.GetComponentInChildren<AgentAnimEvents>();
    }

    private void EnableNewWeapon(EventType eventType)
    {
        if (eventType == EventType.Finish)
        {
            animationDone = true;
        }
    }

    public override void AfterExecution()
    {
        anim.SetBool(animationHash, false);
        animEvents.OnAnimationEvent -= EnableNewWeapon;
    }

    public override void BeforeExecution()
    {
        weapons.EquipWeapon(controller.WeaponNumKey);
        anim.SetBool(animationHash, true);
        animationDone = false;
        animEvents.OnAnimationEvent += EnableNewWeapon;
        self.SetHorizontalVelocity(self.Velocity * .5f);
        audioManager.PlaySoundAtPosition("Blade Equip", transform.position);
    }

    public override void DuringExecution()
    {

    }
}

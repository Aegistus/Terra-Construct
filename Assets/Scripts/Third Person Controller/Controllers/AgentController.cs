using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AgentController : MonoBehaviour
{
    public bool Attack { get; protected set; }
    public bool Block { get; protected set; }
    public bool Forwards { get; protected set; }
    public bool Backwards { get; protected set; }
    public bool Left { get; protected set; }
    public bool Right { get; protected set; }
    public bool Jump { get; protected set; }
    public bool Crouch { get; protected set; }
    public bool Run { get; protected set; }
    public bool Equipping { get; protected set; }
    public int WeaponNumKey { get; protected set; }
    public bool UnEquipping { get; protected set; }
    public Ray Aim { get; protected set; }
}

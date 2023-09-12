using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Playergravityswitch
{
    public Playerstatemachine psm;
    public static Action platformrotate;

    public void playerswitchgravity()
    {
        if (psm.controlls.Player.Gravityswitch.WasPerformedThisFrame())
        {
            psm.switchgravityactiv = !psm.switchgravityactiv;
            psm.transform.Rotate(180, 0, 0);
            psm.rb.gravityScale *= -1;
            platformrotate?.Invoke();
        }
    }
}

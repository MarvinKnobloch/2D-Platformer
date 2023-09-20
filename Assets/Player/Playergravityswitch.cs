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
        if (psm.controlls.Player.Gravityswitch.WasPerformedThisFrame() && Globalcalls.currentgravitystacks > 0)
        {
            Globalcalls.currentgravitystacks--;
            Cooldowns.instance.handlegravitystacks();
            psm.switchgravityactiv = !psm.switchgravityactiv;
            psm.transform.Rotate(180, 0, 0);
            psm.rb.gravityScale *= -1;
            platformrotate?.Invoke();
        }
    }
    public void triggerplatformrotate()
    {
        platformrotate?.Invoke();
    }
}

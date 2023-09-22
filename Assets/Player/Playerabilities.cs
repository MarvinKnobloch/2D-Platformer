using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerabilities
{
    public Playerstatemachine psm;
    private float currentclosestdistance;
    private float hookmaxdistance = 5;
    private float playerangle;

    public void playercheckforhook()
    {
        if (psm.controlls.Player.Hook.WasPerformedThisFrame())
        {
            if(Hookobject.hookobjects.Count > 0)
            {
                checkforclosesthook();
                if(currentclosestdistance < hookmaxdistance)
                {
                    hookplayer();
                    //psm.state = Playerstatemachine.States.Hook;
                }
            }
        }
    }
    private void checkforclosesthook()
    {
        currentclosestdistance = 100;
        float objectdistance;
        for (int i = 0; i < Hookobject.hookobjects.Count; i++)
        {
            objectdistance = Vector3.Distance(psm.transform.position, Hookobject.hookobjects[i].transform.position);
            if (currentclosestdistance > objectdistance)
            {
                currentclosestdistance = objectdistance;
                psm.hooktarget = Hookobject.hookobjects[i];                
            }
        }
    }
    private void hookplayer()
    {
        if(psm.transform.position.x < psm.hooktarget.transform.position.x)
        {
            Vector3 angleposition = psm.hooktarget.transform.position - psm.transform.position;
            playerangle = Vector2.Angle(angleposition, Vector2.up);
            Quaternion hookangle = Quaternion.Euler(0, 0, 90 - playerangle);
            psm.hookpositon = hookangle * Vector2.up * 1.5f;
            psm.transform.position = psm.hookpositon + psm.hooktarget.transform.position;   
        }
        else
        {
            Vector3 angleposition = psm.hooktarget.transform.position - psm.transform.position;
            playerangle = Vector2.Angle(angleposition, Vector2.up);
            Quaternion hookangle = Quaternion.Euler(0, 0, playerangle - 90);
            psm.hookpositon = hookangle * Vector2.up * 1.5f;
            psm.transform.position = psm.hookpositon + psm.hooktarget.transform.position;
        }
    }
}

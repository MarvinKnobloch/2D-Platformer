using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerabilities
{
    public Playerstatemachine psm;
    private float currentclosestdistance;
    private float hookmaxdistance = 20;
    private float playerangle;
    private bool playerisonleftsideofhookobject;
    private bool playerisrightofhookendposition;

    public void playercheckforhook()
    {
        if (psm.controlls.Player.Hook.WasPerformedThisFrame() && psm.inhookstate == false)
        {
            if(Hookobject.hookobjects.Count > 0)
            {
                checkforclosesthook();
                if(currentclosestdistance < hookmaxdistance)
                {
                    hookplayer();
                    psm.state = Playerstatemachine.States.Hook;
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
        psm.inhookstate = true;
        psm.hookstartposition = psm.transform.position;
        psm.hookstarttime = 0;
        psm.rb.gravityScale = 0;
        psm.rb.velocity = Vector2.zero;
        if (psm.transform.position.x < psm.hooktarget.transform.position.x)
        {
            Vector3 angleposition = psm.hooktarget.transform.position - psm.transform.position;
            playerangle = Vector2.Angle(angleposition, Vector2.up);
            Quaternion hookangle = Quaternion.Euler(0, 0, 60 - playerangle);
            psm.hookendposition = hookangle * Vector2.up * psm.hookdistancetoobject;
            psm.hookendposition += psm.hooktarget.transform.position;
            playerisonleftsideofhookobject = true;
            if (psm.transform.position.x > psm.hookendposition.x)
            {
                playerisrightofhookendposition = true;
            }
            else playerisrightofhookendposition = false;
        }
        else
        {
            Vector3 angleposition = psm.hooktarget.transform.position - psm.transform.position;
            playerangle = Vector2.Angle(angleposition, Vector2.up);
            Quaternion hookangle = Quaternion.Euler(0, 0, playerangle - 60);
            psm.hookendposition = hookangle * Vector2.up * psm.hookdistancetoobject;
            psm.hookendposition += psm.hooktarget.transform.position;
            playerisonleftsideofhookobject = false;
            if (psm.transform.position.x > psm.hookendposition.x)
            {
                playerisrightofhookendposition = true;
            }
            else playerisrightofhookendposition = false;
        }
    }
    public void movetohookposition()
    {
        Vector3 center = (psm.hookstartposition + psm.hookendposition) * 0.5f;
        if(playerisonleftsideofhookobject == true)
        {
            if (playerisrightofhookendposition == true)
            {
                center -= new Vector3(0, psm.hookradius);
            }
            else center -= new Vector3(0, psm.hookradius * -1);
        }
        else
        {
            if (playerisrightofhookendposition == true)
            {
                center -= new Vector3(0, psm.hookradius * -1);
            }
            else center -= new Vector3(0, psm.hookradius);
        }

        Vector3 startRelcenter = psm.hookstartposition - center;
        Vector3 endRelcenter = psm.hookendposition - center;

        psm.hookstarttime += Time.deltaTime;
        float fracComplete = psm.hookstarttime / (psm.flathookduration + currentclosestdistance * psm.distancespeedmultiplier);

        psm.rb.position = Vector3.Slerp(startRelcenter, endRelcenter, fracComplete) + center;
        //psm.transform.position = newposi;
        //psm.rb.position = Vector3.MoveTowards(psm.transform.position, newposi, 30 * Time.deltaTime);
        if(Vector3.Distance(psm.transform.position, psm.hookendposition) < 0.3f)
        {
            psm.inhookstate = false;
            psm.switchtoairstate();
        }
    }
}

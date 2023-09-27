using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerhook
{
    public Playerstatemachine psm;
    private float currentclosestdistance;
    private float hookmaxdistance = 20;
    private float playerangle;
    private Quaternion hookangle;
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
        Hookobject.hookobjects.Remove(psm.hooktarget);
        psm.hooktarget.GetComponent<SpriteRenderer>().color = Color.red;
        psm.inhookstate = true;
        psm.hookstartposition = psm.transform.position;
        psm.hookstarttime = 0;
        psm.rb.gravityScale = 0;
        psm.rb.velocity = Vector2.zero;
        if (psm.transform.position.x < psm.hooktarget.transform.position.x)
        {
            Vector3 angleposition = psm.hooktarget.transform.position - psm.transform.position;
            playerangle = 70 - Vector2.Angle(angleposition, Vector2.up);
            hookangle = Quaternion.Euler(0, 0, playerangle);
            psm.hookdistancetoobject = Vector3.Distance(psm.hooktarget.transform.position, psm.transform.position) * 1f;
            if (psm.hookdistancetoobject > psm.maxhookdistanceradius) psm.hookdistancetoobject = psm.maxhookdistanceradius;
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
            playerangle = Vector2.Angle(angleposition, Vector2.up) - 70;
            hookangle = Quaternion.Euler(0, 0, playerangle);
            psm.hookdistancetoobject = Vector3.Distance(psm.hooktarget.transform.position, psm.transform.position) * 1f;
            if (psm.hookdistancetoobject > psm.maxhookdistanceradius) psm.hookdistancetoobject = psm.maxhookdistanceradius;
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

        psm.rb.MovePosition(Vector3.Slerp(startRelcenter, endRelcenter, fracComplete) + center);

        if(Vector3.Distance(psm.transform.position, psm.hookendposition) < 0.1f)
        {
            //playerangle *= Mathf.Deg2Rad;
            //float xComponent = Mathf.Cos(playerangle) * psm.hookreleaseforce;
            //float zComponent = Mathf.Sin(playerangle) * psm.hookreleaseforce;
            //Vector3 forceApplied = new Vector3(xComponent, 0, zComponent);
            //psm.rb.AddForce(forceApplied, ForceMode2D.Impulse);

            //Vector3 tpoint = psm.transform.position + Quaternion.Euler(0, 0, 100) * Vector2.up * 1;
            //psm.rb.AddForce(tpoint * psm.hookreleaseforce, ForceMode2D.Impulse);

            Vector3 direction;
            if (playerisonleftsideofhookobject == true) direction = Quaternion.AngleAxis(playerangle - 70, Vector3.forward) * Vector3.up;
            else direction = Quaternion.AngleAxis(playerangle + 70, Vector3.forward) * Vector3.up;
            psm.rb.AddForce(direction * (currentclosestdistance + 10), ForceMode2D.Impulse);

            psm.xvelocityafterhook = psm.rb.velocity.x;

            psm.rb.gravityScale = 2;
            psm.inhookstate = false;
            psm.state = Playerstatemachine.States.Hookrelease;
        }
        if(psm.hookstarttime > 0.1f + psm.flathookduration + (currentclosestdistance * psm.distancespeedmultiplier))
        {
            psm.inhookstate = false;
            psm.switchtoairstate();
        }
    }
    public void hookreleasemovement()
    {
        if(playerisonleftsideofhookobject == true)
        {
            if (psm.xvelocityafterhook > 0.2f)
            {
                psm.xvelocityafterhook -= psm.xvelocityafterhook * 0.05f;
                psm.rb.velocity = new Vector2((psm.move.x * (psm.movementspeed * 0.8f)) + psm.xvelocityafterhook, psm.rb.velocity.y);
            }
            else
            {
                psm.switchtoairstate();
            }
        }
        else
        {
            if (psm.xvelocityafterhook < -0.2f)
            {
                psm.xvelocityafterhook -= psm.xvelocityafterhook * 0.05f;
                psm.rb.velocity = new Vector2((psm.move.x * (psm.movementspeed * 0.8f)) + psm.xvelocityafterhook, psm.rb.velocity.y);
            }
            else
            {
                psm.switchtoairstate();
            }
        }

    }
}

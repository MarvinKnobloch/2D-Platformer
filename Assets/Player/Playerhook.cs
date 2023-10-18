using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerhook
{
    public Playerstatemachine psm;
    private float hooktargetupdatetime;
    private float currentclosestdistance;
    private Vector3 angleposition;
    private float playerangle;
    private Quaternion hookangle;
    private float hookgravityscale = 3;
    private bool playerisonleftsideofhookobject;
    private bool playerisrightofhookendposition;

    private bool addvelocity;

    public void playercheckforhook()
    {
        if (Hookobject.hookobjects.Count > 0 && psm.inhookstate == false)
        {
            hooktargetupdate();
            if (psm.controlls.Player.Hook.WasPerformedThisFrame())
            {
                checkforclosesthook();
                hookplayer();
                addvelocity = true;
                psm.state = Playerstatemachine.States.Hook;
            }
        }
    }
    private void hooktargetupdate()
    {
        hooktargetupdatetime += Time.deltaTime;
        if (hooktargetupdatetime > 0.2f)
        {
            checkforclosesthook();
        }
    }
    public void checkforclosesthook()
    {
        hooktargetupdatetime = 0;
        if(psm.inhookstate == false)
        {
            currentclosestdistance = 100;
            float objectdistance;
            for (int i = 0; i < Hookobject.hookobjects.Count; i++)
            {
                Hookobject.hookobjects[i].GetComponent<SpriteRenderer>().color = Color.red;
                objectdistance = Vector3.Distance(psm.transform.position, Hookobject.hookobjects[i].transform.position);
                if (currentclosestdistance > objectdistance)
                {
                    currentclosestdistance = objectdistance;
                    psm.hooktarget = Hookobject.hookobjects[i];
                }
            }
            psm.hooktarget.GetComponent<SpriteRenderer>().color = Color.green;
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
            if (psm.gravityswitchactiv == false)
            {
                angleposition = psm.hooktarget.transform.position - psm.transform.position;
                playerangle = 90 - psm.hookangle - Vector2.Angle(angleposition, Vector2.up);
                hookplayernormalgravity();
            }
            else
            {
                angleposition = psm.hooktarget.transform.position - psm.transform.position;
                playerangle = 90 + psm.hookangle - Vector2.Angle(angleposition, Vector2.up);
                hookplayerreversegravity();
            }

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
            if (psm.gravityswitchactiv == false)
            {
                angleposition = psm.hooktarget.transform.position - psm.transform.position;
                playerangle = Vector2.Angle(angleposition, Vector2.up) - (90 - psm.hookangle);
                hookplayernormalgravity();
            }
            else
            {
                angleposition = psm.hooktarget.transform.position - psm.transform.position;
                playerangle = Vector2.Angle(angleposition, Vector2.up) - (90 + psm.hookangle);
                hookplayerreversegravity();
            }

            psm.hookendposition += psm.hooktarget.transform.position;
            playerisonleftsideofhookobject = false;
            if (psm.transform.position.x > psm.hookendposition.x)
            {
                playerisrightofhookendposition = true;
            }
            else playerisrightofhookendposition = false;
        }
    }
    private void hookplayernormalgravity()
    {
        hookangle = Quaternion.Euler(0, 0, playerangle);
        psm.hookdistancetoobject = Vector3.Distance(psm.hooktarget.transform.position, psm.transform.position) * 1f;
        if (psm.hookdistancetoobject > psm.maxhookdistanceradius) psm.hookdistancetoobject = psm.maxhookdistanceradius;
        psm.hookendposition = hookangle * Vector2.up * psm.hookdistancetoobject;
    }
    private void hookplayerreversegravity()
    {
        hookangle = Quaternion.Euler(0, 0, playerangle);
        psm.hookdistancetoobject = Vector3.Distance(psm.hooktarget.transform.position, psm.transform.position) * 1f;
        if (psm.hookdistancetoobject > psm.maxhookdistanceradius) psm.hookdistancetoobject = psm.maxhookdistanceradius;
        psm.hookendposition = hookangle * Vector2.down * psm.hookdistancetoobject;
    }
    public void movetohookposition()
    {
        psm.hookstarttime += Time.deltaTime;
        if (Vector3.Distance(psm.transform.position, psm.hookendposition) < 1f && addvelocity == true)
        {
            addvelocity = false;
            //Vector3 tpoint = psm.transform.position + Quaternion.Euler(0, 0, 100) * Vector2.up * 1;
            //psm.rb.AddForce(tpoint * psm.hookreleaseforce, ForceMode2D.Impulse);

            if (psm.gravityswitchactiv == false)
            {
                Vector3 direction;
                if (playerisonleftsideofhookobject == true) direction = Quaternion.AngleAxis(playerangle - (90 - psm.hookangle), Vector3.forward) * Vector3.up;
                else direction = Quaternion.AngleAxis(playerangle + (90 - psm.hookangle), Vector3.forward) * Vector3.up;
                psm.rb.AddForce(direction * (currentclosestdistance + psm.hookreleaseforce), ForceMode2D.Impulse);
                psm.rb.gravityScale = hookgravityscale;
            }
            else
            {
                Vector3 direction;
                if (playerisonleftsideofhookobject == true) direction = Quaternion.AngleAxis(playerangle - (90 + psm.hookangle), Vector3.forward) * Vector3.up;
                else direction = Quaternion.AngleAxis(playerangle + (90 + psm.hookangle), Vector3.forward) * Vector3.up;
                psm.rb.AddForce(direction * (currentclosestdistance + psm.hookreleaseforce), ForceMode2D.Impulse);
                psm.rb.gravityScale = hookgravityscale * -1;
            }
        }
        if(psm.rb.velocity == Vector2.zero)
        {
            Vector3 center = (psm.hookstartposition + psm.hookendposition) * 0.5f;
            if (playerisonleftsideofhookobject == true)
            {
                if (psm.gravityswitchactiv == false)
                {
                    if (playerisrightofhookendposition == true) center -= multipleradius(1);
                    else center -= multipleradius(-1);
                }
                else
                {
                    if (playerisrightofhookendposition == true) center -= multipleradius(-1);
                    else center -= multipleradius(1);
                }
            }
            else
            {
                if (psm.gravityswitchactiv == false)
                {
                    if (playerisrightofhookendposition == true) center -= multipleradius(-1);
                    else center -= multipleradius(1);
                }
                else
                {
                    if (playerisrightofhookendposition == true) center -= multipleradius(1);
                    else center -= multipleradius(-1);
                }
            }

            Vector3 startRelcenter = psm.transform.position - center;
            Vector3 endRelcenter = psm.hookendposition - center;

            float fracComplete = psm.hookstarttime / (psm.flathookduration + currentclosestdistance * psm.distancespeedmultiplier);

            psm.rb.MovePosition(Vector3.Slerp(startRelcenter, endRelcenter, fracComplete) + center);
        }
        else
        {
            psm.xvelocityafterhook = psm.rb.velocity.x;
            psm.inhookstate = false;
            psm.state = Playerstatemachine.States.Hookrelease;
        }


        if (psm.hookstarttime > 0.5f)//0.1f + psm.flathookduration + (currentclosestdistance * psm.distancespeedmultiplier))
        {
            psm.inhookstate = false;
            psm.switchtoairstate();
        }
    }
    private Vector3 multipleradius(float multiplier)
    {
        return new Vector3(0, psm.hookradius * multiplier);
    }
    public void hookreleasemovement()
    {
        if (playerisonleftsideofhookobject == true)
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

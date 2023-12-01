using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerwhip
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

    private int percision = 40;
    private float straightenLineSpeed = 10;
    private float StartWaveSize = 2;
    private float waveSize = 0;
    private Vector3 whipdistance;

    private float whipProgressionSpeed = 11;

    float movetime = 0;
    private bool drawwhipwaves;

    private bool addvelocity;

    const string whipstartstate = "Whipstart";
    const string jumpnosoundstate = "Jumpnosound";
    const string jumptofallstate = "Jumptofall";
    const string fallstate = "Fall";

    public void playercheckforhook()
    {
        if (Hookobject.hookobjects.Count > 0 && psm.inhookstate == false)
        {
            hooktargetupdate();
            if (psm.controlls.Player.Whip.WasPerformedThisFrame())
            {
                checkforclosesthook();
                hookplayer();
                activatelinerenderer();

                addvelocity = true;
                psm.inair = true;
                Globalcalls.jumpcantriggerswitch = true;
                psm.ChangeAnimationState(whipstartstate);
                psm.state = Playerstatemachine.States.Throwwhip;
            }
        }
    }
    private void activatelinerenderer()
    {
        psm.lineRenderer.positionCount = percision;
        for (int i = 0; i < percision; i++)
        {
            psm.lineRenderer.SetPosition(i, psm.whipstartpoint.position);
        }
        movetime = 0;
        drawwhipwaves = true;
        psm.lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        whipdistance = psm.hooktarget.transform.position - psm.whipstartpoint.position;
        psm.lineRenderer.enabled = true;
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
        if (Hookobject.hookobjects.Count > 0)
        {
            if (psm.inhookstate == false)
            {
                currentclosestdistance = 100;
                float objectdistance;
                for (int i = 0; i < Hookobject.hookobjects.Count; i++)
                {
                    Hookobject.hookobjects[i].GetComponent<SpriteRenderer>().color = Color.white;
                    objectdistance = Vector3.Distance(psm.transform.position, Hookobject.hookobjects[i].transform.position);
                    if (currentclosestdistance > objectdistance)
                    {
                        currentclosestdistance = objectdistance;
                        psm.hooktarget = Hookobject.hookobjects[i];
                    }
                }
                psm.hooktarget.GetComponent<SpriteRenderer>().color = psm.whippointcolor;
            }
        }
        else
        {
            if(psm.lineRenderer.enabled == false) psm.hooktarget = null;
        }
    }
    private void hookplayer()
    {
        Hookobject.hookobjects.Remove(psm.hooktarget);
        psm.hooktarget.GetComponent<SpriteRenderer>().color = Color.white;
        psm.inhookstate = true;
        psm.hookstartposition = psm.transform.position;
        psm.hookstarttime = 0;
        psm.rb.gravityScale = 0;
        psm.rb.velocity = Vector2.zero;
        if (psm.transform.position.x < psm.hooktarget.transform.position.x)
        {
            if(psm.faceright == true)
            {
                psm.faceright = !psm.faceright;
                psm.transform.Rotate(0, 180, 0);
            }
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
            if (psm.faceright == false)
            {
                psm.faceright = !psm.faceright;
                psm.transform.Rotate(0, 180, 0);
            }
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
        if (psm.rb.velocity == Vector2.zero)
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
            if(psm.gravityswitchactiv == false)
            {
                if (psm.rb.velocity.y > 1.5f) psm.ChangeAnimationState(jumpnosoundstate);
                else psm.ChangeAnimationState(fallstate);
            }
            else
            {
                if (psm.rb.velocity.y < -1.5f) psm.ChangeAnimationState(jumpnosoundstate);
                else psm.ChangeAnimationState(fallstate);
            }
            if (psm.lineRenderer.positionCount != 2) { psm.lineRenderer.positionCount = 2; }
            psm.lineRenderer.enabled = false;
            if (Hookobject.hookobjects.Count == 0) psm.hooktarget = null;
            psm.state = Playerstatemachine.States.Whiprelease;
        }

        if (psm.hookstarttime > 0.5f)
        {
            psm.lineRenderer.enabled = false;
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
        if (psm.gravityswitchactiv == false)
        {
            if (psm.rb.velocity.y < -1.5f) psm.ChangeAnimationState(fallstate);
            else if (psm.rb.velocity.y < 1.5f) psm.ChangeAnimationState(jumptofallstate);
        }
        else
        {
            if (psm.rb.velocity.y > 1.5f) psm.ChangeAnimationState(fallstate);
            else if (psm.rb.velocity.y > -1.5f) psm.ChangeAnimationState(jumptofallstate);
        }
    }
    public void displaywhip()
    {
        if (drawwhipwaves == true)
        {
            if(psm.hooktarget != null)
            {
                if (psm.lineRenderer.GetPosition(percision - 1).x == psm.hooktarget.transform.position.x) drawwhipwaves = false;
                else finalwhipdisplay();
            }
        }
        else
        {
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                finalwhipdisplay();
            }
            else
            {
                waveSize = 0;
                if (psm.lineRenderer.positionCount != 2) { psm.lineRenderer.positionCount = 2; }
                finalwhipdisplay();
            }
        }
    }
    private void finalwhipdisplay()
    {
        movetime += Time.deltaTime;
        if (drawwhipwaves == true)
        {
            for (int i = 0; i < percision; i++)
            {
                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(whipdistance).normalized * psm.ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(psm.whipstartpoint.position, psm.hooktarget.transform.position, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(psm.whipstartpoint.position, targetPosition, psm.ropeProgressionCurve.Evaluate(movetime) * whipProgressionSpeed);

                psm.lineRenderer.SetPosition(i, currentPosition);
            }
        }
        else
        {
            psm.lineRenderer.SetPosition(0, psm.whipstartpoint.position);

        }
    }
    public void checkforwhipswitch()
    {
        Vector3 lastlinepoint = psm.lineRenderer.GetPosition(percision - 1);
        if (Vector3.Distance(psm.hooktarget.transform.position, lastlinepoint) < 0.1f)
        {
            psm.playersounds.playwhip();
            psm.state = Playerstatemachine.States.Whip;
        }
    }
}

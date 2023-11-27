using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercollider
{
    public Playerstatemachine psm;

    const string idlestate = "Idle";
    const string runstate = "Run";

    public void playergroundcheck()
    {
        RaycastHit2D downwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 2f, 0, psm.transform.up * -1, 0.2f, psm.groundchecklayer);
        if (downwardhit)
        {
            Debug.DrawRay(downwardhit.point, downwardhit.normal, Color.green);
            psm.groundcheck = true;
            float downwardangle = Vector2.Angle(downwardhit.normal, psm.transform.up);
            RaycastHit2D forwardhit;
            if (psm.faceright == true) forwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 2f, 0, Vector2.left, 0.1f, psm.groundchecklayer);
            else forwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 2f, 0, Vector2.right, 0.1f, psm.groundchecklayer);
            {
                if (forwardhit)
                {
                    float forwardangle = Vector2.Angle(forwardhit.normal, psm.transform.up);
                    //Debug.Log(forwardangle);
                    if (forwardangle > psm.maxslopeangle)
                    {
                        if(downwardangle >= 90)
                        {
                            psm.groundcheck = false;
                            psm.standonslope = false;
                        }
                        else if (downwardangle > psm.maxslopeangle)
                        {
                            psm.switchtoslidwall();
                        }
                        else
                        {
                            psm.state = Playerstatemachine.States.Infrontofwall;
                            psm.rb.velocity = Vector2.zero;
                        }
                    }
                    else standonslope(downwardangle, downwardhit.normal, downwardhit.point);
                }
                else standonslope(downwardangle, downwardhit.normal, downwardhit.point);
            }
        }
        else
        {
            psm.groundcheck = false;
            psm.standonslope = false;
        }
    }
    private void standonslope(float downwardangle, Vector2 downwardnormal, Vector2 hit)
    {
        if (downwardangle == 0)
        {
            psm.standonslope = false;
        }
        else if (downwardangle < psm.maxslopeangle)
        {
            psm.standonslope = true;
            psm.slopemovement = Vector2.Perpendicular(downwardnormal).normalized;
            Debug.DrawRay(hit, psm.slopemovement, Color.red);

        }
        else psm.switchtoslidwall();

        if(psm.standonslope == true && psm.move.x == 0)
        {
            psm.rb.sharedMaterial = psm.friction;
            psm.groundcheckcollider.sharedMaterial = psm.friction;
        }
        else
        {
            psm.rb.sharedMaterial = psm.nofriction;
            psm.groundcheckcollider.sharedMaterial = psm.nofriction;
        }
    }
    public void playergroundcheckair()
    {
        RaycastHit2D downwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 1.9f, 0, psm.transform.up * -1, 0.1f, psm.groundchecklayer);
        if (downwardhit)
        {
            psm.groundcheck = true;
        }
        else psm.groundcheck = false;
    }
    public void playerslidewall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 1.9f, 0, psm.transform.up * -1, 0.2f, psm.groundchecklayer);
        if (hit)
        {
            float angle = Vector2.Angle(psm.transform.up, hit.normal);
            if (angle < psm.maxslopeangle)
            {
                psm.groundcheck = true;
                psm.switchtogroundstate();
            }
        }
        else
        {
            psm.groundcheck = false;
            psm.standonslope = false;
            psm.switchtoairstate();
        }
    }
    public void playerinfrontofwall()
    {
        RaycastHit2D downwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 1.9f, 0, psm.transform.up * -1, 0.2f, psm.groundchecklayer);
        if (downwardhit)
        {
            psm.rb.velocity = Vector2.zero;
            psm.groundcheck = true;
            float downwardangle = Vector2.Angle(psm.transform.up, downwardhit.normal);
            RaycastHit2D forwardhit;
            if (psm.faceright == true) forwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 1.9f, 0, Vector2.left, 0.2f, psm.groundchecklayer);
            else forwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 1.9f, 0, Vector2.right, 0.2f, psm.groundchecklayer);
            {
                if (forwardhit)
                {
                    float forwardangle = Vector2.Angle(psm.transform.up, forwardhit.normal);
                    if (forwardangle > psm.maxslopeangle)
                    {
                        if (downwardangle > psm.maxslopeangle)
                        {
                            psm.switchtoslidwall();
                        }
                    }
                    else psm.switchtogroundstate();
                }
                else psm.switchtogroundstate();
            }
        }
        else
        {
            psm.groundcheck = false;
            psm.standonslope = false;
        }
        if (psm.move == Vector2.zero) psm.ChangeAnimationState(idlestate);
        else psm.ChangeAnimationState(runstate);
    }
}

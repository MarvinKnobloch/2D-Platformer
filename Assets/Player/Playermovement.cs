using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement
{
    public Playerstatemachine psm;

    public void playergroundmovement()
    {
        if (psm.standonslope == true)
        {
            psm.playervelocity.Set(psm.movementspeed * psm.slopemovement.x * -psm.move.x, psm.movementspeed * psm.slopemovement.y * -psm.move.x);
            psm.rb.velocity = psm.playervelocity;
        }
        else
        {
            psm.playervelocity.Set(psm.move.x * psm.movementspeed, -0.5f);
            psm.rb.velocity = psm.rb.velocity = psm.playervelocity;
        }
    }
    public void playerairmovement()
    {
        psm.rb.velocity = new Vector2(psm.move.x * psm.movementspeed, psm.rb.velocity.y);
    }
    public void playercheckforgroundstate()
    {
        if (psm.groundcheck == false) psm.switchtoairstate();
    }

    public void playergroundjump()
    {
        if (psm.controlls.Player.Jump.WasPerformedThisFrame() && psm.canjump == true)
        {
            psm.canjump = false;
            psm.groundintoairswitch();
            playerupwardsmomentum(psm.jumpheight);
        }

    }
    public void playerupwardsmomentum(float upwardsmomentum)
    {
        psm.rb.velocity = new Vector2(psm.rb.velocity.x, 0);
        psm.rb.AddForce(Vector2.up * upwardsmomentum, ForceMode2D.Impulse);
    }
    public void playergroundintoair()
    {
        psm.switchtoairtime += Time.deltaTime;
        if (psm.switchtoairtime > 0.1f)
        {
            psm.switchtoairstate();
        }
    }
    public void playercheckforairstate()
    {
        if (psm.groundcheck == true && psm.rb.velocity.y <= 2f)
        {
            psm.switchtogroundstate();
        }
    }
    public void playerdoublejump()
    {
        if (psm.controlls.Player.Jump.WasPerformedThisFrame() && psm.doublejump == true)
        {
            Globalcalls.jumpcantriggerswitch = true;
            psm.doublejump = false;
            playerupwardsmomentum(psm.jumpheight);
        }
    }
    public void playerdash()
    {
        if (psm.controlls.Player.Dash.WasPerformedThisFrame())
        {
            startdash();
        }
    }
    public void playerairdash()
    {
        if(psm.controlls.Player.Dash.WasPerformedThisFrame() && psm.currentdashcount < psm.maxdashcount)
        {
            psm.currentdashcount++;
            startdash();
        }
    }
    private void startdash()
    {
        psm.rb.velocity = new Vector2(0, 0);
        psm.rb.sharedMaterial = psm.nofriction;
        psm.groundcheckcollider.sharedMaterial = psm.nofriction;
        if (psm.faceright == true) psm.rb.AddForce(Vector2.left * psm.dashlength, ForceMode2D.Impulse);
        else psm.rb.AddForce(Vector2.right * psm.dashlength, ForceMode2D.Impulse);
        psm.dashtimer = 0;
        psm.rb.gravityScale = 0;
        psm.state = Playerstatemachine.States.Dash;
    }
    public void playerdashstate()
    {
        RaycastHit2D downwardhit = Physics2D.BoxCast(psm.groundcheckcollider.bounds.center, psm.groundcheckcollider.bounds.extents * 2f, 0, Vector2.down, 0.2f, psm.groundchecklayer);
        if (downwardhit)
        {
            psm.rb.velocity = new Vector2(psm.rb.velocity.x, psm.rb.velocity.y);
        }
        else
        {
            psm.rb.velocity = new Vector2(psm.rb.velocity.x + psm.rb.velocity.y, 0);
        }
        psm.dashtimer += Time.deltaTime;
        if (psm.dashtimer >= psm.dashtime)
        {
            psm.rb.gravityScale = psm.groundgravityscale;
            psm.rb.velocity = new Vector2(psm.rb.velocity.x, 0);
            psm.switchtoairstate();
        }
    }
    public void playerflip()
    {
        if (psm.move.x > 0 && psm.faceright == true) flip();
        if (psm.move.x < 0 && psm.faceright == false) flip();
    }
    private void flip()
    {
        psm.faceright = !psm.faceright;
        psm.transform.Rotate(0, 180, 0);
    }
}

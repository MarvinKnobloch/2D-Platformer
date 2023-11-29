using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement
{
    public Playerstatemachine psm;

    const string idlestate = "Idle";
    const string runstate = "Run";
    const string jumpstate = "Jump";
    const string dashstate = "Dash";
    const string jumptofallstate = "Jumptofall";
    const string fallstate = "Fall";

    public void playergroundmovement()
    {
        if (psm.standonslope == true)
        {
            psm.playervelocity.Set(psm.movementspeed * psm.slopemovement.x * -psm.move.x, psm.movementspeed * psm.slopemovement.y * -psm.move.x);
            psm.rb.velocity = psm.playervelocity;
        }
        else if(psm.isonplatform == true)
        {
            //if(psm.platformmove != null) psm.playervelocity.Set(psm.platformmove.velocity.x + psm.move.x * psm.movementspeed, psm.platformmove.velocity.y + psm.rb.velocity.y);
            //psm.playervelocity.Set(psm.platformrb.velocity.x + psm.move.x * psm.movementspeed, psm.platformrb.velocity.y);
            psm.playervelocity.Set(psm.movingplatform.velocity.x + psm.move.x * psm.movementspeed, psm.movingplatform.velocity.y);
            psm.rb.velocity = psm.playervelocity;
        }
        else
        {
            if(psm.gravityswitchactiv == false) psm.playervelocity.Set(psm.move.x * psm.movementspeed, -0.5f);
            else psm.playervelocity.Set(psm.move.x * psm.movementspeed, 0.5f);
            //psm.rb.velocity = psm.rb.velocity = psm.playervelocity;
            psm.rb.velocity = psm.playervelocity;
        }
        if(psm.move == Vector2.zero)
        {
            psm.ChangeAnimationState(idlestate);
        }
        else
        {
            psm.ChangeAnimationState(runstate);
        }
    }
    public void playerairmovement()
    {
        if (psm.gravityswitchactiv == false)
        {
            if (psm.rb.velocity.y < -1.5f) psm.ChangeAnimationState(fallstate);
            else if (psm.rb.velocity.y < 1.5f) psm.ChangeAnimationState(jumptofallstate);
            if (psm.rb.velocity.y < -25) psm.rb.velocity = new Vector2(psm.move.x * psm.movementspeed, -25);
            else psm.rb.velocity = new Vector2(psm.move.x * psm.movementspeed, psm.rb.velocity.y);
        }
        else
        {
            if (psm.rb.velocity.y > 1.5f) psm.ChangeAnimationState(fallstate);
            else if (psm.rb.velocity.y > -1.5f) psm.ChangeAnimationState(jumptofallstate);
            if (psm.rb.velocity.y > 25) psm.rb.velocity = new Vector2(psm.move.x * psm.movementspeed, 25);
            else psm.rb.velocity = new Vector2(psm.move.x * psm.movementspeed, psm.rb.velocity.y);
        }
    }
    public void playercheckforgroundstate()
    {
        if (psm.groundcheck == false) psm.switchtoairstate();
    }

    public void playergroundjump()
    {
        if (psm.controlls.Player.Jump.WasPerformedThisFrame() && psm.canjump == true)
        {
            psm.ChangeAnimationState(jumpstate);
            psm.canjump = false;
            psm.isjumping = true;
            psm.jumptime = psm.maxshortjumptime;
            psm.groundintoairswitch();
            psm.playersounds.playjump();
            playerupwardsmomentum(psm.jumpheight);
        }
    }
    public void playerdoublejump()
    {
        if (psm.controlls.Player.Jump.WasPerformedThisFrame() && psm.doublejump == true)
        {
            psm.ChangeAnimationState(jumpstate);
            psm.rb.velocity = new Vector2(psm.rb.velocity.x, 0);
            Globalcalls.jumpcantriggerswitch = true;
            psm.doublejump = false;
            psm.isjumping = true;
            psm.jumptime = psm.maxshortjumptime;
            psm.playersounds.playjump();
            playerupwardsmomentum(psm.jumpheight);
        }
    }
    public void controlljumpheight()
    {
        if(psm.isjumping == true)
        {
            if (psm.controlls.Player.Jump.WasReleasedThisFrame())
            {
                psm.isjumping = false;
                if (psm.gravityswitchactiv == false)
                {
                    if (psm.rb.velocity.y > 0) psm.rb.velocity = new Vector2(psm.rb.velocity.x, psm.rb.velocity.y * 0.5f);
                }
                else
                {
                    if (psm.rb.velocity.y < 0) psm.rb.velocity = new Vector2(psm.rb.velocity.x, psm.rb.velocity.y * 0.5f);
                }
            }
            psm.jumptime -= Time.deltaTime;
            if (psm.jumptime < 0)
            {
                psm.isjumping = false;
            }
        }
    }
    public void playerupwardsmomentum(float upwardsmomentum)
    {
        psm.rb.velocity = new Vector2(psm.rb.velocity.x, 0);
        if(psm.gravityswitchactiv == false) psm.rb.AddForce(Vector2.up * upwardsmomentum, ForceMode2D.Impulse);
        else psm.rb.AddForce(Vector2.up * upwardsmomentum * -1, ForceMode2D.Impulse);
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
        else if (psm.groundcheck == true && psm.isonplatform == true) 
        {
            psm.switchtogroundstate();
        }
    }

    public void playerdash()
    {
        if (psm.controlls.Player.Dash.WasPerformedThisFrame() && Globalcalls.candash == true)
        {
            psm.inair = true;
            startdash();
        }
    }
    public void playerairdash()
    {
        if(psm.controlls.Player.Dash.WasPerformedThisFrame() && psm.currentdashcount < psm.maxdashcount && Globalcalls.candash == true)
        {
            psm.currentdashcount++;
            startdash();
        }
    }
    private void startdash()
    {
        psm.ChangeAnimationState(dashstate);
        psm.isjumping = false;
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
            psm.ChangeAnimationState(fallstate);
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

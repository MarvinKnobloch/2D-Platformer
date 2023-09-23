using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Playerstatemachine : MonoBehaviour
{
    [NonSerialized] public Controlls controlls;
    [NonSerialized] public Rigidbody2D rb;

    [NonSerialized] public Vector2 move;
    [NonSerialized] public Vector2 playervelocity;

    public float movementspeed;
    [NonSerialized] public float switchtoairtime;

    //Jump
    public float jumpheight;
    [NonSerialized] public bool canjump;
    [NonSerialized] public bool doublejump;
    public bool isjumping;
    public float jumptime;
    public float maxshortjumptime;  // 0.13f sollte ganz ok sein
    public float airgravityscale;

    public BoxCollider2D groundcheckcollider;
    public LayerMask groundchecklayer;
    [NonSerialized] public bool groundcheck;
    public float groundgravityscale;
    [NonSerialized] public bool faceright;

    //Dash
    public float dashlength;
    [NonSerialized] public float dashtimer;
    public float dashtime;
    [NonSerialized] public int currentdashcount;
    public int maxdashcount;

    //Slope
    public float maxslopeangle;
    [NonSerialized] public bool standonslope;
    [NonSerialized] public Vector2 slopemovement;
    [NonSerialized] public bool infrontofwall;
    public PhysicsMaterial2D nofriction;
    public PhysicsMaterial2D friction;

    //hook
    public bool inhookstate;
    [NonSerialized] public GameObject hooktarget;
     public Vector3 hookstartposition;
     public Vector3 hookendposition;
    [NonSerialized] public float hookstarttime;
    public float flathookduration;
    public float distancespeedmultiplier;
    public float hookdistancetoobject;
    public float hookradius;

    public bool gravityswitchactiv;


    private Playermovement playermovement = new Playermovement();
    private Playercollider playercollider = new Playercollider();
    private Playergravityswitch playergravityswitch = new Playergravityswitch();
    private Playerabilities playerabilities = new Playerabilities();

    public States state;
    public enum States
    {
        Ground,
        Groundintoair,
        Air,
        Dash,
        Hook,
        Slidewall,
        Infrontofwall,
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controlls = new Controlls();
        switchtogroundstate();

        playermovement.psm = this;
        playercollider.psm = this;
        playergravityswitch.psm = this;
        playerabilities.psm = this;
       
    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case States.Ground:
                playermovement.playergroundmovement();
                break;
            case States.Groundintoair:
                playermovement.playerairmovement();
                break;
            case States.Air:
                playermovement.playerairmovement();
                break;
            case States.Dash:
                break;
            case States.Hook:
                playerabilities.movetohookposition();
                break;
            case States.Slidewall:
                break;
            case States.Infrontofwall:
                break;
        }
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case States.Ground:
                playermovement.playerflip();
                playercollider.playergroundcheck();
                playermovement.playercheckforgroundstate();
                playerabilities.playercheckforhook();
                playergravityswitch.playerswitchgravity();
                playermovement.playerdash();
                playermovement.playergroundjump();
                break;
            case States.Groundintoair:
                playermovement.controlljumpheight();
                playermovement.playergroundintoair();
                playerabilities.playercheckforhook();
                playermovement.playerairdash();
                break;
            case States.Air:
                playermovement.playerflip();
                playermovement.controlljumpheight();
                playercollider.playergroundcheckair();
                playermovement.playercheckforairstate();
                playerabilities.playercheckforhook();
                playergravityswitch.playerswitchgravity();
                playermovement.playerairdash();
                //playermovement.playerdoublejump();
                break;
            case States.Dash:
                playermovement.playerdashstate();
                break;
            case States.Hook:
                break;
            case States.Slidewall:
                playercollider.playerslidewall();
                playermovement.playerdash();
                break;
            case States.Infrontofwall:
                playermovement.playerflip();
                playercollider.playerinfrontofwall();
                playermovement.playerdash();
                playermovement.playergroundjump();
                break;


        }
    }
    public void OnMove(InputValue inputvalue)
    {
        move.x = inputvalue.Get<Vector2>().x;
    }
    public void switchtogroundstate()
    {
        currentdashcount = 0;
        canjump = true;
        doublejump = true;
        isjumping = false;
        state = States.Ground;
        if (gravityswitchactiv == false) rb.gravityScale = groundgravityscale;                      //mit Gravityscale kann ich beeinflussen wie schnell man auf einer slope ist(bei höherer gravity ist man nach oben langsamer aber dafür nach unten schneller)
        else rb.gravityScale = groundgravityscale * -1;

    }
    public void groundintoairswitch()
    {
        Globalcalls.jumpcantriggerswitch = true;
        switchtoairtime = 0;
        rb.sharedMaterial = nofriction;
        groundcheckcollider.sharedMaterial = nofriction;
        if (gravityswitchactiv == false) rb.gravityScale = airgravityscale;
        else rb.gravityScale = airgravityscale * -1;
        state = States.Groundintoair;
    }
    public void switchtoairstate()
    {
        rb.sharedMaterial = nofriction;
        groundcheckcollider.sharedMaterial = nofriction;
        if (gravityswitchactiv == false) rb.gravityScale = airgravityscale;
        else rb.gravityScale = airgravityscale * -1;
        state = States.Air;
    }
    public void switchtoslidwall()
    {
        isjumping = false;
        if (gravityswitchactiv == false) rb.gravityScale = groundgravityscale;                      //mit Gravityscale kann ich beeinflussen wie schnell man auf einer slope ist(bei höherer gravity ist man nach oben langsamer aber dafür nach unten schneller)
        else rb.gravityScale = groundgravityscale * -1;
        state = States.Slidewall;
        rb.velocity = Vector2.zero;
    }
    public void resetplayer()
    {
        Globalcalls.currentgravitystacks = 0;
        Cooldowns.instance.handlegravitystacks();
        canjump = false;
        doublejump = false;
        isjumping = false;
        inhookstate = false;
        currentdashcount = maxdashcount;
        rb.sharedMaterial = nofriction;
        groundcheckcollider.sharedMaterial = nofriction;
        if (gravityswitchactiv == true)
        {
            transform.Rotate(180, 0, 0);
            gravityswitchactiv = false;
            playergravityswitch.triggerplatformrotate();
        }
        rb.gravityScale = airgravityscale;
        rb.velocity = Vector2.zero;
        state = States.Air;
    }
}

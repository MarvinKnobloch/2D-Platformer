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


    private Playermovement playermovement = new Playermovement();
    private Playercollider playercollider = new Playercollider();

    public States state;
    public enum States
    {
        Ground,
        Groundintoair,
        Air,
        Dash,
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
                playermovement.playerdash();
                playermovement.playergroundjump();
                break;
            case States.Groundintoair:
                playermovement.playergroundintoair();
                playermovement.playerairdash();
                break;
            case States.Air:
                playermovement.playerflip();
                playercollider.playergroundcheckair();
                playermovement.playercheckforairstate();
                playermovement.playerairdash();
                //playermovement.playerdoublejump();
                break;
            case States.Dash:
                playermovement.playerdashstate();
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
        state = States.Ground;
        rb.gravityScale = groundgravityscale;                      //mit Gravityscale kann ich beeinflussen wie schnell man auf einer slope ist(bei höherer gravity ist man nach oben langsamer aber dafür nach unten schneller)
    }
    public void groundintoairswitch()
    {
        Globalcalls.jumpcantriggerswitch = true;
        switchtoairtime = 0;
        rb.sharedMaterial = nofriction;
        groundcheckcollider.sharedMaterial = nofriction;
        rb.gravityScale = airgravityscale;
        state = States.Groundintoair;
    }
    public void switchtoairstate()
    {
        rb.sharedMaterial = nofriction;
        groundcheckcollider.sharedMaterial = nofriction;
        rb.gravityScale = airgravityscale;
        state = States.Air;
    }
    public void switchtoslidwall()
    {
        rb.gravityScale = groundgravityscale;
        state = States.Slidewall;
        rb.velocity = Vector2.zero;
    }
}

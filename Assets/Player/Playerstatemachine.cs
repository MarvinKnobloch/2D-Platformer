using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Cinemachine;

public class Playerstatemachine : MonoBehaviour
{
    [NonSerialized] public Controlls controlls;
    public CinemachineConfiner cinemachineConfiner;

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
    [NonSerialized] public float maxshortjumptime = 0.01f;  // 0.13f sollte ganz ok sein       0.01f = deaktiviert
    public float airgravityscale;

    public BoxCollider2D groundcheckcollider;
    public LayerMask groundchecklayer;
    public bool groundcheck;
    public float groundgravityscale;
    [NonSerialized] public bool faceright;

    //platform;
    public bool isonplatform;
    public Movingplatform movingplatform;
    private float platformgravityscale = 30;

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
    [NonSerialized] public Vector3 hookstartposition;
    [NonSerialized] public Vector3 hookendposition;
    [NonSerialized] public float hookstarttime;
    public float hookangle;
    public float flathookduration;
    public float distancespeedmultiplier;
    [NonSerialized] public float hookdistancetoobject;
    public float maxhookdistanceradius;
    public float hookradius;
    public float hookreleaseforce;
    public float xvelocityafterhook;

    //memorie
    public bool memoryisrunning;
    [NonSerialized] public Vector3 memoryposition;
    [NonSerialized] public Vector2 memoryvelocity;
    public float memorymaxusetime;
    public GameObject playermemoryimage;
    public Collider2D memorycamera;
    public Memorytimer memorycdobject;

    public bool gravityswitchactiv;

    private Playermovement playermovement = new Playermovement();
    private Playercollider playercollider = new Playercollider();
    private Playergravityswitch playergravityswitch = new Playergravityswitch();
    private Playerhook playerhook = new Playerhook();
    private Playermemories playermemories = new Playermemories();

    public States state;
    public enum States
    {
        Ground,
        Groundintoair,
        Air,
        Dash,
        Hook,
        Hookrelease,
        Slidewall,
        Infrontofwall,
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controlls = Keybindinputmanager.inputActions;
        switchtogroundstate();

        playermovement.psm = this;
        playercollider.psm = this;
        playergravityswitch.psm = this;
        playerhook.psm = this;
        playermemories.psm = this;

        Globalcalls.playeresetpoint = transform.position;

        //controlls.Player.Move.performed += Context => move = Context.ReadValue<Vector2>();

    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void FixedUpdate()
    {
        if (Globalcalls.gameispaused == false)
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
                    playerhook.movetohookposition();
                    //playerhook.newmovetohookpostion();
                    break;
                case States.Hookrelease:
                    playerhook.hookreleasemovement();
                    break;
                case States.Slidewall:
                    break;
                case States.Infrontofwall:
                    break;
            }
        }
    }
    private void Update()
    {
        if(Globalcalls.gameispaused == false)
        {
            switch (state)
            {
                default:
                case States.Ground:
                    playermovement.playerflip();
                    playercollider.playergroundcheck();
                    playermovement.playercheckforgroundstate();
                    playerhook.playercheckforhook();
                    playergravityswitch.playerswitchgravity();
                    playermemories.playerplacememory();
                    playermovement.playerdash();
                    playermovement.playergroundjump();
                    break;
                case States.Groundintoair:
                    playermovement.controlljumpheight();
                    playermovement.playergroundintoair();
                    playerhook.playercheckforhook();
                    playermemories.playerplacememory();
                    playermovement.playerairdash();
                    break;
                case States.Air:
                    playermovement.playerflip();
                    playermovement.controlljumpheight();
                    playercollider.playergroundcheckair();
                    playermovement.playercheckforairstate();
                    playerhook.playercheckforhook();
                    playergravityswitch.playerswitchgravity();
                    playermemories.playerplacememory();
                    playermovement.playerairdash();
                    //playermovement.playerdoublejump();
                    break;
                case States.Dash:
                    playermovement.playerdashstate();
                    break;
                case States.Hook:
                    break;
                case States.Hookrelease:
                    playermovement.playerflip();
                    playercollider.playergroundcheckair();
                    playermovement.playercheckforairstate();
                    playerhook.playercheckforhook();
                    playergravityswitch.playerswitchgravity();
                    playermemories.playerplacememory();
                    playermovement.playerairdash();
                    break;
                case States.Slidewall:
                    playercollider.playerslidewall();
                    playermovement.playerdash();
                    break;
                case States.Infrontofwall:
                    playermovement.playerflip();
                    playercollider.playerinfrontofwall();
                    playermovement.playercheckforgroundstate();
                    playerhook.playercheckforhook();
                    playergravityswitch.playerswitchgravity();
                    playermemories.playerplacememory();
                    playermovement.playerdash();
                    playermovement.playergroundjump();
                    break;
            }
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
        Globalcalls.jumpcantriggerswitch = true;
        isjumping = false;
        state = States.Ground;
        if (isonplatform == false)
        {
            if (gravityswitchactiv == false) rb.gravityScale = groundgravityscale;                      //mit Gravityscale kann ich beeinflussen wie schnell man auf einer slope ist(bei höherer gravity ist man nach oben langsamer aber dafür nach unten schneller)
            else rb.gravityScale = groundgravityscale * -1;
        }
        else
        {
            if (gravityswitchactiv == false) rb.gravityScale = platformgravityscale;
            else rb.gravityScale = platformgravityscale * -1;
        }

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
        Globalcalls.currentmemorystacks = 0;
        Cooldowns.instance.handlememorystacks();
        if(memorycdobject.transform.parent.gameObject.activeSelf == true) memorycdobject.disablecd();                 //called endmemorietimer

        canjump = false;
        doublejump = false;
        isjumping = false;
        Globalcalls.jumpcantriggerswitch = true;
        inhookstate = false;
        playermemoryimage.SetActive(false);
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
    public void endmemorytimer()
    {
        memoryisrunning = false;
        playermemoryimage.SetActive(false);
    }
    public void hooktargetupdate() => playerhook.checkforclosesthook();
}

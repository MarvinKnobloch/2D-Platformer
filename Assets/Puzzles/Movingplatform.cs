using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movingplatform : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector3 Endposi;
    public Vector3 Startposi;

    private float moveforward = 0;
    public float movesideward;
    public float moveup;

    [SerializeField] private float traveltime;
    private float movetime;

    [NonSerialized] public Vector3 velocity;
    [NonSerialized] public Vector3 oldposi;

    public bool moveonenter;
    [SerializeField] private bool fastreturn;
    [SerializeField] private float fasttraveltime;

    public State state;

    public enum State
    {
        movetoend,
        movetostart,
        dontmove,
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Startposi = transform.position;
        Startposi.z = 0;
        Endposi = transform.position + (transform.right * movesideward) + (transform.forward * moveforward) + (transform.up * moveup);
        Endposi.z = 0;
        
        oldposi = transform.position;

    }
    private void OnEnable()
    {
        if (moveonenter == false) state = State.movetoend;
        else state = State.dontmove;
    }
    private void Update()                        //normals update hat den player nicht mitbewegt
    {
        velocity = (transform.position - oldposi) / Time.deltaTime;
        oldposi = transform.position;
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.movetoend:
                toend();
                break;
            case State.movetostart:
                tostart();
                break;
            case State.dontmove:
                holdposi();
                break;
        }
    }
    private void toend()
    {
        movetime += Time.fixedDeltaTime;
        float precentagecomplete = movetime / traveltime;
        rb.MovePosition(Vector2.Lerp(Startposi, Endposi, precentagecomplete));
        if (movetime >= traveltime)
        {
            state = State.movetostart;
            movetime = 0;
        }
    }
    void tostart()
    {
        if (fastreturn == false)
        {
            moveback(traveltime);
        }
        else moveback(fasttraveltime);

    }
    private void moveback(float time)
    {
        movetime += Time.fixedDeltaTime;
        float precentagecomplete = movetime / time;
        rb.MovePosition(Vector2.Lerp(Endposi, Startposi, precentagecomplete));
        if (movetime >= time)
        {
            if(moveonenter == false)
            {
                state = State.movetoend;
                movetime = 0;
            }
            else
            {
                state = State.dontmove;
                movetime = 0;
            }
        }
    }
    public void startmovement()
    {
        state = State.movetoend;
        movetime = 0;
    }
    public void resetforminstant()
    {
        movetime = 0f;
        state = State.dontmove;
        transform.position = Startposi;
    }
    private void holdposi()
    {
        rb.transform.position = Startposi;
        rb.velocity = Vector2.zero;
    }
}

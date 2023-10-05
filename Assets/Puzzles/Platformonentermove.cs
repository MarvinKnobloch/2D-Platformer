using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformonentermove : MonoBehaviour
{
    public Vector3 Endposi;
    public Vector3 Startposi;
    public Vector3 Resetposi;

    private float moveforward = 0;
    public float movesideward;
    public float moveup;
    public float traveltime;
    public float movetime;

    [SerializeField] private bool fastreturn;
    [SerializeField] private float fasttraveltime;

    public State state;

    public enum State
    {
        dontmove,
        movetoend,
        movetostart,
        reset,
    }
    void Awake()
    {
        Startposi = transform.position;
        Startposi.z = 0;
        Endposi = transform.position + (transform.right * movesideward) + (transform.forward * moveforward) + (transform.up * moveup);
        Endposi.z = 0;
    }
    private void OnEnable()
    {
        movetime = 0;
        state = State.dontmove;
    }

    private void FixedUpdate()                        //normals update hat den player nicht mitbewegt
    {
        switch (state)
        {
            default:
            case State.dontmove:
                break;
            case State.movetoend:
                toend();
                break;
            case State.movetostart:
                tostart();
                break;
            case State.reset:
                resetmovement();
                break;
        }
    }
    public void startmovement()
    {
        state = State.movetoend;
    }
    void toend()
    {
        movetime += Time.deltaTime;
        float precentagecomplete = movetime / traveltime;
        transform.position = Vector2.Lerp(Startposi, Endposi, precentagecomplete);
        if (transform.position == Endposi)
        {
            movetime = 0;
            state = State.movetostart;
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
        movetime += Time.deltaTime;
        float precentagecomplete = movetime / time;
        transform.position = Vector2.Lerp(Endposi, Startposi, precentagecomplete);
        if (transform.position == Startposi)
        {
            movetime = 0f;
            state = State.dontmove;
        }
    }
    public void resetform()
    {
        movetime = 0;
        Resetposi = transform.position;
        state = State.reset;
    }
    private void resetmovement()
    {
        movetime += Time.deltaTime;
        float precentagecomplete = movetime / fasttraveltime;
        transform.position = Vector2.Lerp(Resetposi, Startposi, precentagecomplete);
        if (transform.position == Startposi)
        {
            movetime = 0f;
            state = State.dontmove;
        }
    }
    public void resetforminstant()
    {
        movetime = 0f;
        state = State.dontmove;
        transform.position = Startposi;
    }
}

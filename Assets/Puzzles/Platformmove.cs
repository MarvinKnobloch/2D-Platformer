using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformmove : MonoBehaviour
{
    public Vector3 Endposi;
    public Vector3 Startposi;

    private float moveforward = 0;
    public float movesideward;
    public float moveup;
    public float traveltime;
    public float movetime;


    public State state;
    private Rigidbody2D rb;

    public enum State
    {
        movetoend,
        movetostart,
    }
    void Awake()
    {
        Startposi = transform.position;
        Startposi.z = 0;
        Endposi = transform.position + (transform.right * movesideward) + (transform.forward * moveforward) + (transform.up * moveup);
        Endposi.z = 0;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        movetime = 0;
        state = State.movetoend;
    }

    private void Update()                        //normals update hat den player nicht mitbewegt
    {
        if(rb != null)
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
            }
        }
    }
    void toend()
    {
        movetime += Time.deltaTime;
        float precentagecomplete = movetime / traveltime;
        rb.transform.position = Vector2.Lerp(Startposi, Endposi , precentagecomplete);
        if (transform.position == Endposi)
        {
            movetime = 0;
            state = State.movetostart;
        }
    }
    void tostart()
    {
        movetime += Time.deltaTime;
        float precentagecomplete = movetime / traveltime;
        rb.transform.position = Vector2.Lerp(Endposi, Startposi, precentagecomplete);
        if (transform.position == Startposi)
        {
            movetime = 0f;
            state = State.movetoend;
        }
    }
}

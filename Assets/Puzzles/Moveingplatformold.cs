using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveingplatformold : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector3 Endposi;
    public Vector3 Startposi;
    private Vector3 targetposition;
    [SerializeField] private float traveltime;
    public float movetime;
    [SerializeField] private float speed;

    private float moveforward = 0;
    public float movesideward;
    public float moveup;

    Vector2 playervelocity;

    public State state;


    public enum State
    {
        movetoend,
        movetostart,
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Startposi = transform.position;
        Startposi.z = 0;
        Endposi = transform.position + (transform.right * movesideward) + (transform.forward * moveforward) + (transform.up * moveup);
        Endposi.z = 0;

        float distance = Vector2.Distance(Startposi, Endposi);
        speed = distance * speed;
    }
    private void OnEnable()
    {
        targetposition = Endposi;
        state = State.movetoend;
    }
    private void Update()                        //normals update hat den player nicht mitbewegt
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
    void toend()
    {
        if (Vector2.Distance(transform.position, targetposition) < 0.03f)
        {
            targetposition = Startposi;
            state = State.movetostart;
            movetime = 0;
        }
    }
    void tostart()
    {
        if (Vector2.Distance(transform.position, targetposition) < 0.03f)
        {
            targetposition = Endposi;
            state = State.movetoend;
            movetime = 0;
        }
    }
    private void FixedUpdate()
    {
        var heading = targetposition - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance * speed * Time.fixedDeltaTime;
        playervelocity.Set(direction.x, direction.y);
        rb.velocity = playervelocity;

        //var heading = targetposition - transform.position;
        //var distance = heading.magnitude;
        //var direction = heading / distance;
        //rb.MovePosition(transform.position + direction * 0.03f);
    }
}

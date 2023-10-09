using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformtest : MonoBehaviour
{
    public Vector3 Endposi;
    public Vector3 Startposi;
    private Vector3 targetposition;
    [SerializeField] private float speed;
    private Vector3 movedirection;

    private float moveforward = 0;
    public float movesideward;
    public float moveup;

    Vector2 playervelocity;

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
        targetposition = Endposi;
        movedirection = (targetposition - transform.position).normalized;
    }
    private void Update()
    {
        if(targetposition == Endposi)
        {
            if(Vector2.Distance(transform.position, targetposition) < 0.05f)
            {
                targetposition = Startposi;
                movedirection = (targetposition - transform.position).normalized;
            }
        }
        else if( targetposition == Startposi)
        {
            if (Vector2.Distance(transform.position, targetposition) < 0.05f)
            {
                targetposition = Endposi;
                movedirection = (targetposition - transform.position).normalized;
            }
        }
    }
    private void FixedUpdate()
    {
        //var heading = targetposition - transform.position;
        //var distance = heading.magnitude;
        //var direction = (heading / distance) * speed * Time.deltaTime;
        //playervelocity.Set(direction.x, direction.y);
        //rb.velocity = playervelocity;

        var heading = targetposition - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        rb.MovePosition(transform.position + direction * 0.03f);
    }

}

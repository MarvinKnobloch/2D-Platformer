using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scoutmode : MonoBehaviour
{
    [SerializeField] private Camera maincam;
    private Controlls controls;

    private Rigidbody2D rb;
    private InputAction movehotkey;
    public Vector2 move;
    private Vector2 scoutvelocity;

    [SerializeField] private float scoutspeed;


    private void Awake()
    {
        controls = Keybindinputmanager.inputActions;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        controls.Enable();
        movehotkey = controls.Player.Move;
    }

    void Update()
    {
        if (controls.Player.Move.WasPerformedThisFrame())
        { 
            if(move.x == 0)
            {
                transform.position = new Vector3(maincam.transform.position.x, transform.position.y, 0);
            }
            if(move.y == 0)
            {
                transform.position = new Vector3(transform.position.x, maincam.transform.position.y, 0);
            }
        }

        moveinput();
        scoutvelocity.Set(move.x * scoutspeed, move.y * scoutspeed);
        rb.velocity = scoutvelocity;
    }
    private void moveinput()
    {
        move = movehotkey.ReadValue<Vector2>();
    }
}
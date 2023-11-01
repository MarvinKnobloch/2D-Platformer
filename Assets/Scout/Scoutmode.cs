using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scoutmode : MonoBehaviour
{
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
        moveinput();
        scoutvelocity.Set(move.x * scoutspeed, move.y * scoutspeed);
        rb.velocity = scoutvelocity;
    }
    private void moveinput()
    {
        move = movehotkey.ReadValue<Vector2>();
    }
}
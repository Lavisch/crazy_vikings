using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    int health;
    int anger;
    int cellNumber;
    [SerializeField] float speed;
    private Rigidbody2D rb2d;
    [SerializeField] GameObject frontOfDoor;

    enum State {Idle, BreakDoor};
    private State state;

    void Start()
    {
        state = State.BreakDoor;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.BreakDoor:
                BreakDoor();
                break;
            default:
                goto case State.Idle;
        }
    }

    private void Idle()
    {
        rb2d.velocity = Vector2.zero;
    }

    private void BreakDoor()
    {
        MoveTo(frontOfDoor);
    }

    private void MoveTo(GameObject other)
    {
        rb2d.transform.LookAt(other.transform.position);
        rb2d.velocity = Vector3.forward.normalized * speed;
    }
}

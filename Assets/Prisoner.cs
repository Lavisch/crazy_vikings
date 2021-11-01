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

    enum State {Idle, BreakDoor};
    private State state;

    void Start()
    {
        state = State.Idle;
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
        throw new NotImplementedException();
    }

    private void MoveTo(float x, float y)
    {
        throw new NotImplementedException();
    }
}

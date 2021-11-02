using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    [SerializeField] Transform frontOfDoor, inBed;
    [SerializeField] int prisonerNumber;
    private Rigidbody2D rgbd2D;
    float health = 100f;
    float stress = 0f;
    float speed = 5f;

    bool atDoor = false;

    enum State {Idle, BreakDoor};
    State currentStates;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
        currentStates = State.Idle;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentStates != State.BreakDoor) {
                currentStates = State.BreakDoor;
            } else {
                currentStates = State.Idle;
            }
        } 

        switch (currentStates) {
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

    void Idle() {
        
    }

    void BreakDoor() {
        if(!atDoor) {
            MoveTo(frontOfDoor);
        }
    }

    void MoveTo(Transform directionPoint) {
        var findPos = directionPoint.position - transform.position;
        float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
        rgbd2D.rotation = angle;
        rgbd2D.MovePosition(transform.position + (findPos * speed * Time.deltaTime));
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Door") {
            atDoor = true;
        }
    }
}

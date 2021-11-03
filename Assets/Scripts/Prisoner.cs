using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    [SerializeField] GameObject playerInfrontOfDoor;
    [SerializeField] Transform findDoor, inBed;
    [SerializeField] int prisonerNumber;
    [SerializeField] Door doorScript;
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
        //Temp input
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentStates != State.BreakDoor) {
                currentStates = State.BreakDoor;
            } else {
                currentStates = State.Idle;
            }
        } 

        switch (currentStates) {
            case State.Idle:
                Invoke("Idle01", Random.Range(3, 13));
                break;
            case State.BreakDoor:
                BreakDoor();
                break;
            default:
                goto case State.Idle;
        }
    }

    void Idle01() {
        rgbd2D.rotation = Random.Range(0, 360);
        rgbd2D.velocity = Vector2.zero;
        CancelInvoke();
        if(Random.Range(0, 100) < 50) {
            Invoke("Idle02", 2f);
        }
    }

    void Idle02() {
        Debug.Log("Moving");
        rgbd2D.velocity = rgbd2D.transform.right * speed;
        CancelInvoke();
    }

    void BreakDoor() {
        if(!atDoor) {
            MoveTo(findDoor);
        } else {
            rgbd2D.rotation = 0;
            rgbd2D.transform.position = playerInfrontOfDoor.transform.position;
            //playAnimation
            Invoke("DamageDoor", 2f);
        }
    }

    void MoveTo(Transform directionPoint) {
        var findPos = directionPoint.position - transform.position;
        float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
        rgbd2D.rotation = angle;
        rgbd2D.MovePosition(transform.position + (findPos * speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Door")) {
            atDoor = true;
        }
        if(other.gameObject.CompareTag("idleBorder")) {
            Debug.Log("Hit");
            rgbd2D.velocity = Vector2.zero;
            rgbd2D.rotation = -rgbd2D.rotation;
        }
    }

    void DamageDoor() {
        doorScript.TakeDamage((float)Random.Range(2, 6));
        CancelInvoke();
    }

    public void TakeDamage(float minusHealth) {

    }
}

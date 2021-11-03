using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    [SerializeField] GameObject playerInfrontOfDoor, outsideRoom;
    [SerializeField] Transform findDoor, inBed;
    [SerializeField] int prisonerNumber;
    [SerializeField] Door doorScript;
    private Rigidbody2D rgbd2D;
    float health = 100f;
    float stress = 0f;
    float speed = 5f;

    bool isolationCell = false;
    bool atDoor = false;

    enum State {Idle, BreakDoor, betweenRooms, Isolated};
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
        if(isolationCell) {
            currentStates = State.Isolated;
        }

        switch (currentStates) {
            case State.Idle:
                Invoke("Idle01", Random.Range(3, 6));
                break;
            case State.BreakDoor:
                BreakDoor();
                break;
            case State.betweenRooms:
                rgbd2D.transform.position = new Vector2(-40, -40);
                Invoke("AttackOtherCell", 20f);
                break;
            case State.Isolated:
                rgbd2D.transform.position = new Vector2(-50, -50);
                break; 
            default:
                goto case State.Idle;
        }
        Debug.Log(currentStates);

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
        rgbd2D.velocity = rgbd2D.transform.right * speed;
        CancelInvoke();
    }

    void BreakDoor() {
        if(!atDoor && !doorScript.destroyed) {
            MoveTo(findDoor);
        } else if(doorScript.healthPoints > 0) {
            rgbd2D.transform.position = playerInfrontOfDoor.transform.position;
            rgbd2D.rotation = 0;
            //playAnimation
            Invoke("DamageDoor", 2f);
        } else if(doorScript.healthPoints <= 0) {
            if(rgbd2D.position.x != outsideRoom.transform.position.x) {
                var findPos = outsideRoom.transform.position - transform.position;
                rgbd2D.MovePosition(transform.position + (findPos * speed * Time.deltaTime));
            }
        }
    }

    void AttackOtherCell() {
        
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
            rgbd2D.velocity = Vector2.zero;
        }
        if(other.gameObject.CompareTag("outside")) {
            rgbd2D.velocity = Vector2.zero;
            currentStates = State.betweenRooms;
        }
    }

    void DamageDoor() {
        //doorScript.TakeDamage((float)Random.Range(2, 6));
        doorScript.TakeDamage(50);
        CancelInvoke();
    }

    public void TakeDamage(float minusHealth) {

    }
}

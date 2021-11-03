using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    [SerializeField] GameObject playerInfrontOfDoor, outsideYourRoom;
    [SerializeField] GameObject[] outsideOtherCells;
    [SerializeField] public int prisonerNumber;
    [SerializeField] GameObject[] enemyDoors;
    [SerializeField] Transform findDoor;
    [SerializeField] Door doorScript;
    private Rigidbody2D rgbd2D;
    int randomCellNumberAttack;
    float health = 100f;
    float stress = 0f;
    float speed = 5f;

    bool isolationCell = false;
    public bool dead = false;
    bool atDoor = false;

    public enum State {Idle, breakDoor, betweenRooms, Isolated, attackingOtherPrisoner, electrocuted, gasedToSleep};
    public State currentStates;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
        currentStates = State.Idle;
    }

    void Update() {
        //Temp input
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentStates != State.breakDoor) {
                currentStates = State.breakDoor;
            } else {
                currentStates = State.Idle;
            }
        }
        if(isolationCell) {
            currentStates = State.Isolated;
        }
        if (health <= 0)
            dead = true;

        switch (currentStates) {
            case State.Idle:
                Invoke("Idle01", Random.Range(3, 6));
                break;
            case State.breakDoor:
                BreakDoor();
                break;
            case State.betweenRooms:
                Invoke("AttackOtherCell", 10f);
                break;
            case State.attackingOtherPrisoner:
                //move to other prisoner
                //attack said prisoner
                break;
            case State.electrocuted:
                //take damage
                //play animation
                //invoke
                //change state
                break;
            case State.gasedToSleep:
                //play animation
                //invoke
                //change state
                break;
            case State.Isolated:
                rgbd2D.transform.position = new Vector2(-50, -50);
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
            if(rgbd2D.position.x != outsideYourRoom.transform.position.x) {
                var findPos = outsideYourRoom.transform.position - transform.position;
                rgbd2D.MovePosition(transform.position + (findPos * (speed/2) * Time.deltaTime));
            }
        }
    }

    void AttackOtherCell() {
        rgbd2D.transform.position = outsideOtherCells[randomCellNumberAttack].transform.position;

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
            randomCellNumberAttack = Random.Range(0, outsideOtherCells.Length);
            rgbd2D.transform.position = new Vector2(-40, -40);
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
        health -= minusHealth;
    }
}

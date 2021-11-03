using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    GameObject playerInfrontOfDoor, tempLeftCalc, tempRightCalc, outsideYourRoom, enemyPrisoner;
    [SerializeField] GameObject[] outsideOtherCells;
    [SerializeField] public int prisonerNumber;
    Prisoner enemyPrisonerScript;
    private Rigidbody2D rgbd2D;
    int randomCellNumberAttack;
    public float health = 100f;
    Transform findDoor;
    float stress = 0f;
    float speed = 5f;
    Door doorScript;

    bool isolationCell = false;
    public bool dead = false;
    bool atDoor = false;

    public enum State {Idle, breakDoor, betweenRooms, attackOtherDoor, Isolated, attackingOtherPrisoner, electrocuted, gasedToSleep, dead};
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

        if(isolationCell) currentStates = State.Isolated;

        if (health <= 0) dead = true;

        switch (currentStates) {
            case State.Idle:
                Invoke("Idle01", Random.Range(3, 6));
                break;
            case State.breakDoor:
                BreakDoor();
                break;
            case State.betweenRooms:
                Invoke("AttackOtherCell", 3f);
                break;
            case State.attackOtherDoor:
                EnemyDoor();
                break;
            case State.attackingOtherPrisoner:
                AttackPrisoner();
                
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
                rgbd2D.transform.position = new Vector2(-50*prisonerNumber, -50*prisonerNumber);
                break;
            case State.dead:
                //animation
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
            var findPos = findDoor.position - transform.position;
            float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
            rgbd2D.rotation = angle;
            rgbd2D.MovePosition(rgbd2D.transform.position + (findPos * speed * Time.deltaTime));
            if(Vector3.Distance(rgbd2D.transform.position, playerInfrontOfDoor.transform.position) < 0.1f) {
                atDoor = true;
            }

        } else if(doorScript.healthPoints > 0) {
            rgbd2D.transform.position = playerInfrontOfDoor.transform.position;
            if(playerInfrontOfDoor == tempRightCalc) {
                rgbd2D.rotation = 180;
            } else {
                rgbd2D.rotation = 0;
            }
            //playAnimation
            Invoke("DamageDoor", 2f);

        } else if(doorScript.healthPoints <= 0) {
            if(Vector3.Distance(rgbd2D.transform.position, outsideYourRoom.transform.position) > 0.1f) {
                var findPos = outsideYourRoom.transform.position - transform.position;
                rgbd2D.MovePosition(transform.position + (findPos * (speed / 2) * Time.deltaTime));
            } else {
                randomCellNumberAttack = Random.Range(0, outsideOtherCells.Length);
                rgbd2D.transform.position = new Vector2(-25, -25);
                rgbd2D.velocity = Vector2.zero;
                currentStates = State.betweenRooms;
            }
        }
    }

    void AttackOtherCell() {
        rgbd2D.transform.position = outsideOtherCells[randomCellNumberAttack].transform.position;
        atDoor = false;
        currentStates = State.attackOtherDoor;
        CancelInvoke();
    }

    void EnemyDoor() {
        if(!atDoor && !doorScript.destroyed) {
            var findPos = findDoor.position - transform.position;
            float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
            rgbd2D.rotation = angle;
            rgbd2D.MovePosition(rgbd2D.transform.position + (findPos * speed * Time.deltaTime));
            if(Vector3.Distance(rgbd2D.transform.position, playerInfrontOfDoor.transform.position) < 0.1f) {
                atDoor = true;
            }

        } else if(doorScript.healthPoints > 0) {
            rgbd2D.transform.position = playerInfrontOfDoor.transform.position;
            if(playerInfrontOfDoor == tempRightCalc) {
                rgbd2D.rotation = 180;
            } else {
                rgbd2D.rotation = 0;
            }
            //playAnimation
            Invoke("DamageDoor", 2f);

        } else if(doorScript.healthPoints <= 0 && currentStates == State.attackOtherDoor) {
            currentStates = State.attackingOtherPrisoner;
        }
    }

    void AttackPrisoner() {
        if(!dead && enemyPrisonerScript.health > 0) {
            var findPos = enemyPrisoner.transform.position - transform.position;
            float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
            rgbd2D.rotation = angle;
            rgbd2D.MovePosition(rgbd2D.transform.position + (findPos * speed * Time.deltaTime));
            if(Vector3.Distance(rgbd2D.transform.position, enemyPrisoner.transform.position) > 0.1f) {
                Invoke("DamageGiveToPrisoner", 2f);
            }
        } else if(enemyPrisonerScript.health <= 0){
            enemyPrisonerScript.dead = true;
        }
    }

    void DamageGiveToPrisoner() {
        float damage = Random.Range(10, 15);
        enemyPrisonerScript.health -= damage;
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Door")) {
            findDoor = other.transform.parent.transform;
            tempLeftCalc = findDoor.GetChild(1).gameObject;
            tempRightCalc = findDoor.GetChild(2).gameObject;

            var dist01 = Vector3.Distance(tempLeftCalc.transform.position, rgbd2D.transform.position);
            var dist02 = Vector3.Distance(tempRightCalc.transform.position, rgbd2D.transform.position);

            if(dist01 < dist02) {
                playerInfrontOfDoor = tempLeftCalc;
            } else {
                playerInfrontOfDoor = tempRightCalc;
            }
            
            doorScript = findDoor.GetComponentInParent<Door>();
        }

        if(other.gameObject.CompareTag("outside")) {
            outsideYourRoom = other.gameObject;
        }

        if(other.gameObject.CompareTag("Prisoner")) {
            enemyPrisoner = other.gameObject;
            enemyPrisonerScript = enemyPrisoner.GetComponent<Prisoner>();
        }
    }

    void DamageDoor() {
        //doorScript.TakeDamage((float)Random.Range(2, 6));
        doorScript.TakeDamage(100);
        CancelInvoke();
    }

    public void TakeDamage(float minusHealth) {
        health -= minusHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    GameObject playerInfrontOfDoor, tempLeftCalc, tempRightCalc, outsideYourRoom, enemyPrisoner;
    [SerializeField] GameObject[] outsideOtherCells;
    [SerializeField] public int prisonerNumber;
    outsideRoomVariableHolder roomHolderScript;
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

    public enum State {Idle, breakDoor, betweenRooms, attackOtherDoor, Isolated, attackingOtherPrisoner, backOutside, electrocuted, gasedToSleep, dead};
    public State tempHoldState;
    public State currentStates;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
        currentStates = State.Idle;
        stress = Random.Range(1, 8);
    }

    void Update() {

        if(isolationCell) currentStates = State.Isolated;

        if(health <= 0) {
            dead = true;
            currentStates = State.dead;
        }

        switch (currentStates) {
            case State.Idle:
                Invoke(nameof(Idle01), Random.Range(4, 10));
                break;
            case State.breakDoor:
                BreakDoor();
                break;
            case State.betweenRooms:
                Invoke(nameof(AttackOtherCell), 3f);
                break;
            case State.attackOtherDoor:
                EnemyDoor();
                break;
            case State.attackingOtherPrisoner:
                AttackPrisoner();
                break;
            case State.backOutside:
                newCell();
                break;
            case State.electrocuted:
                if(!dead) {
                    stress += Random.Range(10, 25);
                    rgbd2D.velocity = Vector3.zero;
                }
                Invoke(nameof(electrocutedSleep), 5f);
                break;
            case State.gasedToSleep:
                //play animation
                if(!dead) {
                    stress -= Random.Range(10, 25);
                    rgbd2D.velocity = Vector3.zero;
                }
                Invoke(nameof(sleepingGas), 15f);
                //change state
                break;
            case State.Isolated:
                rgbd2D.transform.position = new Vector2(-50*prisonerNumber, -50*prisonerNumber);
                break;
            case State.dead:
                rgbd2D.velocity = Vector2.zero;
                //animation
                break;
            default:
                goto case State.Idle;
        }
    }

    void electrocutedSleep() {
        currentStates = tempHoldState;
        CancelInvoke();
    }

    void sleepingGas() {
        currentStates = tempHoldState;
        CancelInvoke();
    }

    void Idle01() {
        rgbd2D.rotation = Random.Range(0, 360);
        rgbd2D.velocity = Vector2.zero;
        CancelInvoke();
        if((Random.Range(0, 100) + stress) > 100) {
            currentStates = State.breakDoor;
        } else if(Random.Range(0, 100) > 20 && currentStates == State.Idle) {
            Invoke(nameof(Idle02), 1f);
        } else {
            if(currentStates != State.breakDoor) {
                currentStates = State.Idle;
            }
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
            if(Vector3.Distance(rgbd2D.transform.position, playerInfrontOfDoor.transform.position) < 2.5f) {
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
            Invoke(nameof(DamageDoor), 2f);

        } else if(doorScript.healthPoints <= 0) {
            if(Vector3.Distance(rgbd2D.transform.position, outsideYourRoom.transform.position) > 0.5f) {
                var findPos = outsideYourRoom.transform.position - transform.position;
                rgbd2D.MovePosition(transform.position + (findPos * (speed / 2) * Time.deltaTime));

            } else {
                 rgbd2D.transform.position = new Vector2(-25, -25);
                 rgbd2D.velocity = Vector2.zero;
                 currentStates = State.betweenRooms;
            }
        }
    }

    void AttackOtherCell() {
        randomCellNumberAttack = Random.Range(0, outsideOtherCells.Length);
        rgbd2D.transform.position = outsideOtherCells[randomCellNumberAttack].transform.position;
        atDoor = false;
        roomHolderScript = outsideOtherCells[randomCellNumberAttack].GetComponent<outsideRoomVariableHolder>();
        if(roomHolderScript.peopleInRoom == 1) {
            roomHolderScript.peopleInRoom = 2;
            prisonerNumber = roomHolderScript.roomID;
            currentStates = State.attackOtherDoor;
            CancelInvoke();
        }
    }


    void EnemyDoor() {
        if(!atDoor) {
            var findPos = findDoor.position - transform.position;
            float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
            rgbd2D.rotation = angle;
            rgbd2D.MovePosition(rgbd2D.transform.position + (findPos * speed * Time.deltaTime));
            if(Vector3.Distance(rgbd2D.transform.position, playerInfrontOfDoor.transform.position) < 2f) {
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
            Invoke(nameof(DamageDoor), 2f);

        } else if(doorScript.healthPoints <= 0) {
            atDoor = false;
            currentStates = State.attackingOtherPrisoner;
        }
    }

    void AttackPrisoner() {
        var findPos = enemyPrisoner.transform.position - transform.position;
        float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
        rgbd2D.rotation = angle;
        rgbd2D.MovePosition(rgbd2D.transform.position + (findPos * speed * Time.deltaTime));
        if(Vector3.Distance(rgbd2D.transform.position, enemyPrisoner.transform.position) < 2.5f && enemyPrisonerScript.health > 0) {
            Invoke(nameof(DamageGiveToPrisoner), 1f);
        }
        if(enemyPrisonerScript.health <= 0){
            enemyPrisonerScript.dead = true;
            enemyPrisonerScript.currentStates = State.dead;
            currentStates = State.backOutside;
        }
    }

    void DamageGiveToPrisoner() {
        float damage = Random.Range(10, 15);
        enemyPrisonerScript.health -= damage;
        CancelInvoke();
    }

    void newCell() {
        if(!atDoor) {
            if(rgbd2D.transform.position.y < 0.5f && rgbd2D.transform.position.y > -0.5f) {
                atDoor = true;
            } else {
                if(rgbd2D.transform.position.y > 0.15f) {
                    rgbd2D.velocity = new Vector2(0, -speed);
                    rgbd2D.rotation = -90;
                } else if(rgbd2D.transform.position.y < 0.15f) {
                    rgbd2D.velocity = new Vector2(0, speed);
                    rgbd2D.rotation = 90;
                }
            }
        } else {
            if(Vector3.Distance(rgbd2D.transform.position, outsideYourRoom.transform.position) < 1f) {
                roomHolderScript.peopleInRoom = 0;
                rgbd2D.transform.position = new Vector2(-25, -25);
                rgbd2D.velocity = Vector2.zero;
                currentStates = State.betweenRooms;
            } else {
                var findPos = outsideYourRoom.transform.position - transform.position;
                float angle = Mathf.Atan2(findPos.y, findPos.x) * Mathf.Rad2Deg;
                rgbd2D.rotation = angle;
                rgbd2D.MovePosition(transform.position + (findPos * (speed / 2) * Time.deltaTime));
            }
        }
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
        doorScript.TakeDamage((float)Random.Range(5, 15));
        CancelInvoke();
    }

    public void TakeDamage(float minusHealth) {
        health -= minusHealth;
    }
}

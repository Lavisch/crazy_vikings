using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool destroyed = false;
    public float healthPoints = 100;
    Rigidbody2D rgbd2D;
    float speed = 10;
    
    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(healthPoints <= 0) {
            destroyed = true;
            OpenDoor();
        }
    }

    public void TakeDamage(float amount) {
        healthPoints -= amount;
    }

    void OpenDoor() {
        if(rgbd2D.position.y < 4) {
            rgbd2D.velocity = Vector2.up * speed;
        } else {
            rgbd2D.velocity = Vector2.zero;
            destroyed = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    float healthPoints = 100;
    bool destroyed = false;
    
    void Start() {
        
    }

    void Update() {
        if(healthPoints < 0) {
            destroyed = true;
            OpenDoor();
        }
    }

    public void TakeDamage(float amount) {
        healthPoints -= amount;
    }

    void OpenDoor() {
        if(destroyed) {
            Debug.Log("Sad Life");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] bool isRunning = true;
    [SerializeField] TMP_Text textTimer;
    public int hours;

    int shiftLength = 21600; //Length of a shift in seconds, fictive time
    public float timer = 0;
    float timeMultiplier;
    int gameLength = 180; //Length of a game loop in seconds, real time

    void Start() {
        timeMultiplier = shiftLength / gameLength;
    }

    void Update() {
        if (isRunning) {
            timer += Time.deltaTime * timeMultiplier;
            DisplayTime();
        }
    }

    private void DisplayTime() {
        hours = (int)timer / 3600;
        int minutes = ((int)timer % 3600) / 60;
        //int seconds = ((int)timer % 3600) % 60;
        textTimer.text = string.Format("{0:00}:{1:00}:00", hours, minutes/*, seconds*/);
    }
}

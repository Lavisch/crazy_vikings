using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class introCutsceneFade : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Image fadeBackground;

    bool fadeIn = true, fadeOut = false;
    float fadeRatio = 0.3f, currentFade = 0;


    void FixedUpdate() {
        if(fadeIn && !fadeOut) {
            currentFade += fadeRatio * Time.deltaTime;
            Mathf.Clamp(currentFade, 0, 1);
            text.color = new Color(255, 255, 255, currentFade);
            fadeBackground.color = new Color(0, 0, 0, currentFade);
            if(currentFade >= 1) {
                Invoke(nameof(Wait), 3);
            }
        }

        if(!fadeIn && fadeOut) {
            currentFade -= fadeRatio * Time.deltaTime;
            Mathf.Clamp(currentFade, 0, 1);
            text.color = new Color(255, 255, 255, currentFade);
            if(currentFade <= 0) {
                fadeOut = false;
            }
        }

        if(!fadeOut && !fadeIn) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void Wait() {
        fadeOut = true;
        fadeIn = false;
        CancelInvoke();
    }
}

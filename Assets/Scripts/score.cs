using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class score : MonoBehaviour
{
    [SerializeField] Text winText, loseText, saleryText;
    [SerializeField] GameObject buttons, overlay;
    [SerializeField] Door[] ifDoorsDestroyed;
    [SerializeField] Prisoner[] prisoners;
    [SerializeField] Timer timeScript;

    int numberOfPrisoners;
    int salery = 650; /*Dollar*/

    void Update() {
        for(int i = 0; i < prisoners.Length; i++) {
            if(prisoners[i].dead) {
                numberOfPrisoners++;
            }
        }
    
        //Lose
        if(numberOfPrisoners == 5) {
            saleryText.color = Color.red;
            saleryText.text = "Salery: 0$";
            Time.timeScale = 0;
            LostGame();
        } else {
            numberOfPrisoners = 0;
            salery = 650;
        }
        
        //Win
        if(timeScript.hours >= 6) {
            for(int i = 0; i < prisoners.Length; i++) {
                if(prisoners[i].dead) {
                    salery -= 75;
                }
            }
            for(int x = 0; x < ifDoorsDestroyed.Length; x++) {
                if(ifDoorsDestroyed[x].destroyed) {
                    salery -= 35;
                }
            }
            saleryText.color = Color.green;
            saleryText.text = "Salery: " + salery.ToString() + "$";
            Time.timeScale = 0;
            WinGame();
        }
    }

    public void LostGame() {
        overlay.SetActive(true);
        loseText.gameObject.SetActive(true);
        saleryText.gameObject.SetActive(true);
        buttons.SetActive(true);
    }

    public void WinGame() {
        overlay.SetActive(true);
        winText.gameObject.SetActive(true);
        saleryText.gameObject.SetActive(true);
        buttons.SetActive(true);
    }

    public void BackToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}

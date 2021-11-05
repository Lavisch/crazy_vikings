using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
       Debug.Log("Quit!");
       Application.Quit();
    }

    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image image;

    bool fadeIn = false, fadeOut = false;

    public void PlayGame()
    {
        fadeIn = true;

        //image.color = new Color(0, 0, 0, 0);
        
        //for (float i = 0; i <= 1; i += 0.0001f)
        //{
        // }

        //image.color = new Color(0, 0, 0, 0.5f);

        Invoke(nameof(StartGame), 4);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
       Debug.Log("Quit!");
       Application.Quit();
    }

    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    void Update()
    {
        if (fadeIn && !fadeOut)
        {
            text.color = new Color(0, 0, 0, 0.0001f * Time.deltaTime);
            image.color = new Color(0, 0, 0, 0.0001f * Time.deltaTime);
        }
    }
}

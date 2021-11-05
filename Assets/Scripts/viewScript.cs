using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class viewScript:MonoBehaviour {
    [SerializeField] Image[] buttonColorChange;
    [SerializeField] GameObject[] cameraArray;
    [SerializeField] GameObject staticVideo;
    [SerializeField] AudioSource staticSFX; 
    [SerializeField] Text cellNumberText;
    public int selectedCamera;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            changeCamera(0);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            changeCamera(1);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            changeCamera(2);
        } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            changeCamera(3);
        } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
            changeCamera(4);
        } else if(Input.GetKeyDown(KeyCode.Alpha6)) {
            changeCamera(5);
        }
    }

    public void changeCamera(int pressedCamera) {
        staticVideo.SetActive(true);
        selectedCamera = pressedCamera;
        staticSFX.Play();
        for(int i = 0; i < cameraArray.Length; i++) {
            if(i == pressedCamera) {
                buttonColorChange[i].color = Color.green;
                cellNumberText.text = "Cell " + (pressedCamera + 1).ToString();
                cameraArray[i].SetActive(true);
            } else {
                buttonColorChange[i].color = Color.red;
                cameraArray[i].SetActive(false);
            }
        }
        Invoke("dissableStaticVideo", 0.3f);
    }

    void dissableStaticVideo() {
        staticVideo.SetActive(false);
        staticSFX.Stop();
        CancelInvoke();
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class viewScript:MonoBehaviour {
    [SerializeField] GameObject[] cameraArray;
    [SerializeField] GameObject staticVideo;
    [SerializeField] AudioSource staticSFX; 
    [SerializeField] Text cellNumberText;
    public int selectedCamera;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedCamera = 0;
            changeCamera(selectedCamera);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedCamera = 1;
            changeCamera(selectedCamera);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedCamera = 2;
            changeCamera(selectedCamera);
        } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedCamera = 3;
            changeCamera(selectedCamera);
        } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedCamera = 4;
            changeCamera(selectedCamera);
        } else if(Input.GetKeyDown(KeyCode.Alpha6)) {
            selectedCamera = 5;
            changeCamera(selectedCamera);
        }
    }

    public void changeCamera(int pressedCamera) {
        staticVideo.SetActive(true);
        staticSFX.Play();
        for(int i = 0; i < cameraArray.Length; i++) {
            if(i == pressedCamera) {
                cellNumberText.text = "Cell " + (pressedCamera + 1).ToString();
                cameraArray[i].SetActive(true);
            } else {
                cameraArray[i].SetActive(false);
            }
        }
        Invoke("dissableStaticVideo", 0.3f);
    }

    void dissableStaticVideo() {
        staticVideo.SetActive(false);
        staticSFX.Stop();
    }
}

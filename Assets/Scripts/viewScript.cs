using UnityEngine;
using UnityEngine.UI;

public class viewScript:MonoBehaviour {
    [SerializeField] GameObject[] cameraArray;
    [SerializeField] Text cellNumberText;

    int selectedCamera = 0;

    void Start() {
        changeCamera(0);
    }

    void Update() {
        if(Input.GetKey(KeyCode.Alpha1)) {
            selectedCamera = 0;
            changeCamera(selectedCamera);
        } else if(Input.GetKey(KeyCode.Alpha2)) {
            selectedCamera = 1;
            changeCamera(selectedCamera);
        } else if(Input.GetKey(KeyCode.Alpha3)) {
            selectedCamera = 2;
            changeCamera(selectedCamera);
        }
    }

    public void changeCamera(int pressedCamera) {
        for(int i = 0; i < cameraArray.Length; i++) {
            if(i == pressedCamera) {
                cameraArray[i].SetActive(true);
                cellNumberText.text = "Cell " + (pressedCamera + 1).ToString();
            } else {
                cameraArray[i].SetActive(false);
            }
        }
    }
}

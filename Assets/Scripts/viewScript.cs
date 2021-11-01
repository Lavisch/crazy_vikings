using UnityEngine;

public class viewScript:MonoBehaviour {
    [SerializeField] Camera[] cameraArray;
    int selectedCamera = 0;

    void Start() {
        cameraArray = new Camera[2];
    }

    void Update() {
        if(Input.GetKey(KeyCode.Alpha1)) {
            selectedCamera = 0;
        } else if(Input.GetKey(KeyCode.Alpha2)) {
            selectedCamera = 1;
        }

        switch(selectedCamera) {
            case 0:
            for(int i = 0; i < cameraArray.Length; i++) {
                if(i == selectedCamera) {
                    cameraArray[0].enabled = true;
                } else {
                    cameraArray[i].enabled = false;
                }
            }
            break;
        }
    }
}

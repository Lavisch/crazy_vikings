using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] viewScript getCamViewNumber;
    [SerializeField] Prisoner[] prisonerScript;

    float electrocuteCooldown = 4;
    bool electrocuteReady = true;

    float gasCooldown = 6;
    bool gasReady = true;

    public void Electrocute() {
        if (electrocuteReady) {
            prisonerScript[getCamViewNumber.selectedCamera].TakeDamage(20);
            prisonerScript[getCamViewNumber.selectedCamera].currentStates = Prisoner.State.electrocuted;
            electrocuteReady = false;
            Invoke(nameof(ElectrocuteWait), electrocuteCooldown);
        }
    }

    void ElectrocuteWait() {
        electrocuteReady = true;
    }

    public void Gas() {
        if (gasReady) {
            prisonerScript[getCamViewNumber.selectedCamera].currentStates = Prisoner.State.gasedToSleep;
            gasReady = false;
            Invoke(nameof(GasWait), gasCooldown);
        }
    }

    void GasWait() {
        gasReady = true;
    }
}

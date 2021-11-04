using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField] viewScript getCamViewNumber;
    [SerializeField] Prisoner[] prisonerScript;
    [SerializeField] Slider electrocuteBar, gasBar;

    float electrocuteCooldown = 4;
    bool electrocuteReady = true;
    int electrocuteUses = 10;

    float gasCooldown = 6;
    bool gasReady = true;
    int gasUses = 5;

    void Start()
    {
        electrocuteBar.maxValue = electrocuteUses;
        electrocuteBar.value = electrocuteUses;

        gasBar.maxValue = gasUses;
        gasBar.value = gasUses;
    }

    public void Electrocute() {
        if (electrocuteReady && electrocuteUses > 0) {
            for(int i = 0; i < prisonerScript.Length; i++) {
                if(prisonerScript[i].prisonerNumber == getCamViewNumber.selectedCamera) {
                    prisonerScript[i].TakeDamage(20);
                    prisonerScript[i].tempHoldState = prisonerScript[i].currentStates;
                    prisonerScript[i].currentStates = Prisoner.State.electrocuted;
                }
            }

            Invoke(nameof(ElectrocuteWait), electrocuteCooldown);
            electrocuteUses -= 1;
            electrocuteReady = false;
        }

        electrocuteBar.value = electrocuteUses;
    }

    void ElectrocuteWait() {
        electrocuteReady = true;
    }

    public void Gas() {
        if (gasReady && gasUses > 0) {
            for(int i = 0; i < prisonerScript.Length; i++) {
                if(prisonerScript[i].prisonerNumber == getCamViewNumber.selectedCamera) {
                    prisonerScript[i].tempHoldState = prisonerScript[i].currentStates;
                    prisonerScript[i].currentStates = Prisoner.State.gasedToSleep;
                }
            }
                Invoke(nameof(GasWait), gasCooldown);
            gasUses -= 1;
            gasReady = false;
        }
        gasBar.value = gasUses;
    }

    void GasWait() {
        gasReady = true;
    }
}

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

    public void Electrocute()
    {
        if (electrocuteReady && electrocuteUses > 0)
        {
            prisonerScript[getCamViewNumber.selectedCamera].TakeDamage(20);
            prisonerScript[getCamViewNumber.selectedCamera].currentStates = Prisoner.State.electrocuted;
            Invoke(nameof(ElectrocuteWait), electrocuteCooldown);

            electrocuteUses -= 1;
            electrocuteReady = false;
            Debug.Log("Elec button works");
        }

        electrocuteBar.value = electrocuteUses;
    }

    void ElectrocuteWait() {
        electrocuteReady = true;
    }

    public void Gas()
    {
        if (gasReady && gasUses > 0)
        {
            prisonerScript[getCamViewNumber.selectedCamera].currentStates = Prisoner.State.gasedToSleep;
            Invoke(nameof(GasWait), gasCooldown);

            gasUses -= 1;
            gasReady = false;
            Debug.Log("Gas button works");
        }
        gasBar.value = gasUses;
    }

    void GasWait() {
        gasReady = true;
    }
}

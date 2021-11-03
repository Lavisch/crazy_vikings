using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrocute : MonoBehaviour
{
    [SerializeField] viewScript getCamViewNumber;
    [SerializeField] Prisoner[] prisonerScript;
    float cooldown = 4;
    bool ready = true;


    public void ElectrocutePrisoner()
    {
        if (ready)
        {
            prisonerScript[getCamViewNumber.selectedCamera].TakeDamage(20);

            ready = false;
            Invoke(nameof(Cooldown), cooldown);
        }
    }

    void Cooldown()
    {
        ready = true;
    }
}

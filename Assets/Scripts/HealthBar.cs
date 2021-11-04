using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Prisoner prisoner;

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + new Vector3(0, 1.6f));
        slider.value = prisoner.health;
    }
}

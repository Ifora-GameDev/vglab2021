using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HackUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText = null;
    [SerializeField] private TextMeshProUGUI _droneText = null;

    private void OnEnable()
    {
        HackController.OnRemainingTimeChanged += UpdateTimerText;
        HackController.OnDronesCountChanged += UpdateDroneText;
    }

    private void OnDisable()
    {
        HackController.OnRemainingTimeChanged -= UpdateTimerText;
        HackController.OnDronesCountChanged -= UpdateDroneText;
    }

    private void UpdateTimerText(int value)
    {
        _timerText.text = value.ToString();
    }

    private void UpdateDroneText(int value)
    {
        _droneText.text = value.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShmupUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _rocketText;
    [SerializeField] private TextMeshProUGUI _bombText;

    private void OnEnable()
    {
        PlayerSkillsController.OnRocketCountChanged += UpdateRocketText;
    }

    private void OnDisable()
    {
        PlayerSkillsController.OnRocketCountChanged -= UpdateRocketText;
    }

    private void UpdateScoreText(int value)
    {
        _scoreText.text = value.ToString();
    }

    private void UpdateRocketText(int value)
    {
        _rocketText.text = value.ToString();
    }

    private void UpdateBombText(int value)
    {
        _bombText.text = value.ToString();
    }
}

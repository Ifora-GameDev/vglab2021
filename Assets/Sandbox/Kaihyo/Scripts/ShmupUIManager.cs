using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShmupUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _rocketText;
    [SerializeField] private TextMeshProUGUI _bombText;

    private Canvas _canvas = null;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        PlayerSkillsController.OnRocketCountChanged += UpdateRocketText;
        Teist.GameManager.OnWaveEnd += HandleWaveEnd;
        HackController.OnHackEnd += HandleHackEnd;
    }

    private void OnDisable()
    {
        PlayerSkillsController.OnRocketCountChanged -= UpdateRocketText;
        Teist.GameManager.OnWaveEnd -= HandleWaveEnd;
        HackController.OnHackEnd -= HandleHackEnd;
    }

    private void HandleWaveEnd(int _)
    {
        SetHUDVisible(false);
    }

    private void HandleHackEnd()
    {
        SetHUDVisible(true);
    }

    private void SetHUDVisible(bool isVisible)
    {
        _canvas.enabled = isVisible;
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

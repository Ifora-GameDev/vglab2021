using System;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] private int _level = 1;
    [SerializeField] private int _requiredTurnsAmount = 1;
    [SerializeField] private int _repairingCost = 1000;
    [SerializeField] private HackableModule _requiredSkill = null;

    private bool _isActive = false;
    private bool _hasBeenActivated = false;

    public bool IsActive => _isActive;
    public bool HasBeenActivated => _hasBeenActivated;
    public int Level => _level;
    public int RequiredTurnsAmount => _requiredTurnsAmount;
    public int RepairingCost => _repairingCost;
    public HackableModule RequiredSkill => _requiredSkill;
    public bool IsAvailable => (_requiredSkill == null ||
        (_requiredSkill.Skill.IsActive && _requiredSkill.Skill.IsAvailable));

    public static event Action OnActiveStateChange;

    public void SetActive(bool activeState)
    {
        if(activeState)
        {
            _hasBeenActivated = activeState;
        }

        _isActive = activeState;
        OnActiveStateChange?.Invoke();
    }
}

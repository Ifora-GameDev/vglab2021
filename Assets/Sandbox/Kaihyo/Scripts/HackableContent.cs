using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class HackableContent : MonoBehaviour
{
    [SerializeField] private int _requiredTurnsAmount = 1;
    [SerializeField] private Skill _skill = null;
    [SerializeField] private Color _defaultTint = Color.white;
    [SerializeField] private Color _disableTint = Color.grey;
    [SerializeField] private Color _hackedTint = Color.green;
    [SerializeField] private Color _hoverTint = Color.blue;
    [Header("Repairs")]
    [SerializeField] private Color _brokenTint = Color.red;

    // POWER UP CLASS
    // - ID (pour pouvoir les identifier facilement)
    // - Nombre de tours pour le débloquer
    // - Coût en points pour le réparer
    // - Effet du pouvoir
    // - Toggle IsActive
    // - Niveau du pouvoir (1, 2 ou 3)
    // - Pouvoir précédent (pour la dépendance de débloquage)

    // Components
    private Renderer _renderer = null;
    private Collider _collider = null;
    #region - Private Variables
    private Queue<GameObject> _hackingUnits = new Queue<GameObject>();
    // hacking
    private bool _isHacked = false;
    private bool _isBeingHacked = false;
    private int _remainingTurnsBeforeHack = 0;
    // reparation
    private bool _needsFix = false;
    #endregion

    public static event Action<HackableContent> OnMEnter;
    public static event Action<HackableContent> OnMExit;
    public static event Action<Queue<GameObject>> OnHackComplete;
    public static event Action OnSkillUnlocked;
    public static event Action<Queue<GameObject>> OnBreak;

    public Skill Skill => _skill;
    public bool NeedsFix => _needsFix;

    private void OnEnable()
    {
        // DEBUG
        HackController.OnWaveEnd += ProcessHack;
        Skill.OnActiveStateChange += TryUnlock;
    }

    private void OnDisable()
    {
        // DEBUG
        HackController.OnWaveEnd -= ProcessHack;
        Skill.OnActiveStateChange -= TryUnlock;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        var tint = _skill.IsAvailable ? _defaultTint : _disableTint;
        _renderer.material.SetColor("_Color", tint);
        _collider.enabled = _skill.IsAvailable;
        _skill.SetActive(false);
    }

    private void OnMouseEnter()
    {
        _renderer.material.SetColor("_Color", _hoverTint);
        OnMEnter?.Invoke(this);
    }

    private void OnMouseExit()
    {
        if (_needsFix || !_isHacked)
        {
            var tint = _needsFix ? _brokenTint : _defaultTint;
            _renderer.material.SetColor("_Color", tint);
        }

        OnMExit?.Invoke(this);
    }

    public void FixContent()
    {
        _skill.SetActive(_skill.HasBeenActivated);
        var tint = _isHacked ? _hackedTint : _hoverTint;
        _renderer.material.SetColor("_Color", tint);

        _needsFix = false;
        _collider.enabled = !_isHacked;
    }

    public void BreakContent()
    {
        _skill.SetActive(false);
        _renderer.material.SetColor("_Color", _brokenTint);
        _needsFix = true;
        _collider.enabled = true;

        OnBreak?.Invoke(_hackingUnits);
    }

    public void StartHack(GameObject hackingUnit)
    {
        if (!_isBeingHacked)
        {
            _remainingTurnsBeforeHack = _skill.RequiredTurnsAmount;
            _isBeingHacked = true;
        }

        _hackingUnits.Enqueue(hackingUnit);
        hackingUnit.SetActive(true);
        hackingUnit.transform.position = transform.position + new Vector3(0, 0.75f, 0) * _hackingUnits.Count;
    }

    private void ProcessHack()
    {
        if(!_isBeingHacked)
        {
            return;
        }

        _remainingTurnsBeforeHack -= _hackingUnits.Count;

        if(_remainingTurnsBeforeHack <= 0)
        {
            _renderer.material.SetColor("_Color", _hackedTint);

            _skill.SetActive(true);
            _isHacked = true;
            _isBeingHacked = false;
            _collider.enabled = false;

            OnHackComplete?.Invoke(_hackingUnits);
        }
    }

    private void TryUnlock()
    {
        Color tint;
        if(_needsFix)
        {
            tint = _brokenTint;
        }
        else if(_skill.IsAvailable)
        {
            tint = _skill.IsActive ? _hackedTint : _defaultTint;
        }
        else
        {
            tint = _disableTint;
        }

        _renderer.material.SetColor("_Color", tint);
        _collider.enabled = _skill.IsAvailable && !_skill.IsActive;
    }
}

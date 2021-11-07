using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class HackableModule : MonoBehaviour
{
    [SerializeField] private Skill _skill = null;
    [Header("Placeholder colors")]
    [SerializeField] private Color _defaultTint = Color.white;
    [SerializeField] private Color _disableTint = Color.grey;
    [SerializeField] private Color _hackedTint = Color.green;
    [SerializeField] private Color _hoverTint = Color.blue;
    [SerializeField] private Color _brokenTint = Color.red;

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

    public static event Action<HackableModule> OnMEnter;
    public static event Action<HackableModule> OnMExit;
    public static event Action<Queue<GameObject>> OnHackComplete;
    public static event Action OnSkillUnlocked;
    public static event Action<Queue<GameObject>> OnBreak;

    public UnityEvent<SkillTokenState> OnColorChanged;

    public Skill Skill => _skill;
    public bool NeedsFix => _needsFix;

    private void OnEnable()
    {
        Teist.GameManager.OnWaveEnd += ProcessHack;
        Skill.OnActiveStateChange += TryUnlock;
    }

    private void OnDisable()
    {
        Teist.GameManager.OnWaveEnd += ProcessHack;
        Skill.OnActiveStateChange -= TryUnlock;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _renderer.material.SetColor("_Color", DeduceTint());
        _collider.enabled = _skill.IsAvailable;
        _skill.SetActive(false);
    }

    private void OnMouseEnter()
    {
        OnMEnter?.Invoke(this);
    }

    private void OnMouseOver()
    {
        _renderer.material.SetColor("_Color", _hoverTint);
    }

    private void OnMouseExit()
    {
        _renderer.material.SetColor("_Color", DeduceTint());

        OnMExit?.Invoke(this);
    }

    public void FixContent()
    {
        _skill.SetActive(_skill.HasBeenActivated);
        _needsFix = false;
        _collider.enabled = !_isHacked && _skill.IsAvailable;
        _renderer.material.SetColor("_Color", DeduceTint());
    }

    public void BreakContent()
    {
        _skill.SetActive(false);
        _needsFix = true;
        _collider.enabled = true;
        _renderer.material.SetColor("_Color", DeduceTint());

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

    private void ProcessHack(int _)
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
        Color tint = DeduceTint();

        _renderer.material.SetColor("_Color", tint);
        _collider.enabled = (_skill.IsAvailable && !_skill.IsActive) || (_needsFix);
    }

    private Color DeduceTint()
    {
        Color tint;

        if(_needsFix)
        {
            tint = _brokenTint;
            OnColorChanged?.Invoke(SkillTokenState.Broken);
        }
        else if(_skill.IsAvailable)
        {
            tint = _skill.IsActive ? _hackedTint : _defaultTint;
            var targetTokenState = _skill.IsActive ? SkillTokenState.Hacked : SkillTokenState.Available;
            OnColorChanged?.Invoke(targetTokenState);
        }
        else
        {
            tint = _disableTint;
            OnColorChanged?.Invoke(SkillTokenState.Unavailable);
        }

        return tint;
    }
}

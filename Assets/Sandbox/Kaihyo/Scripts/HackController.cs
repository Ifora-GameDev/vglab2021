using System.Collections.Generic;
using UnityEngine;

public class HackController : MonoBehaviour
{
    [SerializeField] private int _startingDroneAmount = 3;
    [SerializeField] private GameObject _dronePrefab = null;
    [SerializeField] private List<HackableModule> _hackableContents = new List<HackableModule>();

    private HackableModule _lastHoveredContent = null;
    private Queue<GameObject> _availableDrones = new Queue<GameObject>();

    public static event System.Action<int> OnAvailableDronesChanged;

    // DEBUG
    public static event System.Action OnWaveEnd;

    private void Start()
    {
        for(int i = 0; i < _startingDroneAmount; i++)
        {
            var drone = Instantiate(_dronePrefab);
            drone.SetActive(false);
            _availableDrones.Enqueue(drone);
        }
    }

    private void OnEnable()
    {
        HackableModule.OnMEnter += SetHoveredContent;
        HackableModule.OnMExit += ClearHoveredContent;
        HackableModule.OnHackComplete += RecoverDrones;
        HackableModule.OnBreak += RecoverDrones;
    }

    private void OnDisable()
    {
        HackableModule.OnMEnter -= SetHoveredContent;
        HackableModule.OnMExit -= ClearHoveredContent;
        HackableModule.OnHackComplete -= RecoverDrones;
        HackableModule.OnBreak -= RecoverDrones;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _lastHoveredContent != null)
        {
            if (_lastHoveredContent.NeedsFix)
            {
                TryRepair();
            }
            else
            {
                TryHack();
            }
        }

        // DEBUG
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnWaveEnd?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            BreakHackableContent();
        }
    }

    private void SetHoveredContent(HackableModule content)
    {
        if(content != _lastHoveredContent)
        {
            _lastHoveredContent = content;
        }
    }

    private void ClearHoveredContent(HackableModule content)
    {
        if(content == _lastHoveredContent)
        {
            _lastHoveredContent = null;
        }
    }

    private void TryHack()
    {
        if(_availableDrones.Count == 0)
        {
            return;
        }

        var drone = _availableDrones.Dequeue();
        _lastHoveredContent.StartHack(drone);

        OnAvailableDronesChanged?.Invoke(_availableDrones.Count);
    }

    private void TryRepair()
    {
        // Si on n'a pas l'argent, on ne répare pas

        _lastHoveredContent.FixContent();
    }

    private void RecoverDrones(Queue<GameObject> recoveredDrones)
    {
        while(recoveredDrones.Count > 0)
        {
            var drone = recoveredDrones.Dequeue();
            drone.SetActive(false);
            _availableDrones.Enqueue(drone);
        }

        OnAvailableDronesChanged?.Invoke(_availableDrones.Count);
    }

    private void BreakHackableContent()
    {
        if(_hackableContents.Count == 0)
        {
            return;
        }

        List<HackableModule> validModules = new List<HackableModule>();
        int currentWantedLevel = 3; // Max level of a skill
        while(validModules.Count == 0 && currentWantedLevel > 0)
        {
            foreach(var module in _hackableContents)
            {
                if(module.Skill.Level == currentWantedLevel && !module.NeedsFix)
                {
                    validModules.Add(module);
                }
            }

            currentWantedLevel--;
        }

        HackableModule selectedModule = null;
        if (validModules.Count > 0)
        {
            int randomID = Random.Range(0, validModules.Count);
            selectedModule = validModules[randomID];
        }

        if (selectedModule != null)
        {
            selectedModule.BreakContent();
        }
    }
}

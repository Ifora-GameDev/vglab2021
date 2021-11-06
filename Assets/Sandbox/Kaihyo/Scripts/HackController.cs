using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackController : MonoBehaviour
{
    [SerializeField] private int _startingDroneAmount = 3;
    [SerializeField] private GameObject _dronePrefab = null;
    [SerializeField] private List<HackableContent> _hackableContents = new List<HackableContent>();

    private HackableContent _lastHoveredContent = null;
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
        HackableContent.OnMEnter += SetHoveredContent;
        HackableContent.OnMExit += ClearHoveredContent;
        HackableContent.OnHackComplete += RecoverDrones;
        HackableContent.OnBreak += RecoverDrones;
    }

    private void OnDisable()
    {
        HackableContent.OnMEnter -= SetHoveredContent;
        HackableContent.OnMExit -= ClearHoveredContent;
        HackableContent.OnHackComplete -= RecoverDrones;
        HackableContent.OnBreak -= RecoverDrones;
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

    private void SetHoveredContent(HackableContent content)
    {
        if(content != _lastHoveredContent)
        {
            _lastHoveredContent = content;
        }
    }

    private void ClearHoveredContent(HackableContent content)
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

        int randomID = Random.Range(0, _hackableContents.Count);
        _hackableContents[randomID].BreakContent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackController : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Texture2D _repairCursorSprite = null;
    [SerializeField] private float _hackDuration = 15f;
    [SerializeField] private int _startingDroneAmount = 3;
    [SerializeField] private GameObject _dronePrefab = null;
    [SerializeField] private List<HackableModule> _hackableContents = new List<HackableModule>();

    private HackableModule _lastHoveredContent = null;
    private Queue<GameObject> _availableDrones = new Queue<GameObject>();
    private Coroutine _timerCoroutine = null;
    private float _elapsedTime = 0f;

    public static event System.Action OnHackEnd;
    public static event System.Action<int> OnDronesCountChanged;
    public static event System.Action<int> OnRemainingTimeChanged;
    public static event System.Action<bool> OnShipInDanger;

    private void Start()
    {
        for(int i = 0; i < _startingDroneAmount; i++)
        {
            var drone = Instantiate(_dronePrefab);
            drone.SetActive(false);
            _availableDrones.Enqueue(drone);
        }

        OnDronesCountChanged?.Invoke(_availableDrones.Count);
    }

    private void OnEnable()
    {
        Teist.GameManager.OnWaveEnd += HandleWaveEnd;

        HackableModule.OnMEnter += SetHoveredContent;
        HackableModule.OnMExit += ClearHoveredContent;
        // Cursor management
        HackableModule.OnMEnter += HandleCursorChange;
        HackableModule.OnMExit += HandleCursorChange;

        HackableModule.OnHackComplete += RecoverDrones;
        HackableModule.OnBreak += RecoverDrones;

        PlayerManager.OnPlayerHit += BreakHackableContent;
    }

    private void OnDisable()
    {
        Teist.GameManager.OnWaveEnd -= HandleWaveEnd;

        HackableModule.OnMEnter -= SetHoveredContent;
        HackableModule.OnMExit -= ClearHoveredContent;
        HackableModule.OnHackComplete -= RecoverDrones;
        HackableModule.OnBreak -= RecoverDrones;

        // Cursor management
        HackableModule.OnMEnter -= HandleCursorChange;
        HackableModule.OnMExit -= HandleCursorChange;

        PlayerManager.OnPlayerHit -= BreakHackableContent;
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

        if(Input.GetKeyDown(KeyCode.R))
        {
            BreakHackableContent();
        }
    }

    private void HandleCursorChange(HackableModule _)
    {
        if(_lastHoveredContent != null && _lastHoveredContent.NeedsFix)
        {
            Cursor.SetCursor(_repairCursorSprite, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void HandleWaveEnd(int _)
    {
        if (_timerCoroutine == null)
        {
            _timerCoroutine = StartCoroutine(HackTimer());
        }

        SetHackViewVisible(true);
    }

    private void SetHackViewVisible(bool isVisible)
    {
        _camera.gameObject.SetActive(isVisible);
    }

    private void BreakHackableContent()
    {
        if (_hackableContents.Count == 0)
        {
            return;
        }

        List<HackableModule> validModules = new List<HackableModule>();
        int currentWantedLevel = 3; // Max level of a skill
        while (validModules.Count == 0 && currentWantedLevel > 0)
        {
            foreach (var module in _hackableContents)
            {
                if (module.Skill.Level == currentWantedLevel && !module.NeedsFix)
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

        CheckShipEmergency();
    }

    // the ship is in emergency (last life) if all modules are broken
    private bool CheckShipEmergency()
    {
        foreach (var module in _hackableContents)
        {
            if (!module.NeedsFix)
            {
                OnShipInDanger?.Invoke(false);
                return false;
            }
        }

        OnShipInDanger?.Invoke(true);
        return true;
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

        OnDronesCountChanged?.Invoke(_availableDrones.Count);
    }

    private void TryRepair()
    {
        if(Teist.GameManager.Money < _lastHoveredContent.Skill.RepairingCost)
        {
            // Jouer son "reparation impossible"
            Debug.Log("<color=red>Not enough money to repair !</color>");
            return;
        }

        Teist.GameManager.Money -= _lastHoveredContent.Skill.RepairingCost;
        _lastHoveredContent.FixContent();
        CheckShipEmergency();
    }

    private void RecoverDrones(Queue<GameObject> recoveredDrones)
    {
        while(recoveredDrones.Count > 0)
        {
            var drone = recoveredDrones.Dequeue();
            drone.SetActive(false);
            _availableDrones.Enqueue(drone);
        }

        OnDronesCountChanged?.Invoke(_availableDrones.Count);
    }

    private IEnumerator HackTimer()
    {
        _elapsedTime = 0f;
        
        while(_elapsedTime < _hackDuration)
        {
            _elapsedTime += Time.deltaTime;

            int remainingTime = (int)_hackDuration - (int)_elapsedTime;
            OnRemainingTimeChanged?.Invoke(remainingTime);
            
            yield return null;
        }

        OnHackEnd?.Invoke();
        SetHackViewVisible(false);

        _timerCoroutine = null;
    }
}

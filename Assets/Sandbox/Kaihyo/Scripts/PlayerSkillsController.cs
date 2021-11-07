using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
    [Header("Rocket Skill")]
    [SerializeField] private HackableModule _rocketLevel1 = null;
    [SerializeField] private HackableModule _rocketLevel2 = null;
    [SerializeField] private HackableModule _rocketLevel3 = null;
    [Space(2.0f)]
    [SerializeField] private GameObject _rocketPrefab = null;
    [SerializeField] private Transform _rocketOrigin = null;
    [SerializeField] private float _rocketFireRate = 0.5f;
    [SerializeField] private int _baseMaxRocketAmount = 5;
    [SerializeField] private int _upgradedMaxRocketAmount = 10;
    [Header("Bomb Skills")]
    [SerializeField] private HackableModule _bombLevel1 = null;
    [SerializeField] private HackableModule _bombLevel2 = null;
    [SerializeField] private HackableModule _bombLevel3 = null;
    [Space(2.0f)]
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private int _baseRequiredEnergy = 30;
    [SerializeField] private int _upgradedRequiredEnergy = 15;
    [Header("Shoot Skills")]
    [SerializeField] private HackableModule _shootLevel1 = null;
    [SerializeField] private HackableModule _shootLevel2 = null;
    [SerializeField] private HackableModule _shootLevel3 = null;

    private bool _inputsToggle = true;
    // ROCKETS
    private float _nextRocketFireTime = 0f;
    private int _availableRocketsCount = 0;
    private Queue<GameObject> _rocketsPool = new Queue<GameObject>();
    // BOMB
    private bool _bombReady = false;
    private int _currentBombEnergy = 0;
    private Coroutine _bombCooldownCoroutine = null;

    public static event Action<int> OnRocketCountChanged;
    public static event Action<int> OnBombEnergyAmountChanged;

    private int MaxRocketAmount
    {
        get
        {
            int amount = 0;

            if(_rocketLevel2.Skill.IsAvailable && _rocketLevel2.Skill.IsActive)
            {
                amount = _upgradedMaxRocketAmount;
            }
            else if(_rocketLevel1.Skill.IsAvailable && _rocketLevel1.Skill.IsActive)
            {
                amount = _baseMaxRocketAmount;
            }
            else
            {
                amount = 0; // DEBUG, REMETTRE A 0
            }

            return amount;
        }
    }

    private int RequiredBombEnergy
    {
        get
        {
            int amount = 0;

            if (_bombLevel2.Skill.IsAvailable && _bombLevel2.Skill.IsActive)
            {
                amount = _upgradedRequiredEnergy;
            }
            else if (_bombLevel1.Skill.IsAvailable && _bombLevel1.Skill.IsActive)
            {
                amount = _baseRequiredEnergy;
            }
            else
            {
                amount = 0; // DEBUG, REMETTRE A 0
            }

            return amount;
        }
    }

    private void OnEnable()
    {
        HackController.OnHackEnd += RefillRockets;
        HackController.OnHackEnd += RefillBomb;
        HackController.OnHackEnd += ToggleInputs;
        Teist.GameManager.OnWaveEnd += ToggleInputsWrapper;
        Teist.GameManager.OnGameWin += ToggleInputs;
    }

    private void OnDisable()
    {
        HackController.OnHackEnd -= RefillRockets;
        HackController.OnHackEnd -= RefillBomb;
        HackController.OnHackEnd -= ToggleInputs;
        Teist.GameManager.OnWaveEnd -= ToggleInputsWrapper;
        Teist.GameManager.OnGameWin -= ToggleInputs;
    }

    private void Start()
    {
        CreateRocketPool();
        SetRocketCount(MaxRocketAmount);
        SetBombEnergy(RequiredBombEnergy);
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            FireRocket();
        }

        if(Input.GetKey(KeyCode.C))
        {
            FireBomb();
        }
    }

    private void ToggleInputs()
    {
        _inputsToggle = !_inputsToggle;
    }

    private void ToggleInputsWrapper(int _)
    {
        ToggleInputs();
    }

    #region - ROCKETS LOGIC
    private void CreateRocketPool()
    {
        GameObject rocketPoolHolder = new GameObject("RocketPool");
        rocketPoolHolder.transform.parent = transform;

        for (int i = 0; i < _upgradedMaxRocketAmount; i++)
        {
            GameObject rocket = Instantiate(_rocketPrefab, rocketPoolHolder.transform);
            rocket.SetActive(false);
            _rocketsPool.Enqueue(rocket);
        }
    }

    private void RefillRockets()
    {
        SetRocketCount(MaxRocketAmount);
    }

    private void SetRocketCount(int amount)
    {
        _availableRocketsCount = amount;
        OnRocketCountChanged?.Invoke(_availableRocketsCount);
    }

    private void FireRocket()
    {
        if (!_inputsToggle)
        {
            return;
        }


        if (_availableRocketsCount <= 0)
        {
            return;
        }

        if(Time.time >= _nextRocketFireTime)
        {
            var rocket = _rocketsPool.Dequeue();
            _rocketsPool.Enqueue(rocket);
            rocket.transform.position = _rocketOrigin.position;
            rocket.transform.up = _rocketOrigin.up;
            rocket.SetActive(true);

            SetRocketCount(_availableRocketsCount - 1);
            _nextRocketFireTime = Time.time + 1 / _rocketFireRate;
        }
    }
    #endregion - ROCKET LOGIC

    #region - BOMB LOGIC
    private void RefillBomb()
    {
        if(RequiredBombEnergy <= 0)
        {
            Debug.Log("<color=red>Bomb not yet unlocked</color>");
            return;
        }

        SetBombEnergy(RequiredBombEnergy);
        _bombReady = true;
    }

    private void SetBombEnergy(int value)
    {
        if(RequiredBombEnergy <= 0)
        {
            OnBombEnergyAmountChanged?.Invoke(0);
            return;
        }

        _currentBombEnergy = value;
        float bombLoadingPercentage = (float)_currentBombEnergy / (float)RequiredBombEnergy;
        bombLoadingPercentage *= 100;
        OnBombEnergyAmountChanged?.Invoke((int)bombLoadingPercentage);
    }

    private void FireBomb()
    {
        if(RequiredBombEnergy <= 0 || !_bombReady)
        {
            return;
        }

        if (_bombLevel3.Skill.IsAvailable && _bombLevel3.Skill.IsActive)
        {
            // Evenement pour tuer tous les ennemis sur la map
            /*
            SetBombEnergy(0);
            _bombReady = false;

             if (_bombCooldownCoroutine == null)
             {
                StartCoroutine(BombCooldown());
             }
             */
            return;
        }

        Collider2D[] allColider = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (Collider2D collider in allColider)
        {
            if ((collider.gameObject.tag == "Enemy") ||
                collider.gameObject.tag == "Bullet")
            {
                Destroy(collider.gameObject);
            }
        }

        SetBombEnergy(0);
        _bombReady = false;

        if (_bombCooldownCoroutine == null)
        {
            StartCoroutine(BombCooldown());
        }
    }

    private IEnumerator BombCooldown()
    {
        while(_currentBombEnergy < RequiredBombEnergy)
        {
            SetBombEnergy(_currentBombEnergy + 1);
            yield return new WaitForSeconds(1.0f);
        }

        _bombReady = true;
        _bombCooldownCoroutine = null;
    }
    #endregion - BOMB LOGIC
}

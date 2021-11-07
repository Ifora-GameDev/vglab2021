using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
    [Header("Rocket Skill")]
    [SerializeField] private HackableModule _rocketLevel1 = null;
    [SerializeField] private int _baseMaxRocketAmount = 5;
    [SerializeField] private HackableModule _rocketLevel2 = null;
    [SerializeField] private int _upgradedMaxRocketAmount = 10;
    [SerializeField] private HackableModule _rocketLevel3 = null;
    [Space(2.0f)]
    [SerializeField] private GameObject _rocketPrefab = null;
    [SerializeField] private Transform _rocketOrigin = null;
    [SerializeField] private float _rocketFireRate = 0.5f;

    // ROCKETS
    private float _nextRocketFireTime = 0f;
    private int _availableRocketsCount = 0;
    private Queue<GameObject> _rocketsPool = new Queue<GameObject>();

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
                amount = 0; // DEBUG, REMETTE A 0
            }

            return amount;
        }
    }

    private void OnEnable()
    {
        Rocket.OnRocketDestroyed += RecoverRocket;
    }

    private void OnDisable()
    {
        Rocket.OnRocketDestroyed -= RecoverRocket;
    }

    private void Start()
    {
        CreateRocketPool();
        _availableRocketsCount = MaxRocketAmount;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            FireRocket();
        }
    }

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

    private void FireRocket()
    {
        if(_availableRocketsCount <= 0)
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

            _availableRocketsCount--;
            _nextRocketFireTime = Time.time + 1 / _rocketFireRate;
        }
    }

    private void RecoverRocket(GameObject rocket)
    {
        _rocketsPool.Enqueue(rocket);
    }
}

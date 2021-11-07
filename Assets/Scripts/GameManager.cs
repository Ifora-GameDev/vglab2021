using System;
using System.Collections;
using UnityEngine;


namespace Teist
{
    public class GameManager : MonoBehaviour
    {
        private static int money;

        [SerializeField] private int enemiesAlive = 0;

        public Wave[] waves;


        private int waveIndex = 0;
        [SerializeField] private Transform spawnPoint;

        //[SerializeField] private PlayerManager player;
        
        private float waveCooldown;

        private float camHalfHeight;
        private float camHalfWidth;

        private bool isSpawnActive = true;
        private bool isWaveSpawnFinished = false;
        private bool isGameOver = false;
        
        public static event Action<int> OnWaveEnd;
        public static event Action OnGameWin;
        public static event Action<int> OnMoneyValueChanged;

        [SerializeField] private GameObject vfxWarning;

        public static int Money
        {
            get { return money; }
            set { SetMoney(value); }
        }

        private void OnEnable()
        {
            Enemy.OnEnDie += Enemy_OnEnDie;
            HackController.OnHackEnd += StartNextWave;
        }

        private void OnDisable()
        {
            HackController.OnHackEnd -= StartNextWave;
        }

        private void Start()
        {
            SetMoney(0);

            waveCooldown = 0f;
            spawnPoint.position = new Vector3(0, 0, 0);

            camHalfHeight = Camera.main.orthographicSize;
            camHalfWidth = Camera.main.aspect * camHalfHeight;
            StartCoroutine(SpawnWave());
        }


        // Update is called once per frame
        void Update()
        {
            if (isGameOver) return;

            //Managing enemy's spawner

            //Si la vague en cours est timée
            if (isSpawnActive)
            {
                if (waves[waveIndex].isTimed)
                {
                    if (waveCooldown >= waves[waveIndex].timeNextWave)
                    {
                        waveCooldown = 0f;

                        if (waveIndex < waves.Length - 1)
                        {
                            OnWaveEnd?.Invoke(waveIndex);
                            isSpawnActive = false;
                        }
                        else
                        {
                            Debug.Log("<color=green>YOU WIN !</color>");
                            OnGameWin?.Invoke();
                        }
                    }

                    waveCooldown += Time.deltaTime;
                }
                else if (isWaveSpawnFinished)
                { 
                    if (enemiesAlive <= 0)
                    {
                        if (waveIndex < waves.Length - 1)
                        {
                            OnWaveEnd?.Invoke(waveIndex);
                            isSpawnActive = false;
                        }
                        else
                        {
                            Debug.Log("<color=green>YOU WIN !</color>");
                            OnGameWin?.Invoke();
                        }
                    }
                }
            }

        }

        private void StartNextWave()
        {
            waveIndex++;
            isWaveSpawnFinished = false;
            isSpawnActive = true;
            StartCoroutine(SpawnWave());
        }

        IEnumerator SpawnWave()
        {
            isWaveSpawnFinished = false;
            Debug.Log("spawning wave " + waveIndex);
            Wave wave = waves[waveIndex];

            //Check to place alert signal
            if (wave.path.points[0].position.x < -camHalfWidth)
            {
                spawnPoint.position = new Vector2(-camHalfWidth * .9f, 0);

            }
            if (wave.path.points[0].position.x > camHalfWidth)
            {
                spawnPoint.position = new Vector2(camHalfWidth*.9f, 0);
            }

            if (wave.path.points[0].position.y < -camHalfHeight)
            {
                spawnPoint.position = new Vector2(0, -camHalfHeight * .9f);
            }
            if (wave.path.points[0].position.y > camHalfHeight)
            {
                 spawnPoint.position = new Vector2(0, camHalfHeight * .9f);
            }
            foreach (GameObject enemy in wave.enemies)
            {

                //yield return new WaitForSeconds(2f);
                yield return StartCoroutine(SpawnEnemy(enemy,wave.path,wave.isLerp));
                yield return new WaitForSeconds(1f / wave.rate);
            }
            isWaveSpawnFinished = true;
        }


        IEnumerator SpawnEnemy(GameObject enemy,Waypoints path, bool isLerp)
        {
            Instantiate(vfxWarning, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            enemiesAlive++;
            GameObject e = Instantiate(enemy, path.points[0].transform.position, Quaternion.identity);
            e.GetComponent<Enemy>().Init(path,isLerp);
        }


        private static void SetMoney(int value)
        {
            money = value;
            OnMoneyValueChanged?.Invoke(money);
        }

        //Event send when an enemy die, containing money reward information
        private void Enemy_OnEnDie(int reward)
        {
            SetMoney(money + reward);
            enemiesAlive--;
            if (enemiesAlive < 0)
            {
                Debug.LogWarning("enemies alive shouldn't be negative");
                enemiesAlive = 0;
            }
        }
    }
}
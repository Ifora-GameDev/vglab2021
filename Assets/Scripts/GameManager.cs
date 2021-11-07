using System;
using System.Collections;
using UnityEngine;


namespace Teist
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public static int money;
        [SerializeField] private int enemiesAlive = 0;

        public Wave[] waves;


        private int waveIndex = 0;
        [SerializeField] private Transform spawnPoint;

        //[SerializeField] private PlayerManager player;
        
        private float waveCooldown;

        private float camHalfHeight;
        private float camHalfWidth;

        private bool isWaveSpawnFinished = false;
        private bool isGameOver = false;
        
        public static event Action<int> OnWaveEnd;

        [SerializeField] private GameObject vfxWarning;

        // Start is called before the first frame update
        void Awake()
        {
            Enemy.OnEnDie += Enemy_OnEnDie;
        }


        private void Start()
        {
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
            if (waves[waveIndex].isTimed)
            {
                if (waveCooldown >= waves[waveIndex].timeNextWave)
                {
                    waveCooldown = 0f;

                    if (waveIndex < waves.Length - 1)
                    {
                        OnWaveEnd?.Invoke(waveIndex);
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                }
            }
            else if (isWaveSpawnFinished)
            {
                if (enemiesAlive <= 0)
                {
                    if (waveIndex < waves.Length - 1)
                    {
                        OnWaveEnd?.Invoke(waveIndex);
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                }
            }
            /*
            if (waves[waveIndex].isTimed == false)
            {
                if (enemiesAlive <= 0)
                {
                    if (waveIndex < waves.Length-1)
                    {
                        OnWaveEnd?.Invoke(waveIndex);
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                }
            }

            else
            {
                if (waveCooldown >= waves[waveIndex].timeNextWave)
                {
                    waveCooldown = 0f;
                
                    if (waveIndex < waves.Length-1)
                    {
                        OnWaveEnd?.Invoke(waveIndex);
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                }
            
            }
            */

            /*
            if (isWaveFinished && waveIndex >= waves.Length && enemiesAlive<=0)
            {
                isGameOver = true;
                Debug.Log("game over, you win");
            }*/
            waveCooldown += Time.deltaTime;
        }

        IEnumerator SpawnWave()
        {
            isWaveSpawnFinished = false;
            Debug.Log("spawning wave " + waveIndex);
            Wave wave = waves[waveIndex];


            //A MODIFIER SI ON VEUT POUVOIR CHANGER LE PATTERN EN FONCTION DE LA DIRECTION
            //VOIR AVEC WAVE
            //Check to stay in screen
            if (wave.path.points[0].position.x < -camHalfWidth)
            {
                spawnPoint.position = new Vector2(-camHalfWidth, 0);

            }
            if (wave.path.points[0].position.x > camHalfWidth)
            {
                spawnPoint.position = new Vector2(camHalfWidth, 0);
            }

            if (wave.path.points[0].position.y < -camHalfHeight)
            {
                spawnPoint.position = new Vector2(0, -camHalfHeight);
            }
            if (wave.path.points[0].position.y > camHalfHeight)
            {
                 spawnPoint.position = new Vector2(0, camHalfHeight);
            }
            foreach (GameObject enemy in wave.enemies)
            {

                yield return new WaitForSeconds(2f);
                StartCoroutine(SpawnEnemy(enemy,wave.path,wave.isLerp));
                yield return new WaitForSeconds(1f / wave.rate);
            }
            isWaveSpawnFinished = true;
            //Debug.Log(waveIndex);
        }


        IEnumerator SpawnEnemy(GameObject enemy,Waypoints path, bool isLerp)
        {
            Instantiate(vfxWarning, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            enemiesAlive++;
            GameObject e = Instantiate(enemy, path.points[0].transform.position, Quaternion.identity);
            e.GetComponent<Enemy>().Init(path,isLerp);
        }


        //Event send when an enemy die, containing money reward information
        private void Enemy_OnEnDie(int reward)
        {
            money += reward;
            enemiesAlive--;
            if (enemiesAlive < 0)
            {
                Debug.LogWarning("enemies alive shouldn't be negative");
                enemiesAlive = 0;
            }
        }
    }
}
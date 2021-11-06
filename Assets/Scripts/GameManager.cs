using System.Collections;
using UnityEngine;


namespace Teist
{
    public class GameManager : MonoBehaviour
    {
        public int money;
        [SerializeField] private int enemiesAlive = 0;

        public Wave[] waves;


        private int waveIndex = 0;
        [SerializeField] private Transform spawnPoint;

        [SerializeField] private Player player;

        private float waveCooldown;

        private float camHalfHeight;
        private float camHalfWidth;


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
            Debug.Log(waveIndex);
            StartCoroutine(SpawnWave());
            Debug.Log("waves length"+waves.Length);
        }


        // Update is called once per frame
        void Update()
        {
            //Checking player's life


            //Managing enemy's spawner
            if (waves[waveIndex].isTimed == false)
            {
                if (enemiesAlive <= 0)
                {
                    if (waveIndex < waves.Length-1)
                    {
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                    else
                    {
                        Debug.Log("game over, you win");
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
                        waveIndex++;
                        StartCoroutine(SpawnWave());
                    }
                    else
                    {
                        Debug.Log("game over, you win");
                    }
                }
            
            }
            waveCooldown += Time.deltaTime;
        }

        IEnumerator SpawnWave()
        {
            Debug.Log("spawning wave " + waveIndex);
            Wave wave = waves[waveIndex];

            if(wave.spawnRight)
            {
                spawnPoint.position = new Vector3(camHalfWidth, 0, 0);
            }
            else if (wave.spawnLeft)
            {
                spawnPoint.position = new Vector3(-camHalfWidth, 0, 0);
            }
            else if (wave.spawnTop)
            {
                spawnPoint.position = new Vector3(0, camHalfHeight, 0);
            }
            else if (wave.spawnBottom)
            {
                spawnPoint.position = new Vector3(0, -camHalfHeight,  0);
            }

            foreach (GameObject enemy in wave.enemies)
            {
                SpawnEnemy(enemy);
                yield return new WaitForSeconds(1f / wave.rate);
            }

            Debug.Log(waveIndex);
        }


        void SpawnEnemy(GameObject enemy)
        {
            enemiesAlive++;
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }


        //Event send when an enemy die, containing money reward information
        private void Enemy_OnEnDie(int reward)
        {
            Debug.Log(reward);
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
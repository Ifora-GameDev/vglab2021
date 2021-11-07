using System;
using UnityEngine;
using TMPro;

namespace Teist
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textReward;
        [SerializeField] private GameObject UICanva;
        

        private GameObject player;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int life = 2;
        [SerializeField] private int reward = 10;
        [SerializeField] private Renderer rend;


        [SerializeField] private Renderer[] rendInChilds;
        [SerializeField] private Collider2D collisionneur;

        [SerializeField] private AudioClip sfxExplode;
        private AudioSource aSource;


        /*
         * color code: 
         * 0 orange
         * 1 blue
         */
        [SerializeField] private int color;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float distanceFromWaypoint = 0.5f;
        [SerializeField] private float fireRate;


        private float attackCooldown = 0f; //cooldown

        private Waypoints path;
        private Transform target;
        //The first waypoint is the start position
        private int waypointIndex = 1;



        // Movement speed in units per second.
        public float lerpSpeed = .9f;


        public static event Action<int> OnEnDie;

        void Start()
        {
            textReward.text = "+"+ reward.ToString()+" C!";
            aSource = GetComponent<AudioSource>();
            //rend = GetComponent<Renderer>();
            collisionneur = GetComponent<Collider2D>();
            player = GameObject.Find("Player 1");
            target = path.points[waypointIndex];
        }

        // Update is called once per frame
        void Update()
        {
            if (life <= 0) return;

            //Look();

            if (player == null) return;
            if (attackCooldown <= 0f)
            {
                //On tire
                Shoot();

                attackCooldown = 1f / fireRate;
                //firerate correspond à nb coup/s; donc le cooldown est l'inverse
                //aka fireRate=2 donc fireCtdw=1/2=.5s
            }

                Look(player.transform);
                Move();

            attackCooldown -= Time.deltaTime;
        }


        private void Shoot()
        {
            Instantiate(bullet, spawnPoint.position, transform.rotation);
        }


        private void Look(Transform target)
        {
            float angle = 0;

            Vector3 relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);
        }


        private void Move()
        {
            //transform.position += transform.up * moveSpeed;

            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= distanceFromWaypoint)
            {
                GetNextWaypoint();
            }
            //Look(target);
        }

        private void GetNextWaypoint()
        {
            if (waypointIndex >= path.points.Length - 1)
            {
                OnEnDie?.Invoke(reward);
                Destroy(gameObject);
                return;
            }
            else
            {
                waypointIndex++;
                target = path.points[waypointIndex];
            }
            Look(target);

        }

        public void Init(Waypoints p)
        {
            path = p;
        }

        public int GetColor()
        {
            return color;
        }

        public void GetHit(int damage)
        {
            life -= damage;

            if (life <= 0)
            {
                collisionneur.enabled = false;
                rend.enabled = false;
                foreach (Renderer r in rendInChilds)
                {
                    r.enabled = false;
                }

                UICanva.SetActive(true);

                aSource.PlayOneShot(sfxExplode);

                OnEnDie?.Invoke(reward);
                Destroy(gameObject,2f);
            }
        }
    }
}

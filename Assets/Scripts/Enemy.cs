using System;
using System.Collections;
using UnityEngine;


namespace Teist
{
    public class Enemy : MonoBehaviour
    {
        //[SerializeField] private GameObject player;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int life = 2;
        [SerializeField] private int reward = 10;


        /*
         * color code: 
         * 0 orange
         * 1 blue
         */
        [SerializeField] private int color;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float distanceFromWaypoint=0.5f;
        [SerializeField] private float fireRate;
        [SerializeField] private bool isLerp;
        

        private float attackCooldown = 0f; //cooldown

        /*
        private float camHalfHeight;
        private float camHalfWidth;
        private float camHalfHeightMax;
        private float camHalfWidthMax;
        */
        private Waypoints path;
        private Transform target;
        //The first waypoint is the start position
        private int waypointIndex = 1;



        // Movement speed in units per second.
        public float lerpSpeed = .9f;

        // Time when the movement started.
        private float startTime;

        // Total distance between the markers.
        private float journeyLength;


        public static event Action<int> OnEnDie;

        void Start()
        {
            target = path.points[waypointIndex];

            startTime = Time.time;
            journeyLength = Vector3.Distance(path.points[0].position, target.position);
        }

        // Update is called once per frame
        void Update()
        {

            //Look();

            //distance = Vector3.Distance(player.transform.position, transform.position);
            if (attackCooldown <= 0f)
            {
                //On tire
                Shoot();

                attackCooldown = 1f / fireRate;
                //firerate correspond à nb coup/s; donc le cooldown est l'inverse
                //aka fireRate=2 donc fireCtdw=1/2=.5s
            }

            if (isLerp)
            {
                MoveLerp();
            }
            else
            {
                Move();
            }

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
            transform.position += transform.up * moveSpeed;

            if (Vector3.Distance(transform.position, target.position) <= distanceFromWaypoint)
            {
                GetNextWaypoint();
            }
            Look(target);
        }

        private void MoveLerp()
        {
            float distCovered = (Time.time - startTime * lerpSpeed);
            float fractionOfJourney = (Time.time - startTime) * lerpSpeed;

            transform.position =Vector3.Lerp(path.points[waypointIndex-1].position, target.position, fractionOfJourney);

            if (Vector3.Distance(transform.position, target.position) <= distanceFromWaypoint)
            {
                GetNextWaypoint();
            }

            Look(target);
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

                startTime = Time.time;
                journeyLength = Vector3.Distance(path.points[waypointIndex - 1].position, target.position);
            }
            Look(target);

        }

        public void Init(Waypoints p, bool lerp)
        {
            isLerp = lerp;
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
                OnEnDie?.Invoke(reward);

                Destroy(gameObject);
            }
        }
    }
}

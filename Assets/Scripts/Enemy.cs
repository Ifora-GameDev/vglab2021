using System;
using System.Collections;
using UnityEngine;


namespace Teist
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int life = 2;
        [SerializeField] private int reward = 10;


        /*
         * color code: 
         * 0 white
         * 1 black
         */
        [SerializeField] private int color;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float distanceFromWaypoint=0.5f;
        [SerializeField] private float fireRate;

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





        public static event Action<int> OnEnDie;

        void Start()
        {

            Debug.Log("Start path " + path + "received");
            target = path.points[waypointIndex];
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
                //Debug.Log(attackCooldown);
                //firerate correspond à nb coup/s; donc le cooldown est l'inverse
                //aka fireRate=2 donc fireCtdw=1/2=.5s
            }


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
            /*
            Vector3 dir = target.position - transform.position;
            dir = new Vector3(dir.x, dir.y, 0); 
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
            */
            transform.position += transform.up * moveSpeed;

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
            }
            Look(target);

        }

        public void SetPath(Waypoints p)
        {
            path = p;
        }

        public void GetHit(int damage)
        {
            Debug.Log("ouch! j'ai perdu " + damage + "... salope");
            life -= damage;

            if (life <= 0)
            {
                OnEnDie?.Invoke(reward);

                Debug.Log("enemy " + name + "has dieded");
                Destroy(gameObject);
            }
        }
    }
}

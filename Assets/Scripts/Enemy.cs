using System;
using System.Collections;
using UnityEngine;


namespace Teist
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int life = 2;
        [SerializeField] private int reward = 10;
        [SerializeField] private AnimationCurve movementSpeedCurve;
        [SerializeField] private AnimationCurve rotationCurve;
        

        /*
         * color code: 
         * 0 white
         * 1 black
         */
        [SerializeField] private int color;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotSpeed;
        [SerializeField] private float fireRate;
        private float attackCooldown = 0f; //cooldown
        private float moveCooldown = 0f; //cooldown

        private float camHalfHeight;
        private float camHalfWidth;


        public static event Action<int> OnEnDie;

        void Start()
        {

            //Find and assign player object
            //Find and assign gamemanager object

            camHalfHeight = Camera.main.orthographicSize;
            camHalfWidth = Camera.main.aspect * camHalfHeight;
            camHalfHeight = camHalfHeight * .9f;
            camHalfWidth = camHalfWidth * .9f;

            Look();
            //StartCoroutine(AutoDie());
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
            if (moveCooldown >= 1f)
            {
                moveCooldown = 0f;
            }

            if(transform.position.x<=-camHalfWidth || transform.position.x >= camHalfWidth || transform.position.y <= -camHalfHeight || transform.position.y >= +camHalfHeight)
            {
                Look();
            }

            Move();
            attackCooldown -= Time.deltaTime;
            moveCooldown += Time.deltaTime;
        }

        private void Shoot()
        {
            Instantiate(bullet, spawnPoint.position, transform.rotation);
        }

        private void Look()
        {
            float angle = 0;

            Vector3 relative = transform.InverseTransformPoint(player.transform.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);
        }

        private void Move()
        {
            transform.position += transform.up * movementSpeedCurve.Evaluate(moveCooldown) * moveSpeed;
            transform.Rotate(0, 0, rotationCurve.Evaluate(moveCooldown) * rotSpeed, Space.World);
        }

        public void GetHit(int damage)
        {
            life -= damage;

            if (life <= 0)
            {
                OnEnDie?.Invoke(reward);

                Debug.Log("enemy " + name + "has dieded");
                Destroy(gameObject);
            }
        }

        IEnumerator AutoDie()
        {
            yield return new WaitForSeconds(2);
            GetHit(2);
        }
    }
}

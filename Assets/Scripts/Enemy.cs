using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player player;
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int life=2;
    [SerializeField] private int reward=10;
    [SerializeField] private int color;
    [SerializeField] private AnimationCurve patternX;
    [SerializeField] private AnimationCurve patternY;

    /*
     * color code: 
     * 0 white
     * 1 black
     */

    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireRate;
    private float attackCooldown = 0f; //cooldown
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float distance;

    void Start()
    {
        //Find and assign player object
        //Find and assign gamemanager object

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
            Debug.Log(attackCooldown);
            //firerate correspond à nb coup/s; donc le cooldown est l'inverse
            //aka fireRate=2 donc fireCtdw=1/2=.5s

        }
        Move();
        attackCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        Debug.Log("piou piou!");
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
        transform.position+=transform.up*patternX.Evaluate(Time.deltaTime * moveSpeed);
        transform.position += transform.right*patternY.Evaluate(Time.deltaTime * moveSpeed);
    }

    public void GetHit(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            Debug.Log("enemy " + name + "has dieded");
            Destroy(gameObject);
        }
    }
}

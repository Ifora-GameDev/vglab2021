using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teist;

public class BulletController : MonoBehaviour
{

    [SerializeField] private int damage;

    /*
     * color code: 
     * 0 white
     * 1 black
     */
    [SerializeField] private int color;
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //If the bullet collide with the player, he takes damage
        if (col.gameObject.tag == "Enemy")
        {
            Enemy e = col.gameObject.GetComponent<Enemy>();
            e.GetHit(damage);
            //hit Player
            Debug.Log("gagné");
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bomb : MonoBehaviour
{
    public float explosionRadius = 10f;

    public float _fireRate = 1f;

    private float _nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    void processInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= _nextFireTime)
            {
                explode();
                _nextFireTime = Time.time + 1 / _fireRate;
            }
            
        }
    }

    void explode()
    {
        Collider2D[] allColider = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        foreach(Collider2D collider in allColider)
        {
            if((collider.gameObject.tag == "Enemy") || 
                collider.gameObject.tag == "Bullet")
            {
                Destroy(collider.gameObject);
            }
            
        }
    }
}

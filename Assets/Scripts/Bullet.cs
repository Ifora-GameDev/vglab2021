using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;

    // Update is called once per frame
    void Update()
    {
        //When created from Enemy, go forward with base on its original rotation
        transform.position += transform.up* moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //If the bullet collide with the player, he takes damage
        if(col.gameObject.tag=="Player")
        {
            //hit Player
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        //If the bullet go out of screen, it's destroyed
        Destroy(gameObject);
    }
}

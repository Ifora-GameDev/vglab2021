using UnityEngine;

namespace Teist
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int damage;

        /*
         * color code: 
         * 0 orange
         * 1 blue
         */
        [SerializeField] private int color;

        // Update is called once per frame
        void Update()
        {
            //When created from Enemy, go forward with base on its original rotation
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("tu m'as eu " + damage);
            //If the bullet collide with the player, he takes damage
            if (col.gameObject.tag == "Player")
            {
                PlayerManager pm = col.gameObject.GetComponent<PlayerManager>();
                pm.GetHit(damage);
                //hit Player
                Debug.Log("perdu");
                Destroy(gameObject);
            }
        }

        void OnBecameInvisible()
        {
            //If the bullet go out of screen, it's destroyed
            Destroy(gameObject);
        }
    }
}

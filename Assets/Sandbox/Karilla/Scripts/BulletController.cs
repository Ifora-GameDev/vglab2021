using UnityEngine;
using Teist;

public class BulletController : MonoBehaviour
{

    [SerializeField] private int damage;

    /*
     * color code: 
     * 0 orange
     * 1 blue
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
            int c = e.GetColor();
            Debug.Log("couleur ennemi: " + c);
            if (color == c)
            {
                e.GetHit(damage*2);
            }
            else
            {
                e.GetHit(damage);
            }

            //hit Player
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

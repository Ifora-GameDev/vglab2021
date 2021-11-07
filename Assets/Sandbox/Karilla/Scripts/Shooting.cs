using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform[] firePoint;

    public int nbShot;

    public GameObject[] bulletObj;

    public float bulletForce = 20f;

    public float _fireRate = 2f;

    /*
     * color code: 
     * 0 orange
     * 1 blue
     */
    [SerializeField] private int color=0;
    // Start is called before the first frame update

    private float _nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    void processInput()
    {
        if (Input.GetButton("Fire1")) { Shoot(); };
        if (Input.GetButton("Fire2")) { ChangeColor(); };
    }

    void Shoot()
    {
        
        if (Time.time >= _nextFireTime)
        {
            
            for(int i = 0; i < nbShot; i++)
            {
                GameObject bullet = Instantiate(bulletObj[color], firePoint[i].position, firePoint[i].rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(firePoint[i].up * bulletForce, ForceMode2D.Impulse);
            }

            _nextFireTime = Time.time + 1 / _fireRate;
        }
        
    }
    void ChangeColor()
    {
        if (color == 0) { color = 1; }
        else { color = 0; }
    }
}

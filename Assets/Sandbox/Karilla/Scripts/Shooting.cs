using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform[] firePoint;

    public int nbShot;

    public GameObject[] bulletObj;

    public float bulletForce = 20f;

    public float _fireRate = 2f;

    public SpriteRenderer baseCanon;

    public SpriteRenderer canon;

    private Color orange;

    private Color blue;
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
        orange = new Color(1, 0.5333334f, 0, 1);
        blue = new Color(0,0.9529412f,1,1);

        canon.color = blue;
        baseCanon.color = blue;
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    void processInput()
    {
        if (Input.GetButton("Fire1")) { Shoot(); };
        if (Input.GetButtonDown("Fire2")) { ChangeColor(); };
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
        if (color == 0)
        {
            color = 1;
            canon.color = orange;
            baseCanon.color = orange;
        }
        else {
            color = 0;
            canon.color = blue;
            baseCanon.color = blue;
        }
    }
}

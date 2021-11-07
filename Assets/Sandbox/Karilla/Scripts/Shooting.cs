using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bulletObj;

    public float bulletForce = 20f;

    public float _fireRate = 2f;

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
    }

    void Shoot()
    {
        if (Time.time >= _nextFireTime)
        {
            GameObject bullet = Instantiate(bulletObj, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

            _nextFireTime = Time.time + 1 / _fireRate;
        }
    }
}

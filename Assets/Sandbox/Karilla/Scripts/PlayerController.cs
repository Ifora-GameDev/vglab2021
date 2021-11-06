using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 10f;

    public Rigidbody2D rb;

    private Vector2 movementDirection;

    private Camera cam;

    private Vector3 viewportPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    void OnBecameVisible()
    {
        Debug.Log("Coucou");
    }

    void Update()
    {
        processInput();
    }

    void OnBecameInvisible()
    {
        viewportPosition = cam.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;

        if (viewportPosition.x > 1 || viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x;
        }

        if (viewportPosition.y > 1 || viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y;
        }

        transform.position = newPosition;
    }

    private void FixedUpdate()
    {
        move();
    }

    void processInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;
    }

    void move()
    {
        rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
    }

}

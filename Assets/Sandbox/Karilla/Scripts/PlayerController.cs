using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 10f;

    public Rigidbody2D rb;

    private Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        processInput();
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

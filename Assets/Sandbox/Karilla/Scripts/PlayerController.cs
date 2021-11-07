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

    private Vector2 mousePos;


    private float camHalfHeight;
    private float camHalfWidth;
    private float camHalfHeightMax;
    private float camHalfWidthMax;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = Camera.main.aspect * camHalfHeight;
        camHalfHeight = camHalfHeight * .9f;
        camHalfWidth = camHalfWidth * .9f;
    }

    void OnBecameVisible()
    {
    
    }

    void Update()
    {
        processInput();
    }
    /*
    void OnBecameInvisible()
    {
        if(cam == null)
        {
            return;
        }
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
    }*/

    private void FixedUpdate()
    {
        move();
        rotate();

    }

    void processInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void move()
    {
        rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
        if (transform.position.x < -camHalfHeight)
        {
            //transform.position.x = camHalfHeight;
        }

    }

    float getAngleMouse()
    {
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        return angle;
    }
    
    void rotate()
    {
        rb.rotation = getAngleMouse();
    }
}

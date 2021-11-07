using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 10f;

    public Rigidbody2D rb;

    private Vector2 movementDirection;

    private Camera cam;

    private Vector2 mousePos;


    private float camHalfHeight;
    private float camHalfWidth;

    private bool _areInputsEnable = true;

    private void OnEnable()
    {
        Teist.GameManager.OnWaveEnd += ToggleInputsActive;
        Teist.GameManager.OnGameWin += ToggleInputsActive;
        HackController.OnHackEnd += ToggleInputsActive;
    }

    private void OnDisable()
    {
        Teist.GameManager.OnWaveEnd -= ToggleInputsActive;
        Teist.GameManager.OnGameWin -= ToggleInputsActive;
        HackController.OnHackEnd -= ToggleInputsActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = Camera.main.aspect * camHalfHeight;
        camHalfHeight = camHalfHeight * .9f;
        camHalfWidth = camHalfWidth * .9f;
    }


    void Update()
    {
        processInput();
    }

    private void FixedUpdate()
    {
        move();
        rotate();
    }

    private void ToggleInputsActive()
    {
        _areInputsEnable = !_areInputsEnable;
    }

    private void ToggleInputsActive(int _)
    {
        _areInputsEnable = !_areInputsEnable;
    }

    void processInput()
    {
        if(!_areInputsEnable)
        {
            Debug.Log("Aled");
            rb.velocity = new Vector2(0, 0);
            return;
        }
        else
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            movementDirection = new Vector2(moveX, moveY).normalized;

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        
    }

    void move()
    {
        rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);


        //Check to stay in screen
        if (transform.position.x < -camHalfWidth)
        {
            if (movementDirection.x < 0)
            {
                rb.velocity = new Vector2(0, movementDirection.y * movementSpeed);
            }
            
        }
        if (transform.position.x > camHalfWidth)
        {
            if (movementDirection.x > 0)
            {
                rb.velocity = new Vector2(0, movementDirection.y * movementSpeed);
            }
        }

        if (transform.position.y < -camHalfHeight)
        {
            if (movementDirection.y < 0)
            {
                rb.velocity = new Vector2(movementDirection.x * movementSpeed,0);
            }
        }
        if (transform.position.y > camHalfHeight)
        {
            if (movementDirection.y > 0)
            {
                rb.velocity = new Vector2(movementDirection.x * movementSpeed, 0);
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapin : MonoBehaviour
{
    public GameObject startPoint;

    private LineRenderer lineRenderer;
    
    public float distanceGrapinMax = 10f;

    public float expandTime = 1f;

    private Coroutine myCoroutine;

    private Rigidbody2D rb2d;

    private Camera cam;

    private Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    void processInput()
    {
        if (Input.GetButtonDown("Fire2")) { launchGrapin(); }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void launchGrapin()
    {
        Vector2 direction = mousePos - rb2d.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint.transform.position, direction, distanceGrapinMax);
        Debug.DrawRay(startPoint.transform.position, direction,Color.black , distanceGrapinMax);
        /*
        if(myCoroutine == null)
        {
            myCoroutine = StartCoroutine(drawLine(startPoint));
        }
        */


    }
    /*
    IEnumerator drawLine(GameObject startPoint)
    {
        lineRenderer.positionCount = 1000;
        float elapsedTime = 0;
        Vector2 cachedStartPoint = startPoint.transform.position;
        Vector2 endpoint = startPoint.transform.up * distanceGrapinMax;
        Vector2 hookPosition = cachedStartPoint;
        int index = 0;
        while (elapsedTime < expandTime)
        {
            Debug.Log(index);
            hookPosition = Vector2.Lerp(cachedStartPoint, endpoint, elapsedTime / expandTime);

            lineRenderer.SetPosition(index, hookPosition);

            index++;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Exit");
        myCoroutine = null;

    }
    */
}

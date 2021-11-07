using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    public GameObject objectToBlink;
    public float timeON = 0.5f;
    public float blinkTime = 1f;
    private float elapsedtime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        objectToBlink.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedtime > blinkTime)
        {
            elapsedtime = 0;
        }
        if(elapsedtime > timeON)
        {
            objectToBlink.SetActive(false);
        }
        else
        {
            objectToBlink.SetActive(true);
        }
        elapsedtime += Time.deltaTime;
        
    }
}

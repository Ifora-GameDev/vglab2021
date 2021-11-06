using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenWrapper : MonoBehaviour
{
    Renderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        //renderers = GetComponentsInChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CheckRenderers()
    {
        foreach(var renderer in renderers)
        {
            if(renderer.isVisible)
            {
                return true;
            }
        }
        return false;
    }
}

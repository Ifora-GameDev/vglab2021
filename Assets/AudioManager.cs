using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip sfxExplode;
    private AudioSource aSource;

    void Start()
    {
        aSource = GetComponent<AudioSource>();
        aSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

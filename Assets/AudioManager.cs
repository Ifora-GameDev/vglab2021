using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource aSource;

    void Start()
    {
        aSource = GetComponent<AudioSource>();
        aSource.loop = true;
    }
}

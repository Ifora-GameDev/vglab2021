using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] private AudioSource aSource;
    [SerializeField] private AudioClip sfxCoin;
    [SerializeField] private AudioClip sfxBtnPress;

    public void Play()
    {
        aSource.PlayOneShot(sfxCoin);
        StartCoroutine(WaitForSound(2));
    }

    IEnumerator WaitForSound(int id)
    {
        yield return new WaitUntil(() => aSource.isPlaying == false);
        SceneManager.LoadScene(id);
        Debug.Log("load scene" + id);
    }
    public void MainMenu()
    {
        //SceneManager.LoadScene(0);
        aSource.PlayOneShot(sfxBtnPress);
        StartCoroutine(WaitForSound(0));
    }

    public void Credit()
    {
        aSource.PlayOneShot(sfxBtnPress);
        StartCoroutine(WaitForSound(1));
        //SceneManager.LoadScene(1);
    }
}

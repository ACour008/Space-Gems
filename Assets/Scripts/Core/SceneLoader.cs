using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MiiskoWiiyaas.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] CanvasGroupFadeInComponent fadeIn;
    [SerializeField] CanvasGroupFadeOutComponent fadeOut;

    private IEnumerator LoadScene(int index)
    {
        fadeOut.Fade();
        yield return new WaitForSeconds(fadeOut.FadeTime + fadeOut.StartDelay);
        SceneManager.LoadScene(index);
    }

    public void LoadSceneIndex(int index)
    {
        StartCoroutine(LoadScene(index));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
}

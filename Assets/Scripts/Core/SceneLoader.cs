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

    /// <summary>
    /// Loads the scene of the given build index.
    /// </summary>
    /// <param name="index">The build index of the scene to load.</param>
    public void LoadSceneIndex(int index)
    {
        StartCoroutine(LoadScene(index));
    }

    /// <summary>
    /// Loads the scene of the next build index.
    /// </summary>
    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
}

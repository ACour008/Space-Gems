using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableAnimations
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private bool playInTransitionOnStart = true;

        private void Start()
        {
            if (playInTransitionOnStart)
            {

            }
        }

        private IEnumerator LoadScene(int index)
        {
            

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(index);
        }

        public void LoadNextScene()
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

}

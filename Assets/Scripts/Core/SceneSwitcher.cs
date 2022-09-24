using UnityEngine;
using MiiskoWiiyaas.UI;

namespace MiiskoWiiyaas.Core
{
    /// <summary>
    /// Subscribes the SceneLoader to a MenuToggler button click event, all given
    /// through Unity's inspector.
    /// </summary>
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] SceneLoader sceneLoader;
        [SerializeField] MenuToggler menuToggler;
        [SerializeField] int switchSceneIndex;

        private void Awake()
        {
            menuToggler.OnConfirmButtonClicked += MenuToggler_OnConfirmButtonClicked;
        }

        private void MenuToggler_OnConfirmButtonClicked(object sender, System.EventArgs e)
        {
            sceneLoader.LoadSceneIndex(switchSceneIndex);
        }
    }
}

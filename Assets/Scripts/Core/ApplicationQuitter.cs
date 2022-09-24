using UnityEngine;
using MiiskoWiiyaas.UI;

namespace MiiskoWiiyaas.Core
{
    public class ApplicationQuitter : MonoBehaviour
    {
        [SerializeField] ButtonHandler quitButton;

        private void Start() => quitButton.OnClicked += QuitButton_OnClicked;

        private void QuitButton_OnClicked(object sender, System.EventArgs e) => Application.Quit(0);
    }
}

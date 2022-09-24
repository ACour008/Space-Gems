using UnityEngine;
using MiiskoWiiyaas.Core;

namespace MiiskoWiiyaas.UI
{
    public class GemCellUISelector : MonoBehaviour
    {
        [SerializeField] private GameObject selectionUI;

        private void Awake()
        {
            selectionUI.SetActive(false);
        }

        /// <summary>
        /// Activates the selection icon for the selected cell.
        /// </summary>
        /// <param name="cell">The cell for which the selection icon will appear over.</param>
        public void Select(GemCell cell)
        {
            selectionUI.transform.position = cell.transform.position;
            selectionUI.SetActive(true);
        }

        /// <summary>
        /// De-activates the selection icon regardless of its current position.
        /// </summary>
        public void Deselect()
        {
            selectionUI.SetActive(false);
        }
    }
}

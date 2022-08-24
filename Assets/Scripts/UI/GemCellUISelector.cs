using System;
using UnityEngine;
using MiiskoWiiyaas.Core;

namespace MiiskoWiiyaas.UI
{
    public class GemCellUISelector : MonoBehaviour
    {
        [SerializeField] private GameObject selectionUI;
        private GemCell selectedCell;

        private void Awake()
        {
            selectionUI.SetActive(false);
        }

        public void Select(GemCell cell)
        {
            selectedCell = cell;
            selectionUI.transform.position = cell.transform.position;
            selectionUI.SetActive(true);
        }

        public void Deselect()
        {
            selectionUI.SetActive(false);
            selectedCell = null;
        }
    }
}

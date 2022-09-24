using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiiskoWiiyaas.Core
{
    public class GemCell : MonoBehaviour, IClickable, IPointerClickHandler
    {
        public static GemCell Selected;

        [SerializeField] private Gem currentGem;

        private int id;
        private bool canClick = true;
        private Dictionary<string, GemCell> neighbors;

        public event EventHandler OnClicked;

        public int ID { get => id; }
        public bool CanClick { set => canClick = value; }

        public Gem CurrentGem { 
            get => currentGem; 
            set
            {
                currentGem = value;
                if (value != null) currentGem.transform.SetParent(this.transform);
            }
        }

        /// <summary>
        /// Connects neighboring cells within the game grid.
        /// </summary>
        /// <param name="direction">A string that specifies which direction the neighbor is relative to the current GemCell.</param>
        /// <param name="neighbor">The neighboring GemCell.</param>
        public void AddNeighbor(string direction, GemCell neighbor)
        {
            neighbors.Add(direction, neighbor);
        }

        private void Awake()
        {
            neighbors = new Dictionary<string, GemCell>();
        }

        /// <summary>
        /// Gets all neighbors of the current GemCell.
        /// </summary>
        /// <returns>A dictionary with a string/GemCell key-value pair.</returns>
        public Dictionary<string, GemCell> GetAllNeighbors()
        {
            return neighbors;
        }

        /// <summary>
        /// Sets the id of the GemCell.
        /// </summary>
        /// <param name="id">the id of the cell.</param>
        public void SetCellId(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Checks to see if the given cell is a neighbor of the current cell.
        /// </summary>
        /// <param name="cell">A GemCell object</param>
        /// <returns>A boolean that indicates whether the given cell is a neighbor of the current cell.</returns>
        /// <remarks>Avoided iterating over dictionary because there are other cardinal neighbors that do not
        /// need to be checked when user is swapping Gems.</remarks>
        public bool IsNeighborsWith(GemCell cell)
        {
            return neighbors.GetValueOrDefault<string, GemCell>("north") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("south") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("east") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("west") == cell;
        }

        /// <summary>
        /// Attempts to get a neighboring GemCell of a given direction.
        /// </summary>
        /// <param name="direction">A string that specifies the which neighbor to return</param>
        /// <returns>Either a GemCell object if the current cell has a neighbor in the given direction, otherwise, returns null.</returns>
        public GemCell TryGetNeighbor(string direction)
        {
            neighbors.TryGetValue(direction, out GemCell neighbor);
            return neighbor;
        }

        /// <summary>
        /// Invokes the OnClicked event when user clicks the current cell.
        /// </summary>
        /// <param name="eventData">A UnityEngine event payload associated with pointer events. Unused in this context.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (canClick) OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString() => $"Cell {id}";
    }
}

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

        public void AddNeighbor(string direction, GemCell neighbor)
        {
            neighbors.Add(direction, neighbor);
        }

        private void Awake()
        {
            neighbors = new Dictionary<string, GemCell>();
        }

        public Dictionary<string, GemCell> GetAllNeighbors()
        {
            return neighbors;
        }

        public void Init(int id)
        {
            this.id = id;
        }

        public bool IsNeighborsWith(GemCell cell)
        {
            return neighbors.GetValueOrDefault<string, GemCell>("north") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("south") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("east") == cell ||
                   neighbors.GetValueOrDefault<string, GemCell>("west") == cell;
        }


        public GemCell TryGetNeighbor(string direction)
        {
            neighbors.TryGetValue(direction, out GemCell neighbor);
            return neighbor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (canClick) OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString() => $"Cell {id}";
    }
}

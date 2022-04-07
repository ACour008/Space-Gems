using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using MiskoWiiyaas.Events;

namespace MiskoWiiyaas.Cells
{
    public class GridCell : Cell, IPointerClickHandler
    {
        [SerializeField] private GameObject selection;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private float targetYPosition;
        [SerializeField] private float scoreDuration;

        private Dictionary<string, GridCell> neighbors;
        private Vector3 initialPosition;
        private Color initialColor;
        private RectTransform scoreTextRT;

        public event EventHandler<CellEventArgs> OnClick;
        public static GridCell selected;
        public bool scoreIsPlaying = false;

        public bool IsActive { get; set; }

        private void Awake()
        {
            neighbors = new Dictionary<string, GridCell>();
            scoreTextRT = scoreText.gameObject.GetComponent<RectTransform>();
            initialPosition = scoreTextRT.localPosition;
            initialColor = scoreText.color;
        }

        private void Update()
        {
            if (selected == this && IsActive)
            {
                selection.SetActive(true);
            }
            else
            {
                selection.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selected == this)
            {
                selected = null;
            } 
            else
            { 
                selected = this;
            }
            OnClick?.Invoke(this, new CellEventArgs { cell = this });
        }

        private void Tile_OnTileChange(object sender, CellEventArgs e)
        {
            e.tile.transform.SetParent(this.transform);
            _currentTile = e.tile;
            _currentTile.SetCurrentCellID(_id);
        }

        public void PlayScoreAnimation(int score)
        {
            scoreText.text = score.ToString();

            scoreTextRT.localPosition = initialPosition;

            // For now until you can figure out how to access faceColor & outlineColor
            Color newColor = new Color(currentTile.Color.main.r, currentTile.Color.main.g, currentTile.Color.main.b, 1);
            scoreText.color = newColor;

            StartCoroutine(DoAnimation(score));
        }

        private IEnumerator DoAnimation(int score)
        {
            Vector3 targetPosition = initialPosition + new Vector3(0, targetYPosition, 0);
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
            float timeStart = Time.time;

            scoreIsPlaying = true;
            scoreText.gameObject.SetActive(true);

            while (Vector3.Distance(scoreTextRT.localPosition, targetPosition) > 0.05f)
            {
                float timeSinceStart = Time.time - timeStart;
                float complete = timeSinceStart / scoreDuration;

                scoreTextRT.localPosition = Vector3.Lerp(scoreTextRT.localPosition, targetPosition, complete);
                yield return null;
            }

            while (scoreText.color.a > 0.05f)
            {
                float timeSinceStart = Time.time - timeStart;
                float complete = timeSinceStart / scoreDuration;

                scoreText.color = Color.Lerp(scoreText.color, targetColor, complete);
                yield return null;

            }

            scoreText.color = targetColor;
            scoreTextRT.localPosition = targetPosition;

            scoreIsPlaying = false;
            scoreText.gameObject.SetActive(false);
        }

        public void AddNeighbor(string direction, GridCell neighbor) => neighbors.Add(direction, neighbor);

        public GridCell GetNeighbor(string direction)
        {
            if (neighbors.ContainsKey(direction))
            {
                return neighbors[direction];
            }

            return null;
        }

        public GridCell GetNextNeighbor(string direction)
        {
            return GetNeighbor(direction)?.GetNeighbor(direction);
        }

        public Dictionary<string, GridCell> GetAllNeighbors()
        {
            return neighbors;
        }

        public void SetAllNeighbors(Dictionary<string, GridCell> newNeighbors)
        {
            neighbors = newNeighbors;
        }

        public bool IsNeighbor(GridCell cell)
        {
            foreach(KeyValuePair<string, GridCell> neighbor in neighbors)
            {
                if (neighbor.Value == cell)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString() => $"GridCell {_id}";
        public override int GetHashCode() => _id.GetHashCode();
        public override bool Equals(object other)
        {
            if ( other == null || !this.GetType().Equals(other.GetType()))
            {
                return false;
            }
            else
            {
                GridCell _other = (GridCell)other;
                return _other._id == _id;
            }
        }

    }
}
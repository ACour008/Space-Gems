using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Events;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas.Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType _tileType;
        [SerializeField] private int _currentCellId;
        [SerializeField] private int _value = 100;

        [Header("Animation")]
        [SerializeField] private float swapDurationInMS;
        [SerializeField] private float disappearDurationInMS;
        [SerializeField] private Vector3 disappearScaleUp;

        [SerializeField] private Sprite[] tiles;
        [SerializeField] private TileColor[] colors;

        private TileColor currentColor;
        private SpriteRenderer spriteRenderer;

        private bool _isMoving = false;
        private bool _isDisappearing = false;

        public bool IsMoving { get => _isMoving; }
        public bool IsDisappearing { get => _isDisappearing; }
        public TileColor Color { get => currentColor; }

        public TileType tileType { get => _tileType; }
        public int currentCellId { get => _currentCellId; }
        public int Value { get => _value; }

        public event EventHandler<CellEventArgs> OnTileChange;
        public event EventHandler<CellEventArgs> OnTileDestroyed;
        public event EventHandler<CellEventArgs> OnTileMoveDone;

        public void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _isMoving = false;
            _isDisappearing = false;
        }

        public void SetCurrentCellID(int id)
        {
            _currentCellId = id;
        }

        public void AssignTile(TileType type)
        {
            _tileType = type;
            gameObject.name = type.ToString();
            spriteRenderer.sprite = tiles[(int)type];
            currentColor = colors[(int)type];
            OnTileChange?.Invoke(this, new CellEventArgs { tile = this });
        }

        #region NewMoves

        public IEnumerator MoveTo(Cell target, bool invokeEvent)
        {
            float timeStart = Time.time;

            while(Vector3.Distance(transform.position, target.transform.position) > 0.05f)
            {
                _isMoving = true;

                float timeSinceStart = Time.time - timeStart;
                float completed = (timeSinceStart / (swapDurationInMS * Time.deltaTime));

                transform.position = Vector3.Lerp(transform.position, target.transform.position, completed);
                yield return null;
            }

            _isMoving = false;
            transform.position = target.transform.position;
            transform.SetParent(target.transform);
            target.currentTile = this;

            if (invokeEvent) { OnTileMoveDone?.Invoke(this, new CellEventArgs()); }
        }

        #endregion

        public void AddToValueBy(float multiplier)
        {
            _value = (int)(_value * multiplier);
        }

        public void CreateRandomTile(int cellID, List<TileType> possibleTypes)
        {
            _currentCellId = cellID;

            int r = UnityEngine.Random.Range(0, possibleTypes.Count);
            AssignTile(possibleTypes[r]);
        }

        public void Destroy()
        {
            StartCoroutine("Disappear");
        }

        private IEnumerator Disappear()
        {
            float timeStart = Time.time;
            Vector3 targetScale = transform.localScale + disappearScaleUp;

            _isDisappearing = true;
            while (Vector3.Distance(transform.localScale, targetScale) > 0.1f)
            {
                float timeSinceStart = Time.time - timeStart;
                float complete = timeSinceStart / ((disappearDurationInMS / 2) * Time.deltaTime);

                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Mathf.SmoothStep(0, 1, complete));
                yield return null;
            }
            transform.localScale = targetScale;

            timeStart = Time.time;
            while(Vector3.Distance(transform.localScale, Vector3.zero) > 0.1f)
            {
                float timeSinceStart = Time.time - timeStart;
                float complete = timeSinceStart / ((disappearDurationInMS / 2) * Time.deltaTime);

                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Mathf.SmoothStep(0, 1, complete));
                yield return null;

            }
            transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);

            _isDisappearing = false;
            OnTileDestroyed?.Invoke(this, new CellEventArgs());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public override string ToString() => $"{_tileType}";
    }
}
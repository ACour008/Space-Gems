using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Tiles;
using MiskoWiiyaas.Events;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas.Cells
{
    public abstract class Cell: MonoBehaviour
    {
        protected int _id;
        [SerializeField] protected Tile _currentTile;

        public int id { get => _id; } 

        public Tile currentTile
        {
            get { return _currentTile; }

            set
            {
                _currentTile = value;
                if (value != null && _currentTile != null) _currentTile.SetCurrentCellID(_id);
            }
        }

        public void SetID(int id)
        {
            _id = id;
        }

        private void Tile_OnTileChange(object sender, CellEventArgs e)
        {
            e.tile.transform.SetParent(this.transform);
            _currentTile = e.tile;
            _currentTile.SetCurrentCellID(_id);
        }

        #region TileManipulation
        public Tile CreateTile(GameObject prefab, List<TileType> possibleTypes, Transform parent)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);

            Tile tile = instance.GetComponent<Tile>();
            tile.OnTileChange += Tile_OnTileChange;
            tile.CreateRandomTile(_id, possibleTypes);

            return tile;
        }

        public void DestroyTile()
        {
            currentTile.Destroy();
            currentTile = null;
        }

        #endregion
    }
}
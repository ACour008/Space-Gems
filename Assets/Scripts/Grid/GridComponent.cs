using UnityEngine;
using MiiskoWiiyaas.Grid;
using MiiskoWiiyaas.Builders;

namespace MiiskoWiiyaas.Core
{
    public class GridComponent : MonoBehaviour
    {
        [SerializeField] private int gridRows;
        [SerializeField] private int gridCols;
        [SerializeField] private int gridCellSize;
        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private GameObject gridItemPrefab;
        [SerializeField] private Vector3 gridStartPosition;

        [SerializeField] private GameGrid<GemCell> grid;
        [SerializeField] private GameSystems gridSystem;
        
        private void Start()
        {
            GemCellBuilder gemBuilder = new GemCellBuilder(gridCellPrefab, gridItemPrefab, gridRows, gridCols);
            
            grid = new GameGrid<GemCell>(gridRows, gridCols, gridCellSize, gemBuilder);

            grid.Build(gridStartPosition, transform, gridSystem.InputHandler, gridSystem.SFXPlayer);

            gridSystem.Initialize(grid, gridItemPrefab);
        }


    }
}
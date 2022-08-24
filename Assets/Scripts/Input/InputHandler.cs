using System;
using System.Collections;
using UnityEngine;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GemCell[] selectedCells;
    [SerializeField] private MenuToggler gameOverUIToggler;
    private MatchFinder matchFinder;
    private MatchChecker matchChecker;
    private GemMover gemMover;
    private GemCellUISelector uiSelector;
    private bool matchFindingCompleted = false;

    public event EventHandler OnMatchFindingDone;
    public event EventHandler OnTilesSelected;
    public event EventHandler OnTilesDeselected;

    private void Awake() => selectedCells = new GemCell[2];

    private void ClearSelectedCells()
    {
        uiSelector.Deselect();
        selectedCells[0] = null;
        selectedCells[1] = null;
        GemCell.Selected = null;
    }

    public void GemCell_OnClicked(object sender, EventArgs eventArgs)
    {
        GemCell clickedCell = sender as GemCell;
        bool noTileSelected = selectedCells[0] == null;

        if (noTileSelected)
        {
            SelectFirstCell(clickedCell);
        }
        else
        {
            bool clickedTileAlreadySelected = selectedCells[0] == clickedCell;

            if (clickedTileAlreadySelected)
            {
                ClearSelectedCells();
                return;
            }

            if (selectedCells[0].IsNeighborsWith(clickedCell))
            {
                StartCoroutine(HandleInput(clickedCell));
            }
            else
            {
                SelectFirstCell(clickedCell);
            }
        }
    }

    public void GridRetiler_OnGridRetiled(object sender, EventArgs eventArgs) => matchFindingCompleted = true;

    public IEnumerator HandleInput(GemCell clickedCell)
    {
        uiSelector.Deselect();

        OnTilesSelected?.Invoke(this, EventArgs.Empty);

        selectedCells[1] = clickedCell;

        gemMover.Swap(selectedCells[0], selectedCells[1]);
        yield return new WaitUntil(() => gemMover.IsFinishedMoving);

        bool matchFound = matchFinder.FindAllMatches(selectedCells, true);

        if (matchFound)
        {
            do
            {
                matchFindingCompleted = false;
                yield return new WaitUntil(() => matchFindingCompleted);
            }
            while (matchFinder.FindAllMatches(selectedCells, false));

            if (!matchChecker.PotentialMatchesExist)
            {
                gameOverUIToggler.OpenMenu();
            }
            else
            {
                OnMatchFindingDone?.Invoke(this, EventArgs.Empty);
                OnTilesDeselected?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            gemMover.Swap(selectedCells[0], selectedCells[1]);
            OnTilesDeselected?.Invoke(this, EventArgs.Empty);
        }
        ClearSelectedCells();
    }

    public void Initialize(MatchFinder matchFinder, MatchChecker matchChecker, GemMover gemMover, GemCellUISelector uiSelector)
    {
        this.matchFinder = matchFinder;
        this.matchChecker = matchChecker;
        this.gemMover = gemMover;
        this.uiSelector = uiSelector;
    }

    private void SelectFirstCell(GemCell clickedCell)
    {
        GemCell.Selected = clickedCell;
        selectedCells[0] = clickedCell;
        selectedCells[1] = null;
        uiSelector.Select(clickedCell);
    }
}

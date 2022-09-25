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

    #region Events
    public event EventHandler OnMatchFindingDone;
    public event EventHandler OnTilesSelected;
    public event EventHandler OnTilesDeselected;
    #endregion

    private void Awake() => selectedCells = new GemCell[2];

    private void ClearSelectedCells()
    {
        uiSelector.Deselect();
        selectedCells[0] = null;
        selectedCells[1] = null;
        GemCell.Selected = null;
    }

    private void EndGame()
    {
        gameOverUIToggler.OpenMenu();
    }

    /// <summary>
    /// Handles GemCell clicks by either selecting or unselecting the cell on the first click.
    /// On the second click, the swap and match-finding processes will begin if the cell is a neighbor.
    /// </summary>
    /// <param name="sender">The GemCell that invoked the event.</param>
    /// <param name="eventArgs">An empty System.EventArgs object.</param>
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
                StartCoroutine(StartMatchFinding(clickedCell));
            }
            else
            {
                SelectFirstCell(clickedCell);
            }
        }
    }
    /// <summary>
    /// Notifies the InputHandler that the matching finding process is complete
    /// the game grid was retiled.
    /// </summary>
    /// <param name="sender">The GridRetiler object that invoked the event.</param>
    /// <param name="eventArgs">An empty System.EventArgs object.</param>
    public void GridRetiler_OnGridRetiled(object sender, EventArgs eventArgs) => matchFindingCompleted = true;


    private IEnumerator StartMatchFinding(GemCell clickedCell)
    {
        OnTilesSelected?.Invoke(this, EventArgs.Empty);
        uiSelector.Deselect();

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
                EndGame();
            }
            else
            {
                InvokeEndOfTurnEvents();
            }
        }
        else
        {
            gemMover.Swap(selectedCells[0], selectedCells[1]);
            OnTilesDeselected?.Invoke(this, EventArgs.Empty);
        }
        ClearSelectedCells();
    }

    private void InvokeEndOfTurnEvents()
    {
        OnMatchFindingDone?.Invoke(this, EventArgs.Empty);
        OnTilesDeselected?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sets up the InputHandler
    /// </summary>
    /// <param name="matchFinder">The object that searches for matches of 3 or more. Not to be confused with MatchChecker.</param>
    /// <param name="matchChecker">The object that checks for potential matches without triggering move animations.</param>
    /// <param name="gemMover">The object responsible for moving Gems to specified targets</param>
    /// <param name="uiSelector">The object that handles the UI when grid cells are clicked.</param>
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

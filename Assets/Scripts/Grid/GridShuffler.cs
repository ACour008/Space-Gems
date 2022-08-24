using System;
using UnityEngine;
using MiiskoWiiyaas.Audio;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.UI;

namespace MiiskoWiiyaas.Grid
{
    public class GridShuffler : MonoBehaviour
    {
        [SerializeField] private int maxShuffles = 1;
        [SerializeField] private int shufflesLeft = 0;
        [SerializeField] private ButtonHandler shuffleButton;
        [SerializeField] private SFXClip shuffleSFXClip;

        private GameGrid<GemCell> grid;
        private MatchChecker matchChecker;
        private Switcher switcher;

        private void Awake()
        {
            shuffleButton.OnClicked += ShuffleButton_OnClicked;
        }

        private void ShuffleButton_OnClicked(object sender, EventArgs e) => Shuffle();

        public void Initialize(GameGrid<GemCell> grid, MatchChecker matchChecker)
        {
            shufflesLeft = maxShuffles;
            switcher = new Switcher();
            this.grid = grid;
            this.matchChecker = matchChecker;
        }

        private void DoShuffle()
        {
            for (int i = grid.Cells.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                GemCell cell1 = grid.Cells[i];
                GemCell cell2 = grid.Cells[j];

                if (!matchChecker.SwitchWouldCauseMatch(cell1, cell2))
                {
                    cell1.CurrentGem.Move(cell2.transform.position);
                    cell2.CurrentGem.Move(cell1.transform.position);

                    switcher.ExchangeGems(cell1, cell2);
                }
            }
        }

        public void Shuffle()
        {
            if (shufflesLeft == 0) return;
            shuffleSFXClip.Play();
            DoShuffle();
            shufflesLeft--;
        }
    }
}
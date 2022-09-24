using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Core;
using MiiskoWiiyaas.Core.Events;
using FMODUnity;
using FMOD.Studio;

namespace MiiskoWiiyaas.Audio
{
    public class GridSFXPlayer : MonoBehaviour
    {
        [SerializeField] private EventReference gemCellSelectedEventReference;
        [SerializeField] private EventReference matchEventReference;
        [SerializeField] private EventReference powerGemEventReference;
        [SerializeField] private EventReference gemExplosionEventReference;
        [SerializeField] private EventReference gemElectricShockEventReference;
        [SerializeField] private EventReference gemClinkEventReference;
        [SerializeField] private EventReference gemSwapEventReference;

        private Dictionary<MatchSFXType, EventReference> gemSounds;

        private void Awake()
        {
            gemSounds = new Dictionary<MatchSFXType, EventReference>()
            {
                { MatchSFXType.NORMAL, matchEventReference },
                { MatchSFXType.POWERUP, powerGemEventReference },
                { MatchSFXType.BOMB, gemExplosionEventReference },
                { MatchSFXType.ELECTRIC, gemElectricShockEventReference }
            };
        }

        #region Events
        public void GemCell_OnClicked(object sender, EventArgs eventArgs) => PlaySFXForCellSelect(sender);
        public void MatchFinder_OnPlayMatchSFX(object sender, SFXEventArgs eventArgs) => PlaySFXForMatches(eventArgs);
        public void GemMover_OnGemMoved(object sender, SFXEventArgs eventArgs) => PlaySFXForGemClink(eventArgs);
        public void GemMover_OnSwap(object sender, SFXEventArgs eventArgs) => PlaySFXForGemSwap(eventArgs);
        public void GridRetiler_OnGemMovedToPosition(object sender, SFXEventArgs eventArgs) => PlaySFXForGemClink(eventArgs);
        #endregion

        #region EventMethods

        private IEnumerator PlaySFX(EventInstance sfxInstance, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            sfxInstance.start();
            sfxInstance.release();
        }

        private void PlaySFXForCellSelect(object sender)
        {
            GemCell clickedCell = sender as GemCell;
            EventInstance sfxInstance = RuntimeManager.CreateInstance(gemCellSelectedEventReference);

            sfxInstance.setParameterByName("Selected", (clickedCell == GemCell.Selected) ? 1 : 0);
            StartCoroutine(PlaySFX(sfxInstance, 0));
        }

        private void PlaySFXForGemClink(SFXEventArgs eventArgs)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(gemClinkEventReference);
            StartCoroutine(PlaySFX(eventInstance, eventArgs.startDelaySeconds));
        }

        private void PlaySFXForGemSwap(SFXEventArgs eventArgs)
        {
            EventInstance sfxInstance = RuntimeManager.CreateInstance(gemSwapEventReference);
            sfxInstance.setParameterByName("SwapState", eventArgs.swapState);

            StartCoroutine(PlaySFX(sfxInstance, eventArgs.startDelaySeconds));
        }

        private void PlaySFXForMatches(SFXEventArgs eventArgs)
        {
            EventInstance sfxInstance = RuntimeManager.CreateInstance(gemSounds[eventArgs.sfxType]);
            if (eventArgs.sfxType == MatchSFXType.NORMAL) sfxInstance.setParameterByName("SequenceNumber", eventArgs.matchCount);

            StartCoroutine(PlaySFX(sfxInstance, 0f));
        }
        #endregion
    }
}

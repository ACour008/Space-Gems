using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiiskoWiiyaas.Core.Events;
using MiiskoWiiyaas.Scoring;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIAnimator chainBonusAnimator;
    [SerializeField] UIAnimator multiBonusAnimator;
    [SerializeField] UIAnimator cellScoreAnimator;

    private Dictionary<BonusType, UIAnimator> bonusUIAnimators;

    private void Start()
    {
        bonusUIAnimators = new Dictionary<BonusType, UIAnimator>()
        {
            {BonusType.CHAIN, chainBonusAnimator },
            {BonusType.MULTI, multiBonusAnimator }
        };
    }

    public void ScoreManager_OnBonus(object sender, BonusEventArgs eventArgs)
    {
        IUIAnimatedComponent uiComponent = bonusUIAnimators[eventArgs.bonusType].GetUIAnimatedComponent();
        string scoreValue = eventArgs.score.ToString();

        uiComponent.Run(chainBonusAnimator.transform, scoreValue);
    }

    public void MatchFinder_OnMatchProcessed(object sender, MatchEventArgs eventArgs)
    {
        string scoreValue = (eventArgs.matches[0].Value * eventArgs.matches.Count).ToString();
        Transform parent = eventArgs.scoreCell.transform;
        IUIAnimatedComponent uiComponent = cellScoreAnimator.GetUIAnimatedComponent();

        uiComponent.Run(parent, scoreValue);
    }


}

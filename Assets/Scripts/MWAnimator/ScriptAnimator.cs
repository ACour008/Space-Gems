using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MiskoWiiyaas.Interfaces;

namespace MiskoWiiyaas.MWAnimator
{
    public class ScriptAnimator: MWAnimator, IStartAnimationHandler
    {
        public void OnBeginStartAnimation()
        {
            foreach(MWAnimation anim in animations)
            {
                StartCoroutine(anim.DoAnimation());
            }
        }

        private void Start()
        {
            animations = GetComponents<MWAnimation>();

            if (beginOnStart)
            {
                OnBeginStartAnimation();
            }
        }
    }
}
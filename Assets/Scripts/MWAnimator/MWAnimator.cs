using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiskoWiiyaas.MWAnimator
{
    public abstract class MWAnimator : MonoBehaviour
    {
        protected MWAnimation[] animations;
        [SerializeField] protected bool beginOnStart = false;
    }
}

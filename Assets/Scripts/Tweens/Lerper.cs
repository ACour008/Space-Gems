using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweens
{
    public abstract class Lerper<T1>: ILerp
    {
        protected Type _lerpType = typeof(T1);
        protected Getter<T1> _getter;
        protected Setter<T1> _setter;
        protected T1 _startValue;
        protected T1 _endValue;
        protected float _durationInSecs;
        protected float _timeElapsed = 0;
        protected bool _isComplete;
        protected YieldInstruction _wait;

        public Type LerpType { get => _lerpType; }
        public bool IsComplete { get => _isComplete; }

        public Lerper<T1> Init(Getter<T1> getter, Setter<T1> setter, T1 endValue, float durationInSecs, float startDelayInSecs)
        {
            _getter = getter;
            _setter = setter;
            _startValue = _getter();
            _endValue = endValue;
            _durationInSecs = durationInSecs;
            _wait = new WaitForSeconds(startDelayInSecs);

            return this;

        }

        public abstract IEnumerator StartLerping();
    }

}


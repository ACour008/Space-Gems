using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweens
{
    public class ColorLerper : Lerper<Color>
    {

        // add to Application Update in Effect.
        public override IEnumerator StartLerping()
        {
            yield return _wait;

            float duration = _durationInSecs - 0.01f;

            _isComplete = false;

            while (_timeElapsed < duration)
            {
                float complete = _timeElapsed / duration;

                _setter(
                    new Color(
                        _startValue.r,
                        _startValue.g,
                        _startValue.b,
                        _startValue.a + (_endValue.a - _startValue.a) * complete
                    )
                );

                _timeElapsed += Time.deltaTime;
                yield return null;

            }

            _setter(_endValue);
            _isComplete = true;
        }
    }

}
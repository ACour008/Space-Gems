using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tweens
{
    public delegate T1 Getter<T1>();
    public delegate void Setter<T1>(T1 newValue);
}

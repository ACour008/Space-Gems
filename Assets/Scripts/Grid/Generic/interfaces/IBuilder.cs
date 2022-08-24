using MiiskoWiiyaas.Core;
using UnityEngine;

public interface IBuilder<T1, T2>
{
    public T1 Build(int id, float xPosition, float yPosition, Transform parent, T2[] cells);

    public T1 BuildFromLayout(int id, float xPosition, float yPosition, Transform parent, T2[] cells, GemColor[] layout);
}

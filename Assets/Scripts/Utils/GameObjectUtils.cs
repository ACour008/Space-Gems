using System.Collections;
using UnityEngine;

namespace MiiskoWiiyaas.Utils
{
    public static class GameObjectUtils
    {
        public static IEnumerator DestroyAfterSeconds(GameObject gameObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameObject.Destroy(gameObject);
        }
    }
}

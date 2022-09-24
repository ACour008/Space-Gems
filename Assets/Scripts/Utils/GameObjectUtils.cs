using System.Collections;
using UnityEngine;

namespace MiiskoWiiyaas.Utils
{
    public static class GameObjectUtils
    {
        /// <summary>
        /// Destroys the given GameObject after a specified number of seconds.
        /// </summary>
        /// <param name="gameObject">The GameObject to be destroyed.</param>
        /// <param name="seconds">The delay in seconds before the GameObject is destroyed.</param>
        /// <returns></returns>
        public static IEnumerator DestroyAfterSeconds(GameObject gameObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameObject.Destroy(gameObject);
        }
    }
}

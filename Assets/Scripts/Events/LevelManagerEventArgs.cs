using System;

namespace MiiskoWiiyaas.Core.Events
{
    /// <summary>
    /// Holds data as to whether a restart has occured.
    /// </summary>
    public class LevelManagerEventArgs : EventArgs
    {
        public bool isALevelRestart;
    }

}
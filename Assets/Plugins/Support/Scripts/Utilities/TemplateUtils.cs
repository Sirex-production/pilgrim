using UnityEngine;

namespace Support
{
    /// <summary>
    /// Class that holds static utility methods 
    /// </summary>
    public static class TemplateUtils
    {
        /// <summary>
        /// Prints log to unity console. During build runtime suppresses all of the messages thereby increases performance
        /// </summary>
        /// <param name="objectContent">Object that will be logged</param>
        /// <param name="logType">Type of log in the console</param>
        public static void SafeDebug(object objectContent, LogType logType = LogType.Log, Object context = null)
        {
#if UNITY_EDITOR
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(objectContent, context);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(objectContent, context);
                    break;
                default:
                    Debug.LogError(objectContent, context);
                    break;
            }
#endif
        }
    }
}
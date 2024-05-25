using UnityEngine;

namespace Utility
{
    public static class CustomDebug
    {
        public static void Log(string text)
        {
            Debug.Log(string.Format("<color=white>[Log]</color>    {0}", text));
        }
        public static void WarningLog(string text)
        {
            Debug.Log(string.Format("<color=yellow>[Warning]</color>    {0}", text));
        }
        public static void ErrorLog(string text)
        {
            Debug.Log(string.Format("<color=red>[Error]</color>    {0}", text));
        }
    }
}

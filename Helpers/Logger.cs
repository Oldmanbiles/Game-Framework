using System;
using UnityEngine;

namespace GameFramework
{
    public static class Logger
    {
        public static void Log(object log, Type param = null, LogType type = LogType.Log)
        {
#if LOGGING
        log = string.Format(log + " " + GetTypeName(param));

        switch (type)
        {
            case LogType.Error:
                Debug.LogError(log);
                break;
            case LogType.Assert:
                Debug.LogAssertion(log);
                break;
            case LogType.Warning:
                Debug.LogWarning(log);
                break;
            case LogType.Log:
                Debug.Log(log);
                break;
            case LogType.Exception:
                Debug.LogError(log);
                break;
        }
#endif
        }
    
        public static void Log<T>(object log, Type param = null, LogType type = LogType.Log)
        {
#if LOGGING
            var context = GetTypeName(typeof(T));
            log = string.Format("[" + context + "] "  + log + GetTypeName(param));

            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(log);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(log);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(log);
                    break;
                case LogType.Log:
                    Debug.Log(log);
                    break;
                case LogType.Exception:
                    Debug.LogError(log);
                    break;
            }
#endif
        }
    
        public static void LogWarning<T>(object log, Type param = null)
        {
            Log<T>(log, param, LogType.Warning);
        }
    
        public static void LogError<T>(object log, Type param = null)
        {
            Log<T>(log, param, LogType.Error);
        }

        public static void LogWarning(object log, Type param = null)
        {
            Log(log, param, LogType.Warning);
        }

        public static void LogException<T>(Exception e)
        {
#if LOGGING
            var context = GetTypeName(typeof(T));
            Exception newExc = new Exception(context, e);
            Debug.LogException(e);
#endif
        }
        
        public static void LogException(Exception e)
        {
#if LOGGING
            Debug.LogException(e);
#endif
        }
    
        public static void LogError(object log, Type param = null)
        {
            Log(log, param, LogType.Error);
        }
    
        private static string GetTypeName(Type type)
        {
            if (type == null)
                return string.Empty;

            var split = type.ToString().Split(Period);
            var last = split.Length - 1;
            return last > 0 ? split[last] : string.Empty;
        }

        const char Period = '.';
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomShaderHandler
{
    public static class Logging
    {
        public static string Normal(object message)
        {
            string m = string.Concat("[CSM] " + message);
            Debug.Log(m);
            return m;
        }

        public static string Warning(object message)
        {
            string m = string.Concat("[CSM] " + message);
            Debug.LogWarning(m);
            return m;
        }

        public static string Error(object message)
        {
            string m = string.Concat("[CSM] " + message);
            Debug.LogError(m);
            return m;
        }
    }
}

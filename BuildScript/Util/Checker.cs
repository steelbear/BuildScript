using System;

namespace BuildScript.Util
{
    internal static class Checker
    {
        public static void CheckNull(object obj, string message = "")
        {
            if (obj == null) throw new NullReferenceException(message);
        }
    }
}

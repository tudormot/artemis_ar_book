using System;
using System.Collections;
using UnityEngine;

namespace BookAR.Scripts.Utils.Coroutines
{
    public static class ExecutePeriodically
    {
        public static IEnumerator executePeriodically(float seconds, Action fun)
        {
            while (true)
            {
                fun.Invoke();
                yield return new WaitForSeconds(seconds);
            }

        }
    }
}
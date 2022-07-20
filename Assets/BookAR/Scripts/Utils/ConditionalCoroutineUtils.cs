using System;
using System.Collections;
using UnityEngine;

namespace Scenes.BookAR.Scripts.Utils
{
    internal static class ConditionalCoroutineUtils
    {
        public static IEnumerator ConditionalExecutionCoroutine(Func<bool> conditional, Action executable, int timeout = WaitForCondition.DEFAULT_TIMEOUT_IN_FRAMES)
        {
            yield return new WaitForCondition(conditional, timeout);
            executable.Invoke();
        }

    }
    public class WaitForCondition : CustomYieldInstruction
    {
        private Func<bool> conditional;
        public override bool keepWaiting => conditional.Invoke();
        public const int DEFAULT_TIMEOUT_IN_FRAMES = 60*60*3;
        private int timeout;
        private int currentFrame = 0;
        public WaitForCondition(Func<bool> conditional, int timeout = DEFAULT_TIMEOUT_IN_FRAMES)
        {
            this.timeout = timeout;
            this.conditional = ()=>
            {
                currentFrame += 1;
                if (currentFrame >= this.timeout)
                {
                    Debug.LogError("Conditional did not become true after timeout!! A coroutine is  not accomplishing its purpose, care!!");
                    return false;
                }

                return !conditional.Invoke();
            };
        }
    }
}
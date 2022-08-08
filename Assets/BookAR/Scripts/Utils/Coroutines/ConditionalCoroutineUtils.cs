using System;
using System.Collections;
using UnityEngine;

namespace BookAR.Scripts.Utils.Coroutines
{
    public static class ConditionalCoroutineUtils
    {
        public static IEnumerator ConditionalExecutionCoroutine(Func<bool> conditional, Action executable, int timeout = WaitForConditionWithTimeout.DEFAULT_TIMEOUT_IN_FRAMES)
        {
            yield return new WaitForConditionWithTimeout(conditional, timeout);
            executable.Invoke();
        }

    }
    public class WaitForConditionWithTimeout : CustomYieldInstruction
    {
        private Func<bool> conditional;
        public override bool keepWaiting => conditional.Invoke();
        public const int DEFAULT_TIMEOUT_IN_FRAMES = 60*60*3;
        private int timeout;
        private int currentFrame = 0;
        public WaitForConditionWithTimeout(Func<bool> nonTimeoutCondition, int timeout = DEFAULT_TIMEOUT_IN_FRAMES)
        {
            this.timeout = timeout;
            this.conditional = ()=>
            {
                currentFrame += 1;
                if (currentFrame >= this.timeout)
                {
                    UnityEngine.Debug.LogError("Conditional did not become true after timeout!! A coroutine is  not accomplishing its purpose, care!!");
                    return false;
                }
                return !nonTimeoutCondition.Invoke();
            };
        }
    }
}
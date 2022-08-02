using System;
using System.Collections;
using UnityEngine;

namespace BookAR.Scripts.Utils.Coroutines
{
    public class EventDebouncingHelper<T> where T:MonoBehaviour
    {
        public EventDebouncingHelper(Action<T> callbackToDebounce, T context)
        {
            this.callbackToDebounce = callbackToDebounce;
            this.context = context;
        }

        private T context;
        private Action<T> callbackToDebounce;
        private const float DEBOUNCE_PERIOD = 0.5f; //in seconds
        private bool isDuringDebouncePeriod = false;

        
        private IEnumerator debounceCooldown(float cooldownSeconds)
        {
            isDuringDebouncePeriod = true;
            yield return new WaitForSeconds(cooldownSeconds);
            isDuringDebouncePeriod = false;

        }
        public void executeCallback()
        {
            if (!isDuringDebouncePeriod)
            {
                callbackToDebounce.Invoke(this.context);
                context.StartCoroutine(debounceCooldown(DEBOUNCE_PERIOD));
            }
        }
    }
}
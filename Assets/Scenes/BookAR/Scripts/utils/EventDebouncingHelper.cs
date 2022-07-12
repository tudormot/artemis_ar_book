using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

namespace UnityEngine
{
    public class EventDebouncingHelper<T> where T:MonoBehaviour
    {
        public EventDebouncingHelper(Action<T> callbackToDebounce, MonoBehaviour context)
        {
            this.callbackToDebounce = callbackToDebounce;
            this.context = context;
        }

        private MonoBehaviour context;
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
                callbackToDebounce.Invoke();
                context.StartCoroutine(debounceCooldown(DEBOUNCE_PERIOD));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using BookAR.Scripts.Global;
using BookAR.Scripts.Utils.Coroutines;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DebugMessagesManager : MonoBehaviour
    {
        private TextMeshProUGUI textView;
        private Coroutine coroutine;
        private void OnEnable()
        {
            textView = GetComponent<TextMeshProUGUI>();
            if (textView == null)
            {
                Debug.LogError("DebugTextview is null! no good!");
            }

            transform.GetComponent<TextMeshPro>();
            
            GlobalSettingsSingleton.instance.GlobalSettingsChanged += onSettingsChanged;
            textView.enabled = GlobalSettingsSingleton.instance.state.enableOnScreenDebugMessages;

            imageManager = FindObjectOfType<ARTrackedImageManager>();
            coroutine = StartCoroutine(
                ExecutePeriodically.executePeriodically(
                    seconds:1,
                    fun:writeDebugMessages
                )
            );
        }
        private void OnDisable()
        {
            GlobalSettingsSingleton.instance.GlobalSettingsChanged -= onSettingsChanged;
            StopCoroutine(coroutine);
        }

        private ARTrackedImageManager imageManager;
        private void writeDebugMessages()
        {
            var messages = new List<string>();
            messages.Add("Debug messages:\n");
            foreach (var im in imageManager.trackables)
            {
                messages.Add(
              //      $"Im size: {im.size}, ratio: {im.size.x / im.size.y}\n"
                    $"Im detection state: {im.trackingState}, {im.name}"
                    );
                
            }
            textView.text = string.Concat(messages);
        }

        private void onSettingsChanged(object obj, GlobalSettingsEventData data )
        {
            Debug.Log("in DebugMessageSManager, onSettingsChanged called!");
            textView.enabled = data.newState.enableOnScreenDebugMessages;
        }
    }
}
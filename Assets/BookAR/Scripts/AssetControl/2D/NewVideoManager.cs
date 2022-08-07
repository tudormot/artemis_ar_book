using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;

public class NewVideoManager : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]private Button revealAssetButton;

    [SerializeField]private VideoPlayer player;
    [SerializeField]private Canvas videoPlayerCanvas;
    [SerializeField]private Canvas buttonsTabCanvas;

    [SerializeField]private Image progress;
    [SerializeField]private Button playButton;
    [SerializeField]private Button pauseButton;
    [SerializeField]private Button fullScreenButton;
    [SerializeField]private Button smallScreenButton;

    
    
    void Update()
    {
        if (player.frameCount > 0)
            progress.fillAmount = (float)player.frame / (float)player.frameCount;
    }

    private void OnEnable()
    {
        buttonsTabCanvas.gameObject.SetActive(false);
        revealAssetButton.gameObject.SetActive(true);
        videoPlayerCanvas.gameObject.SetActive(true);
        player.gameObject.SetActive(false);

        
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        fullScreenButton.gameObject.SetActive(true);
        smallScreenButton.gameObject.SetActive(false);
        
        revealAssetButton.onClick.AddListener(
            () =>
            {
                revealAssetButton.gameObject.SetActive(false);
                buttonsTabCanvas.gameObject.SetActive(true);
                videoPlayerCanvas.gameObject.SetActive(true);
                player.gameObject.SetActive(true);
                
            }
        );
        
        playButton.onClick.AddListener(
            () =>
            {
                playButton.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(true);
                player.Play();
            }
        );
        pauseButton.onClick.AddListener(
            () =>
            {
                playButton.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(false);
                player.Pause();
            }
        );
        fullScreenButton.onClick.AddListener(
            () =>
            {
                fullScreenButton.gameObject.SetActive(false);
                smallScreenButton.gameObject.SetActive(true);
                videoPlayerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        );
        smallScreenButton.onClick.AddListener(
            () =>
            {
                fullScreenButton.gameObject.SetActive(true);
                smallScreenButton.gameObject.SetActive(false);
                videoPlayerCanvas.renderMode = RenderMode.WorldSpace;
                videoPlayerCanvas.worldCamera = Camera.main;
            }
        );
        
    }

    private void OnDisable()
    {
        fullScreenButton.onClick.RemoveAllListeners();
        smallScreenButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }
    private void SkipToPercent(float pct)
    {
        var frame = player.frameCount * pct;
        player.frame = (long)frame;
    }
    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progress.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(pct);
        }
    }

  
}


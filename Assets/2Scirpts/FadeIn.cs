using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image image;
    private float startOpacity;
    private float endOpacity;
    private float playTime = 0f;
    private float fadeTime = 0f;
    private bool isPlaying = false;
    private Action callback;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    IEnumerator FadeInAnimation()
    {
        while (true)
        {
            playTime += .05f / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(startOpacity, endOpacity, playTime);
            image.color = color;

            if (playTime >= fadeTime)
            {
                isPlaying = false;

                color.a = endOpacity;
                image.color = color;

                if (endOpacity == 0f)
                    gameObject.SetActive(false);

                if (callback != null)
                    callback?.Invoke();

                break;
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    public void StartFadeIn(float startOpacity, float endOpacity, float fadeTime, Action callback=null)
    {
        if (isPlaying) return;

        gameObject.SetActive(true);

        this.startOpacity = startOpacity;
        this.endOpacity = endOpacity;
        this.fadeTime = fadeTime;
        this.callback = callback;

        playTime = 0f;
        isPlaying = true;

        StartCoroutine(FadeInAnimation());
    }
}

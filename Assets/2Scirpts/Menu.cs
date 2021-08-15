using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject optionModal;
    public FadeIn fadeIn;

    void Start()
    {
        fadeIn = GetComponent<FadeIn>();
    }

    public void Open()
    {
        fadeIn.StartFadeIn(0f, .9f, .5f, OnMenuOpened);
    }

    public void Resume()
    {
        fadeIn.StartFadeIn(.9f, 0f, .2f);
        Time.timeScale = 1f;
    }

    private void OnMenuOpened()
    {
        Time.timeScale = 0f;
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnResumeButtonClicked()
    {
        if (fadeIn == null) return;
        Resume();
    }

    public void OnGalleryButtonClicked()
    {

    }

    public void OnOptionButtonClicked()
    {
        optionModal.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void OnQuitButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}

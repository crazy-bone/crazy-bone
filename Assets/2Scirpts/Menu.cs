using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject optionModal;
    public GameObject instructionModal;
    public FadeIn fader;

    public void Open()
    {
        if (instructionModal.activeSelf)
            return;

        fader.StartFadeIn(0f, .9f, .5f, OnMenuOpened);
    }

    public void Resume()
    {
        fader.StartFadeIn(.9f, 0f, .2f, OnMenuClosed);
        Time.timeScale = 1f;
    }

    public void Toggle()
    {
        if (gameObject.activeSelf)
            Resume();
        else
            Open();
    }

    private void OnMenuOpened()
    {
        Time.timeScale = 0f;
    }

    private void OnMenuClosed()
    {
        optionModal.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        if (fader != null)
            fader.StartFadeIn(0f, 1f, 1f, () => SceneManager.LoadScene("SampleScene") );
        else
            SceneManager.LoadScene("SampleScene");
    }

    public void OnResumeButtonClicked()
    {
        if (fader == null) return;
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

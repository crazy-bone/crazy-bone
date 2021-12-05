using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cutscene : MonoBehaviour
{
    public GameObject[] dialogueBoxes;
    public TextMeshProUGUI[] textMeshes;

    private Action callback;

    public bool running = false;
    private int currentIndex = 0;
    private List<Dictionary<string, object>> script;

    void Update()
    {
        if (running)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                ShowNext();
        }
    }

    public void Run(string dialogueName, Action callback=null)
    {
        this.callback = callback;

        script = CSVReader.Read(dialogueName);
        running = true;

        Time.timeScale = 0f; // 게임 정지

        Show(0);
    }

    public void ShowNext()
    {
        if (currentIndex == script.Count - 1)
        {
            Time.timeScale = 1f; // 게임 재개

            running = false;
            foreach (GameObject dialogueBox in dialogueBoxes)
                dialogueBox.SetActive(false);

            if (callback != null)
                callback?.Invoke();
        }
        else
            Show(currentIndex + 1);
    }

    private void Show(int index)
    {
        if (!running)
            return;

        currentIndex = index;
        Dictionary<string, object> row = script[index];

        int ui = (int)row["UI"];
        string speaker = (string)row["Speaker"];
        string dialog = (string)row["Dialog"];
        /*float cameraSpeed = (float)row["CameraSpeed"];
        float cameraPositionX = (float)row["CameraPositionX"];
        float cameraPositionY = (float)row["CameraPositionY"];
        float cameraPositionZ = (float)row["CameraPositionZ"];
        float cameraRotationX = (float)row["CameraRotationX"];
        float cameraRotationY = (float)row["CameraRotationY"];
        float cameraRotationZ = (float)row["CameraRotationZ"];*/

        foreach (GameObject dialogueBox in dialogueBoxes)
            dialogueBox.SetActive(false);
        dialogueBoxes[ui].SetActive(true);

        textMeshes[ui].SetText(dialog);
    }
}

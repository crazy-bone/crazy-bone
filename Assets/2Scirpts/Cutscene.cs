using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cutscene : MonoBehaviour
{
    public GameObject[] dialogueBoxes;
    public TextMeshProUGUI[] textMeshes;

    private bool running = false;
    private int currentIndex = 0;
    private List<Dictionary<string, object>> script;

    void Start()
    {
        textMeshes = new TextMeshProUGUI[dialogueBoxes.Length];
        for (int i = 0; i < dialogueBoxes.Length; i++)
            textMeshes[i] = dialogueBoxes[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void Run(string dialogueFile)
    {
        script = CSVReader.Read(dialogueFile);
        running = true;
        Show(0);
    }

    public void ShowNext()
    {
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

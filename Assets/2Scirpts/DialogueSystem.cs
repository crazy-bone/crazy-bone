using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text txtName;
    public Text txtSentence;
    public GameObject Target;
    DialogueTrigger DialogueTrigger;
    bool isTak = false;

    Queue<string> sentences = new Queue<string>();
    public void Begin(Dialogue info)
    {
        sentences.Clear();
        Target.SetActive(true);
        txtName.text = info.name;

        foreach(var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }
        Next(); 
    }

    public void Next()
    {
        if(sentences.Count == 0)
        {
            End();
            return;
        }
        txtSentence.text = sentences.Dequeue();
          
    }
    private void End()
    {
        txtSentence.text = string.Empty;
        //DialogueTrigger.GetComponent<DialogueTrigger>().ChangeTrigger();
        
        Target.SetActive(false);
        if (isTak == false)
        {
            isTak = true;
            var vlieageEnter = FindObjectOfType<DialogueRewardTrigger>();
            vlieageEnter.ChangeTrigger();
        }
    }
}

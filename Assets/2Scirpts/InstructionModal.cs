using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionModal : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

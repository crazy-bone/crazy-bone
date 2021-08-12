using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionModal : MonoBehaviour
{
    public void OnApplyButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
    }
}

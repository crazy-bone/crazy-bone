using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Image bar;

    public void UpdateHealthBar(float health, float maxHealth)
    {
        float ratio = health / maxHealth;

        if (ratio < 0f)
            ratio = 0f;
        else if (ratio > 1f)
            ratio = 1f;

        bar.fillAmount = ratio;
    }
}

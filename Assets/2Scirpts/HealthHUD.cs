using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Image bar;

    public void UpdateHealthBar(float health, float maxHealth)
    {
        bar.fillAmount = health / maxHealth;
    }
}

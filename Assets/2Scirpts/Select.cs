using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    public GameObject optionModal;
    public GameObject instructionModal;
    public FadeIn fader;
    public GameObject weapon;
    public GameObject bullet;
    public GameObject player;



    public void YonBalButtonClicked()
    {
        weapon.GetComponent<Weapon>().ChangeYonBal();
        gameObject.SetActive(false);

    }

    public void BomWeButtonClicked()
    {
        weapon.GetComponent<Weapon>().ChangeBomWe();
        gameObject.SetActive(false);
    }

    public void CheRukButtonClicked()
    {
        player.GetComponent<Player>().CheRukUp();
        gameObject.SetActive(false);
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

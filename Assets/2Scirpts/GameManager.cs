using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public Menu menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text A;
    public Text B;
    public Text C;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            menuPanel.Toggle();
    }

    void LateUpdate()
    {

        A.text = "플레이어 체력 " + player.health + " / " + player.maxHealth;
        B.text = string.Format("플레이어 코인 {0} / {1}", player.coin, player.maxCoin);
        C.text = string.Format("장전 수 {0} / {1}", player.ammo, player.maxAmmo);

    }

    void Start()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int bulletLayer = LayerMask.NameToLayer("EnemyBullet");
        Physics.IgnoreLayerCollision(enemyLayer, bulletLayer, true);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
    }
}

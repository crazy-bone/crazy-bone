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
    public GameObject retryPanel;
    public GameObject ClearPanel;
    public GameObject skyBox;
    public GameObject RedBox;
    public GameObject BlueBox;
    public Cutscene cutscene;
    public InstructionModal instructionModal;
    public FadeIn fader;
    public Text A;
    public Text B;
    public Text C;

    public  Text TalkText;
    public GameObject scanObject;
    public GameObject[] Fire;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        TalkText.text = "이것의 이름은 " + scanObject.name + "이라고 한다." ;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (instructionModal.gameObject.activeSelf)
            {
                instructionModal.Close();
                RedBox.SetActive(true);
            }
            else
                menuPanel.Toggle();

        /*if (Input.GetKeyDown(KeyCode.Mouse0))
            if (RedBox.activeSelf)
            {
                RedBox.SetActive(false);
                Time.timeScale = 1f;
            }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            if (BlueBox.activeSelf)
            {
                BlueBox.SetActive(false);
            }*/
        if(Boss.attackPhase == 0)
        {
            Fire[0].gameObject.SetActive(true);
            Fire[1].gameObject.SetActive(true);
            Fire[2].gameObject.SetActive(true);
            Fire[3].gameObject.SetActive(true);
            Fire[4].gameObject.SetActive(true);
            Fire[5].gameObject.SetActive(true);
        }
        if (Boss.attackPhase == 1)
        {
            Fire[0].gameObject.SetActive(false);
            Fire[1].gameObject.SetActive(false);
            Fire[2].gameObject.SetActive(true);
            Fire[3].gameObject.SetActive(true);
            Fire[4].gameObject.SetActive(true);
            Fire[5].gameObject.SetActive(true);
        }
        if (Boss.attackPhase == 2)
        {
            Fire[0].gameObject.SetActive(false);
            Fire[1].gameObject.SetActive(false);
            Fire[2].gameObject.SetActive(false);
            Fire[3].gameObject.SetActive(true);
            Fire[4].gameObject.SetActive(true);
            Fire[5].gameObject.SetActive(true);
        }
        if (Boss.attackPhase == 3)
        {
            Fire[0].gameObject.SetActive(false);
            Fire[1].gameObject.SetActive(false);
            Fire[2].gameObject.SetActive(false);
            Fire[3].gameObject.SetActive(false);
            Fire[4].gameObject.SetActive(true);
            Fire[5].gameObject.SetActive(true);
        }
        if (Boss.attackPhase == 4)
        {
            Fire[0].gameObject.SetActive(false);
            Fire[1].gameObject.SetActive(false);
            Fire[2].gameObject.SetActive(false);
            Fire[3].gameObject.SetActive(false);
            Fire[4].gameObject.SetActive(false);
            Fire[5].gameObject.SetActive(true);
        }



    }

    void LateUpdate()
    {
        // Player 상태를 보여줌
        A.text = "플레이어 체력 " + player.health + " / " + player.maxHealth;
        B.text = string.Format("플레이어 코인 {0} / {1}", player.coin, player.maxCoin);
        C.text = string.Format("장전 수 {0} / {1}", player.ammo, player.maxAmmo);

    }

    void Start()
    {
        // Enemy와 EnemyBullet의 충돌자체를 막아줌
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int bulletLayer = LayerMask.NameToLayer("EnemyBullet");
        Physics.IgnoreLayerCollision(enemyLayer, bulletLayer, true);

        fader.StartFadeIn(1f, 0f, .75f, () => instructionModal.gameObject.SetActive(true));

        // 컷신 실행
        cutscene.Run("StartCutscene");
    }

    public void GameOver()
    {
        // 게임 오버시에 Panel을 보여줌
        gamePanel.SetActive(false);
        retryPanel.SetActive(true);
        skyBox.SetActive(true);
    }

    public void GameClear()
    {
        // 게임 클리어시에 Panel을 보여줌
        retryPanel.SetActive(true);
        ClearPanel.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject missile;
    public GameObject Awl;
    public SubBoss SubBossTemplate;
    public Transform missilePort;
    public Transform missilePortA;
    public Transform missilePortB;
    public Transform missilePortC;
    public Transform missilePortD;
    public Transform missilePortE;
    public Transform AwlPort;
    public Transform AwlPortA;
    public Transform AwlPortB;
    public Transform AwlPortC;
    public Transform[] summonPositions;
    public int attackPhase = 0; // 전투 페이즈
    // Start is called before the first frame update
    
    Vector3 lookVec;
    Vector3 tauntVec;
    bool isLook;
    SubBoss[] summonedSubBosses;


    void Awake()
    {
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }

        // 전투 페이즈 갱신
        switch (attackPhase)
        {
            case 0: // 페이즈 A
                if ((float)curHealth/maxHealth <= 3f/3f) // 보스 체력 2/3 이하
                {
                    SummonSubBosses();
                    attackPhase++;
                }
                break;
        }
    }

    void SummonSubBosses()
    {
        int i = 0;

        summonedSubBosses = new SubBoss[summonPositions.Length];
        foreach (Transform position in summonPositions)
        {
            if (position == null)
                continue;

            SubBoss summoned = Instantiate<SubBoss>(SubBossTemplate, position.position, SubBossTemplate.transform.rotation);
            summoned.gameObject.SetActive(true);
            summonedSubBosses[i++] = summoned;
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(AwlAttack());
                StartCoroutine(MissileShot());
                break;

            case 2:
            case 3:
                StartCoroutine(MissileShot2());
                break;
            case 4:
                StartCoroutine(MissileShot3());
                break;
        }
    }
    IEnumerator MissileShot()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject instantMissile = Instantiate(missile, missilePort.position, missilePort.rotation);
        BossMissile bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;

        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePort.position, missilePort.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePort.position, missilePort.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePort.position, missilePort.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePort.position, missilePort.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;

        StartCoroutine(Think());
    }
    IEnumerator MissileShot2()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject instantMissile = Instantiate(missile, missilePortE.position, missilePortE.rotation);
        BossMissile bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;

        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePortC.position, missilePortC.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(1f);
        instantMissile = Instantiate(missile, missilePortD.position, missilePortD.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Think());
    }
    IEnumerator MissileShot3()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject instantMissile = Instantiate(missile, missilePortE.position, missilePortE.rotation);
        BossMissile bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;

        instantMissile = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        instantMissile = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        instantMissile = Instantiate(missile, missilePortC.position, missilePortC.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        instantMissile = Instantiate(missile, missilePortD.position, missilePortD.rotation);
        bossMissile = instantMissile.GetComponent<BossMissile>();
        bossMissile.target = target;
        StartCoroutine(Think());
    }
    IEnumerator AwlAttack()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject instantMissile = Instantiate(Awl, AwlPort.position, AwlPort.rotation);
        instantMissile.GetComponent<BossAwl>();

        instantMissile = Instantiate(Awl, AwlPortA.position, AwlPortA.rotation);

        instantMissile.GetComponent<BossAwl>();
        instantMissile = Instantiate(Awl, AwlPortB.position, AwlPortB.rotation);

        instantMissile.GetComponent<BossAwl>();
        instantMissile = Instantiate(Awl, AwlPortC.position, AwlPortC.rotation);

        instantMissile.GetComponent<BossAwl>();
    }
}

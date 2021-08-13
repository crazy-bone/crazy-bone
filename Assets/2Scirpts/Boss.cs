using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject missile;
    public GameObject Awl;
    public GameObject AwlDamage;
    public GameObject AwlDamage2;
    public SubBoss SubBossRangeTemplate;
    public SubBoss SubBossMeleeTemplate;
    public GameObject AwlNoti;
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
    public Transform AwlPortD;
    public Transform AwlPortE;
    public Transform AwlPortF;
    public Transform AwlPortG;
    public Transform AwlPortH;
    public Transform[] summonPositions;
    /// <summary> 공격 페이즈 </summary>
    public int attackPhase = 0;
    // Start is called before the first frame update
    Vector3 lookVec;
    Vector3 tauntVec;

    bool isDead = false;
    
    bool isLook = true;
    SubBoss[] summonedSubBosses;

    Animator anim;

    void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (isLook && isDead == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }

        // 공격 페이즈
        switch (attackPhase)
        {
            case 0: // 페이즈 A 전환
                if ((float)curHealth/maxHealth <= 2f/3f && (float)curHealth / maxHealth >= 0f) // 체력이 2/3 이하인 경우
                {
                    attackPhase = 1;
                    SummonRangeSubBosses();
                }
                attackPhase = 1; //TODO: 페이즈 적용 전까지만 임시로 페이즈 A 건너뜀
                break;

            case 1: // 페이즈 B 전환
                if ((float)curHealth/maxHealth <= 3f/3f && (float)curHealth / maxHealth >= 0f) // 체력이 1/2 이하인 경우
                {
                    attackPhase = 2;
                    SummonMeleeSubBosses();
                    
                }
                attackPhase = 2;
                break;

            case 2: 
                if((float)curHealth/maxHealth <= 0f)
                {
                    attackPhase = 3;
                    anim.SetTrigger("doDie");
                    isDead = true;
                    
                }
                break;
        }
    }

    private void SummonRangeSubBosses()
    {
        int i = 0;

        summonedSubBosses = new SubBoss[2];
        foreach (Transform position in summonPositions)
        {
            if (position == null)
                continue;

            SubBoss summoned = Instantiate<SubBoss>(SubBossRangeTemplate, position.position, SubBossRangeTemplate.transform.rotation);
            summoned.gameObject.SetActive(true);
            summonedSubBosses[i++] = summoned;
        }
    }

    private void SummonMeleeSubBosses()
    {
        int i = 0;

        summonedSubBosses = new SubBoss[4];
        foreach (Transform position in summonPositions)
        {
            if (position == null)
                continue;

            anim.SetTrigger("doSummon");
            SubBoss summoned = Instantiate<SubBoss>(SubBossMeleeTemplate, position.position, SubBossMeleeTemplate.transform.rotation);
            summoned.gameObject.SetActive(true);
            summonedSubBosses[i++] = summoned;
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(AwlAttack2());

        int ranAction = Random.Range(0, 5);
        /*switch (ranAction)
        {
            case 0:
                StartCoroutine(AwlAttack());
                break;
            case 1:
                StartCoroutine(AwlAttack2());
                break;
            case 2:
                StartCoroutine(MissileShot());
                break;
            case 3:
                StartCoroutine(MissileShot2());
                break;
            case 4:
                StartCoroutine(MissileShot3());
                break;
         }*/
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
        yield return new WaitForSeconds(1f);

        Instantiate(AwlNoti, AwlPort.position, AwlPort.rotation);
        Instantiate(AwlNoti, AwlPortA.position, AwlPortA.rotation);
        Instantiate(AwlNoti, AwlPortB.position, AwlPortB.rotation);
        Instantiate(AwlNoti, AwlPortC.position, AwlPortC.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(AwlNoti, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(Awl, AwlPort.position, AwlPort.rotation);
        Instantiate(AwlDamage, AwlPort.position, AwlPort.rotation);

        Instantiate(Awl, AwlPortA.position, AwlPortA.rotation);
        Instantiate(AwlDamage, AwlPortA.position, AwlPortA.rotation);

        Instantiate(Awl, AwlPortB.position, AwlPortB.rotation);
        Instantiate(AwlDamage, AwlPortB.position, AwlPortB.rotation);

        Instantiate(Awl, AwlPortC.position, AwlPortC.rotation);
        Instantiate(AwlDamage, AwlPortC.position, AwlPortC.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(AwlDamage2, AwlPort.position, AwlPort.rotation);
        Instantiate(AwlDamage2, AwlPortA.position, AwlPortA.rotation);
        Instantiate(AwlDamage2, AwlPortB.position, AwlPortB.rotation);
        Instantiate(AwlDamage2, AwlPortC.position, AwlPortC.rotation);


        Instantiate(Awl, AwlPortH.position, AwlPortH.rotation);
        Instantiate(AwlDamage, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(AwlDamage2, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());

        //GameObject instantMissile = Instantiate(Awl, AwlPort.position, AwlPort.rotation);
        //Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
        //missileRigid.AddForce(AwlPort.up * 1);
    }
    IEnumerator AwlAttack2()
    {
        yield return new WaitForSeconds(1f);

        Instantiate(AwlNoti, AwlPortD.position, AwlPortD.rotation);
        Instantiate(AwlNoti, AwlPortE.position, AwlPortE.rotation);
        Instantiate(AwlNoti, AwlPortF.position, AwlPortF.rotation);
        Instantiate(AwlNoti, AwlPortG.position, AwlPortG.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(AwlNoti, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(Awl, AwlPortD.position, AwlPortD.rotation);
        Instantiate(AwlDamage, AwlPortD.position, AwlPortD.rotation);

        Instantiate(Awl, AwlPortE.position, AwlPortE.rotation);
        Instantiate(AwlDamage, AwlPortE.position, AwlPortE.rotation);

        Instantiate(Awl, AwlPortF.position, AwlPortF.rotation);
        Instantiate(AwlDamage, AwlPortF.position, AwlPortF.rotation);

        Instantiate(Awl, AwlPortG.position, AwlPortG.rotation);
        Instantiate(AwlDamage, AwlPortG.position, AwlPortG.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(AwlDamage2, AwlPortD.position, AwlPortD.rotation);
        Instantiate(AwlDamage2, AwlPortE.position, AwlPortE.rotation);
        Instantiate(AwlDamage2, AwlPortF.position, AwlPortF.rotation);
        Instantiate(AwlDamage2, AwlPortG.position, AwlPortG.rotation);


        Instantiate(Awl, AwlPortH.position, AwlPortH.rotation);
        Instantiate(AwlDamage, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(AwlDamage2, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());

    }
}

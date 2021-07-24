using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject missile;
    public GameObject Awl;
    public SubBoss SubBossTemplate;
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
    public Transform AwlPortC
    public Transform AwlPortD;
    public Transform AwlPortE;
    public Transform AwlPortF;
    public Transform AwlPortG;
    public Transform AwlPortH;;
    public Transform[] summonPositions;
    public int attackPhase = 0; // ���� ������
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

        // ���� ������ ����
        switch (attackPhase)
        {
            case 0: // ������ A
                if ((float)curHealth/maxHealth <= 3f/3f) // ���� ü�� 2/3 ����
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
        yield return new WaitForSeconds(1f);

        Instantiate(AwlNoti, AwlPort.position, AwlPort.rotation);
        Instantiate(AwlNoti, AwlPortA.position, AwlPortA.rotation);
        Instantiate(AwlNoti, AwlPortB.position, AwlPortB.rotation);
        Instantiate(AwlNoti, AwlPortC.position, AwlPortC.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(AwlNoti, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(Awl, AwlPort.position, AwlPort.rotation);
        Instantiate(Awl, AwlPortA.position, AwlPortA.rotation);
        Instantiate(Awl, AwlPortB.position, AwlPortB.rotation);
        Instantiate(Awl, AwlPortC.position, AwlPortC.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(Awl, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());

        //GameObject instantMissile = Instantiate(Awl, AwlPort.position, AwlPort.rotation);
        //Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
        //missileRigid.AddForce(AwlPort.up * 1);
        // instantMissile = Instantiate(Awl, AwlPortA.position, AwlPortA.rotation);
        //missileRigid = instantMissile.GetComponent<Rigidbody>();
        //missileRigid.AddForce(AwlPort.up * 1);
        //instantMissile.GetComponent<BossAwl>();
    }
    IEnumerator AwlAttack2()
    {
        yield return new WaitForSeconds(1f);

        Instantiate(AwlNoti, AwlPortD.position, AwlPortD.rotation);
        Instantiate(AwlNoti, AwlPortE.position, AwlPortE.rotation);
        Instantiate(AwlNoti, AwlPortF.position, AwlPortF.rotation);
        Instantiate(AwlNoti, AwlPortG.position, AwlPort.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(AwlNoti, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        Instantiate(Awl, AwlPortD.position, AwlPort.rotation);
        Instantiate(Awl, AwlPortE.position, AwlPortA.rotation);
        Instantiate(Awl, AwlPortF.position, AwlPortB.rotation);
        Instantiate(Awl, AwlPortG.position, AwlPortC.rotation);
        yield return new WaitForSeconds(1f);
        Instantiate(Awl, AwlPortH.position, AwlPortH.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());

    }
}

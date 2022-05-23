using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range };
    public Type type;
    public bool damageEnabled;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet5;
    public GameObject bullet6;
    public GameObject bullet7;
    public GameObject bullet8;
    public GameObject bullet9;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public int YonBal = 1;
    public int BomWe = 1;



    private void Start()
    {
    }


    void Update()
    {
        if (rate > 0)
            rate -= Time.deltaTime;
    }

    public void Use()
    {
        // 무기의 타입에 맞게 공격하는 로직. 단, 아직 원거리 무기는 없음
        damageEnabled = true;
        rate = 0;
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }

        else if (type == Type.Range)
        {
            if (YonBal == 1 && BomWe ==1)
                StartCoroutine("ShotA");
            else if (YonBal == 2 && BomWe == 1)
                StartCoroutine("ShotB");
            else if (YonBal == 3 && BomWe == 1)
                StartCoroutine("ShotC"); 
            else if (YonBal == 1 && BomWe == 2)
                StartCoroutine("ShotD");
            else if (YonBal == 2 && BomWe == 2)
                StartCoroutine("ShotE");
            else if (YonBal == 3 && BomWe == 2)
                StartCoroutine("ShotF");
            else if (YonBal == 1 && BomWe == 3)
                StartCoroutine("ShotG");
            else if (YonBal == 2 && BomWe == 3)
                StartCoroutine("ShotH");
            else if (YonBal == 3 && BomWe == 3)
                StartCoroutine("ShotI");
        }
    }
   
    public void Unuse()
    {
        damageEnabled = false;
        if (type == Type.Melee)
        {
            meleeArea.enabled = false;
        }
    }

    public void ChangeYonBal()
    {
        YonBal++;
    }
    public void ChangeBomWe()
    {
        BomWe++;
    }

    IEnumerator Swing()
    {
        // 무기의 범위와 이펙트를 시간에 순서에 따라 배치
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }
    IEnumerator SwingB()
    {
        // 무기의 범위와 이펙트를 시간에 순서에 따라 배치
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator ShotA()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;
 
        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2,3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    IEnumerator ShotB()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet2, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    }

    IEnumerator ShotC()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet2, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet3, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    IEnumerator ShotD()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet4, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    IEnumerator ShotE()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet4, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet5, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    }
    IEnumerator ShotF()
    {
        //총알 발사
        GameObject instantBullet = Instantiate(bullet4, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet5, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        //총알 발사
        instantBullet = Instantiate(bullet6, bulletPos.position, bulletPos.rotation);
        bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 500;

        yield return null;
        //탄피 배출
        instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        CaseRigid = instantCase.GetComponent<Rigidbody>();
        caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

     IEnumerator ShotG()
        {
            //총알 발사
            GameObject instantBullet = Instantiate(bullet7, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        }

        IEnumerator ShotH()
        {
            //총알 발사
            GameObject instantBullet = Instantiate(bullet7, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

            yield return new WaitForSeconds(0.2f);

            //총알 발사
            instantBullet = Instantiate(bullet8, bulletPos.position, bulletPos.rotation);
            bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            CaseRigid = instantCase.GetComponent<Rigidbody>();
            caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        }
        IEnumerator ShotI()
        {
            //총알 발사
            GameObject instantBullet = Instantiate(bullet7, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

            yield return new WaitForSeconds(0.2f);

            //총알 발사
            instantBullet = Instantiate(bullet8, bulletPos.position, bulletPos.rotation);
            bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            CaseRigid = instantCase.GetComponent<Rigidbody>();
            caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

            yield return new WaitForSeconds(0.2f);

            //총알 발사
            instantBullet = Instantiate(bullet9, bulletPos.position, bulletPos.rotation);
            bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 500;

            yield return null;
            //탄피 배출
            instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            CaseRigid = instantCase.GetComponent<Rigidbody>();
            caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
            CaseRigid.AddForce(caseVec, ForceMode.Impulse);
            CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        }
    
}

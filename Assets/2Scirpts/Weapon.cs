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
    public Transform bulletCasePos;
    public GameObject bulletCase;

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
            StartCoroutine("Shot");
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


   /* private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
            other.GetComponent<Enemy>().OnDamage(10);
    }*/
}

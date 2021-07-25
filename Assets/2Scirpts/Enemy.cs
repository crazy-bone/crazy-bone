using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target;
    /// <summary> 체력바 </summary>
    public Transform HealthBarValue;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;


    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        //nav.SetDestination(target.position);

        // 체력바 업데이트
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (HealthBarValue == null)
            return;
        
        float ratio = (float)curHealth / maxHealth;
        HealthBarValue.localPosition = new Vector3(1.6f - ratio * 1.6f, 0f, 0f);
        HealthBarValue.localScale = new Vector3(ratio * 10f, 1f, 1f);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            StartCoroutine(OnDamage());
        }
         else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            StartCoroutine(OnDamage());
        }
    }*/

    IEnumerator OnDamge()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {

        }
    }

   
}

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



    public void Use()
    {
        damageEnabled = true;
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
        yield return new WaitForSeconds(0.1f); //0.1√  ¥Î±‚
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
            other.GetComponent<Enemy>().OnDamage(10);
    }
}

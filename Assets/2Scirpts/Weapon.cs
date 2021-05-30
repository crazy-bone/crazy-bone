using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range };
    public Type type;
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
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }

    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1000);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1200);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1400);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1600);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1800);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2000);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2200);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2400);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2600);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2800);
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 3000);
        yield return new WaitForSeconds(3f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1000);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1200);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1400);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1600);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1800);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2000);
        bulletRigid.velocity = bulletPos.forward * -10; 
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2200);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2400);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2600);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2800);
        bulletRigid.velocity = bulletPos.forward * -10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 3000);
        bulletRigid.velocity = bulletPos.forward * -10; 
        yield return new WaitForSeconds(3f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1000);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1200);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1400);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1600);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 1800);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2000);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2200);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2400);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2600);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 2800);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * 3000);
        bulletRigid.velocity = bulletPos.forward * 10;
        yield return new WaitForSeconds(1f);
        /*bulletRigid.velocity = bulletPos.forward * 50;//add force도 가능
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 75;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 125;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 150;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 175;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 200;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 225;
        yield return new WaitForSeconds(1f);
        intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 250;*/
    }

    //use() 메인루틴 -> swing() 서브루틴 -> Use() 메인루틴
    //use() 메인루틴 + swing() 코루틴(같이 실행되는 것임. co)
    //우리는 코루틴을 쓸거임, 열거형 함수 IEnumerator, 결과를 전달하는 yield
    //
}

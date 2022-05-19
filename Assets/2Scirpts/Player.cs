using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 15f;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public GameManager manager;
    public HealthHUD healthHUD;

    public int ammo;
    public int coin;
    public int health;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool ctrlDown;
    bool leftMouseDown;

    public bool isHealth = false;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isBorder;
    bool isFireReady = true;
    bool isDamage;
    bool isDead = false;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;

    GameObject nearObject;
    Weapon equipWeapon;

    int equipWeaponIndex = -1;


    // Start is called before the first frame update

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();

    }


    // Update is called once per frame
    void Update()
    {
        // 함수 모음
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Dodge();
        Interation();
        Swap();
        Die();
        UpdateHealthHUD();
    }

    void GetInput()
    {
        // 입력값 모음
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        ctrlDown = Input.GetKeyDown(KeyCode.LeftControl);
        leftMouseDown = Input.GetMouseButtonDown(0);
    }

    void Move()
    {
        // 플레이어의 이동
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap || isDead)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * (wDown ? 2f : 1f) * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        // 점프 여러 제약을 걸어둠
        if (jDown && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * 40, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Floor과 Wall을 맞았을때 점프가 끝나야 하기 때문에 설정
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void Attack()
    {
        // 무기를 들었을때, 공격로직, isFireReady가 만족할때, 즉 rate가 끝나면 공격할 수 있음
        if (equipWeapon == null)
            return;

        isFireReady = equipWeapon.rate <= 0;

        if (leftMouseDown && isFireReady && !isDodge && !isSwap && !isDead)
        {
            equipWeapon.Use();
            equipWeapon.rate = 1;
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
           
        }
    }

     void Dodge()
       {
        // 플레이어 구르기 로직
           if (ctrlDown && !isDodge && !isSwap)
           {
               dodgeVec = moveVec;
               anim.SetTrigger("doDodge");
                
                if (wDown)
                {
                    speed *= 2;
                }

                if (!wDown)
                {
                    speed *= 3;
                }

               isDodge = true;

               Invoke("DodgeOut", 0.3f);
           }
       }
    
    void DodgeOut()
    {
        // 닷지를 끝낼 즈음에 속도 원상복귀, 달리기냐 걷기냐에 따라 다름
        if(speed == 30)
        {
            speed *= 1 / 2f;
        }
        if (speed == 45)
        {
            speed *= 1 / 3f;
        }
        isDodge = false;

    }

     void Swap()
     {
        //TODO: SWap로직, 애니메이션 없으므로 아직 미구현
         if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
             return;
         if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
             return;
         if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
             return;

         int weaponIndex = -1;
         if (sDown1) weaponIndex = 0;
         if (sDown2) weaponIndex = 1;
         if (sDown3) weaponIndex = 2;

         if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
         {
             if (equipWeapon != null)
                 equipWeapon.gameObject.SetActive(false);

             equipWeaponIndex = weaponIndex;
             equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
             equipWeapon.gameObject.SetActive(true);

             //anim.SetTrigger("doSwap");

             isSwap = true;

             Invoke("SwapOut", 0.4f);

         }
     }
    
    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        // 아이템 줍기
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void Die()
    {
        if (health <= 0 && isDead == false)
        {
            // 피가 0이하면 죽음. 한번 죽었을시 계속 죽지 않도록 isDead의 조건 추가
            OnDie();
        }
    }

    void UpdateHealthHUD()
    {
        if (healthHUD == null)
            return;

        healthHUD.UpdateHealthBar(health, maxHealth);
    }


    void FreezeRotation()
    {
        // 혼자서 도는 버그 수정
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        // 벽을 뚫어버리는 버그 수정
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));

    }

    private void FixedUpdate()
    {
        // Update랑 분리함
        FreezeRotation();
        StopToWall();
    }



    void OnTriggerEnter(Collider other)
    {
        // 아이템에 충돌 시 해당 아이템 값의 수치를 올려줌
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                    //총알
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                    // 코인
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                    // 피
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    isHealth = true;
                    break;
                    // 폭탄
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;

            }// 먹으면 오브젝트는 사라짐
            Destroy(other.gameObject);

        }
        // 투사체에 맞은 경우
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage && !isDead)
            {
                // 피가 닳고 데미지 맡는 로직이 발생
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                StartCoroutine(OnDamge());
            }

            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }

        // 송곳 공격에 맞은 경우
        else if (other.tag == "EnemyAwl")
        {
            if (!isDamage && !isDead)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                StartCoroutine(OnDamge());


            }
        }
        // 송곳 공격 두번째
        else if (other.tag == "AwlDamage2")
        {
            if (!isDamage && !isDead)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                StartCoroutine(OnDamge());

                Destroy(other.gameObject);
            }
        }

        
    }

    IEnumerator OnDamge()
    {
        // 데미지를 받을 때 표현하기 위해서 mesh 색깔이 바뀌는 로직
        if (isDead == false)
        {
            // 데미지 받는 중이라는 것 체크
            isDamage = true;
            foreach (MeshRenderer mesh in meshs)
            {// mesh 색을 바꿔줌
                mesh.material.color = Color.yellow;
            }
            // 데미지 받는 애니메이션
            anim.SetTrigger("doDamaged");
            yield return new WaitForSeconds(1f);

            isDamage = false;

            foreach (MeshRenderer mesh in meshs)
            {// 원상 복귀
                mesh.material.color = Color.white;
            }
        }

    }
    void OnDie()
    {
        // 죽을 때
        anim.SetTrigger("doDie");
        isDead = true;
        // 죽는 Panel 
        manager.GameOver();
    }

    void OnTriggerStay(Collider other)
    {
        // 무기가 있을때 충돌을 인지시킴
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;

    }
}

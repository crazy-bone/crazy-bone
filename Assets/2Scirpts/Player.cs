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
    public Vector3 position;
    public GameObject Aposition;
    public GameObject Bposition;
    public GameObject Cposition;
    public GameObject Dposition;
    public GameObject Eposition;

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

    public GameObject triggerSetActive;
    public GameObject triggerSetActive2;
    public GameObject triggerSetActive3;
    public GameObject triggerSetActive4;
    public GameObject triggerSetActive5;
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
        // ?Լ? ????
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

        if(coin == 100)
        {
            coin = 0;
            triggerSetActive.GetComponent<TriggerSetActive>().setActive();
        }
        if (coin == 200)
        {
            coin = 0;
            triggerSetActive2.GetComponent<TriggerSetActive>().setActive();
        }
        if (coin == 300)
        {
            coin = 0;
            triggerSetActive3.GetComponent<TriggerSetActive>().setActive();
        }
        if (coin == 400)
        {
            coin = 0;
            triggerSetActive4.GetComponent<TriggerSetActive>().setActive();
        }
        if (coin == 500)
        {
            coin = 0;
            triggerSetActive5.GetComponent<TriggerSetActive>().setActive();
        }
    }

    void GetInput()
    {
        // ?Է°? ????
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
        // ?÷??̾??? ?̵?
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
        // ???? ???? ?????? ?ɾ???
        if (jDown && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * 40, ForceMode.Impulse);
            //anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            //isJump = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Floor?? Wall?? ?¾????? ?????? ?????? ?ϱ? ?????? ????
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
        // ???⸦ ????????, ???ݷ???, isFireReady?? ?????Ҷ?, ?? rate?? ?????? ?????? ?? ????
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
        // ?÷??̾? ?????? ????
           if (ctrlDown && !isDodge && !isSwap)
           {
                equipWeapon.Use();
                equipWeapon.rate = 1;
                dodgeVec = moveVec;
               anim.SetTrigger("doDodge");
                
                if (wDown)
                {
                    speed *= 10;
                }

                if (!wDown)
                {
                    speed *= 6;
                }

               isDodge = true;

               Invoke("DodgeOut", 0.2f);
           }
       }
    
    void DodgeOut()
    {
        // ?????? ???? ?????? ?ӵ? ???󺹱?, ?޸????? ?ȱ??Ŀ? ???? ?ٸ?
        if(speed == 200)
        {
            speed *= 1 / 10f;
        }
        if (speed == 120)
        {
            speed *= 1 / 6f;
        }
        isDodge = false;

    }

     void Swap()
     {
        //TODO: SWap????, ?ִϸ??̼? ?????Ƿ? ???? ?̱???
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
        // ?????? ?ݱ?
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
            // ?ǰ? 0???ϸ? ????. ?ѹ? ?׾????? ???? ???? ?ʵ??? isDead?? ???? ?߰?
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
        // ȥ?ڼ? ???? ???? ????
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        // ???? ?վ??????? ???? ????
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));

    }

    private void FixedUpdate()
    {
        // Update?? ?и???
        FreezeRotation();
        StopToWall();
    }



    void OnTriggerEnter(Collider other)
    {
        // ?????ۿ? ?浹 ?? ?ش? ?????? ???? ??ġ?? ?÷???
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                    //?Ѿ?
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    Destroy(other.gameObject);
                    break;
                    // ????
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    Destroy(other.gameObject);
                    break;
                    // ??
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    isHealth = true;
                    Destroy(other.gameObject);
                    break;
                    // ??ź
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    Destroy(other.gameObject);
                    break;
                case Item.Type.Trap:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    isHealth = true;
                    break;

            }// ?????? ??????Ʈ?? ??????

        }
        // ????ü?? ???? ????
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage && !isDead)
            {
                // ?ǰ? ???? ?????? ?ô? ?????? ?߻?
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                StartCoroutine(OnDamge());
            }

            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }

        // ?۰? ???ݿ? ???? ????
        else if (other.tag == "EnemyAwl")
        {
            if (!isDamage && !isDead)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                StartCoroutine(OnDamge());


            }
        }
        // ?۰? ???? ?ι?°
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
        // ???????? ???? ?? ǥ???ϱ? ???ؼ? mesh ?????? ?ٲ??? ????
        if (isDead == false)
        {
            // ?????? ?޴? ???̶??? ?? üũ
            isDamage = true;
            foreach (MeshRenderer mesh in meshs)
            {// mesh ???? ?ٲ???
                mesh.material.color = Color.yellow;
            }
            // ?????? ?޴? ?ִϸ??̼?
            anim.SetTrigger("doDamaged");
            yield return new WaitForSeconds(1f);

            isDamage = false;

            foreach (MeshRenderer mesh in meshs)
            {// ???? ????
                mesh.material.color = Color.white;
            }
        }

    }
    void OnDie()
    {
        // ???? ??
        anim.SetTrigger("doDie");
        isDead = true;
        // ?״? Panel 
        manager.GameOver();
    }

    void OnTriggerStay(Collider other)
    {
        // ???Ⱑ ?????? ?浹?? ??????Ŵ
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;

    }
    public void CheRukUp()
    {
        maxHealth += 100;
      
    }

    public void RePositionA()
    {
        transform.position = Aposition.transform.position;
    }
    public void RePositionB()
    {
        transform.position = Bposition.transform.position;
    }
    public void RePositionC()
    {
        transform.position = Cposition.transform.position;
    }
    public void RePositionD()
    {
        transform.position = Dposition.transform.position;
    }
    public void RePositionE()
    {
        transform.position = Eposition.transform.position;
    }


}

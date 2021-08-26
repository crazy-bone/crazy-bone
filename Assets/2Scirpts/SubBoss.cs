using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubBoss : Enemy
{
    public enum Type
    {
        Range,
        Melee
    };

    /// <summary> 원거리/근거리 유형 </summary>
    public Type type;
    /// <summary> 플레이어 </summary>
    public Player player;
    /// <summary> 미사일 템플릿 Prefab, 클론하여 사용됨 </summary>
    public BossMissile missileTemplate;
    /// <summary> 미사일 발사 위치 </summary>
    public Transform[] missilePorts;
    /// <summary> 접촉 데미지 </summary>
    public int contactDamage = 10;

    Animator anim;
    bool isdead = false;

    private enum Status
    {
        IDLE,
        Attack,
        Die

    }

    private NavMeshAgent navMeshAgent;
    private Status status = Status.IDLE;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        base.Update();

        if(isdead == false)
        navMeshAgent.SetDestination(target.position);
        
       
        switch (status)
        {
            case Status.IDLE:
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance <= navMeshAgent.stoppingDistance && isdead == false)
                {
                    StartCoroutine(Attack());
                    anim.SetTrigger("doAttack");
                }
                break;

        }

        if ((float)curHealth / maxHealth <= 0f && isdead == false)
        {
            anim.SetTrigger("doDie");
            isdead = true;
            Destroy(gameObject, 3f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            player.health -= contactDamage;


            if (type == Type.Range) // 원거리 서브웬디고
                Destroy(gameObject); //TODO: 웬디고가 플레이어 근처까지 안 쫓아옴
        }
    }

    IEnumerator Attack()
    {
        status = Status.Attack;

        yield return new WaitForSeconds(2f);

        // 미사일 발사
        foreach (Transform missilePort in missilePorts)
        {
            BossMissile missile = Instantiate<BossMissile>(missileTemplate, missilePort.position, missilePort.rotation);
            missile.transform.localScale = new Vector3(1f, 1f, 1f); 
            missile.target = target;
            missile.moveSpeed = 50f;
            missile.turnSpeed = 180f;
        }

        yield return new WaitForSeconds(1f);

        status = Status.IDLE;
    }
}

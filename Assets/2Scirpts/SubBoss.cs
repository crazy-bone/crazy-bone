using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubBoss : MonoBehaviour
{
    public Transform target; // 공격 대상
    public float attackDistance; // 공격할 거리
    public BossMissile missileTemplate;
    public Transform[] missilePorts;

    private enum Status
    {
        IDLE,
        Attack
    }

    private NavMeshAgent navMeshAgent;
    private Status status = Status.IDLE;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (status)
        {
            case Status.IDLE:
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < attackDistance)
                    StartCoroutine(Attack());
                else
                    navMeshAgent.SetDestination(target.position);
                break;
        }
    }

    IEnumerator Attack()
    {
        status = Status.Attack;
        navMeshAgent.enabled = false;
        Debug.Log("서브웬디고 정지");

        yield return new WaitForSeconds(2f);

        Debug.Log("서브보스 미사일 발사");
        foreach (Transform missilePort in missilePorts)
        {
            BossMissile missile = Instantiate<BossMissile>(missileTemplate, missilePort.position, missilePort.rotation);
            missile.target = target;
        }

        yield return new WaitForSeconds(1f);

        status = Status.IDLE;
        navMeshAgent.enabled = true;
    }
}

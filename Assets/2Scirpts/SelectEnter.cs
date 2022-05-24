
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 트리거 배경음악 재생기 : 스크립트#2. 트리거측
// 설명 : 마을에 진입할 때 트리거에 닿으면 배경음악을 재생합니다.
//  플레이어의 tag 속성을 'myplayer'로 지어주거나, 아래의 소스를 변경해 주시면 됩니다.
// 주석을 삭제하지 않는 조건으로 자유롭게 사용하셔도 됩니다.
// 개발 : Cray
// 블로그 : 크레이의 IT 탐구 https://itadventure.tistory.com/415
public class SelectEnter : MonoBehaviour
{
    public GameObject UI;
    public GameObject MonsterA;
    public GameObject MonsterB;
    public GameObject MonsterC;
    public GameObject MonsterD;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UI.SetActive(true);
            MonsterA.SetActive(true);
            MonsterB.SetActive(true);
            MonsterC.SetActive(true);
            MonsterD.SetActive(true);
            Destroy(gameObject);
        }
    }
}
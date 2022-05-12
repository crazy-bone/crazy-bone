using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 트리거 배경음악 재생기 : 스크립트1. 카메라측
// 설명 : 마을에 진입할 때 트리거에 닿으면 배경음악을 재생합니다.
// 주석을 삭제하지 않는 조건으로 자유롭게 사용하셔도 됩니다.
// 개발 : Cray
// 블로그 : 크레이의 IT 탐구 https://itadventure.tistory.com/415
public class PlayMusicOperator : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    // Inspector 에표시할 배경음악 목록
    public BgmType[] BGMList;

    private AudioSource BGM;
    private string NowBGMname = "";

    void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        if (BGMList.Length > 0) PlayBGM(BGMList[0].name);
    }

    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
            }
    }
}
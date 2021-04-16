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

    public void Use()
    {
        if(type == Type.Melee)
        {
            Swing();
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return null; //1프레임 대기
        //2
        yield return null; //1프레임 대기
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        yield break; //break로 코루틴 탈출 가능 그러나 break 밑의 함수는 
        //실행 안되기 때문에 주의할 것.

    }

    //use() 메인루틴 -> swing() 서브루틴 -> Use() 메인루틴
    //use() 메인루틴 + swing() 코루틴(같이 실행되는 것임. co)
    //우리는 코루틴을 쓸거임, 열거형 함수 IEnumerator, 결과를 전달하는 yield
    //
}

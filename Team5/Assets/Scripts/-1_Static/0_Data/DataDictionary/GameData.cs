using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임에서 사용되는 데이터들의 부모 클래스 : 데이터의 아이디와 이름이 있다. 
///  아이템, 적, 사운드 파일 등 모든 리소스에 사용될 예정
/// </summary>
public abstract class GameData : ScriptableObject
{
    public abstract string id {get;}
    public abstract string dataName {get;}
}

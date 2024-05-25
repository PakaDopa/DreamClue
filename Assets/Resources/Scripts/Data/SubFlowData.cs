using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class SubFlowData
{
    //Did   name    isInteraction Sfx bgm   context1    context2   context3    context4
    //Dia_1 Kick	              Cry	    꿈을 꾸는중..		

    public string Did;                  // 다이얼로그 ID
    public string actorName;            // 액터(캐릭터) 이름
    public bool isInteraction = false;  // 상호작용을 해야하는 다이얼로그
    public string sfxFileName;          // sfx 파일 이름
    public string bgmFileName;          // bgm 파일 이름
    public string[] contexts;           // 표시할 이름
}

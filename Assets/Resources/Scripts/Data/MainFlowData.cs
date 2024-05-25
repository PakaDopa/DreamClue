using System;
using UnityEngine;

[Serializable]
public class MainFlowData
{
    // Mid         dialogueId  isEnd IsAuto  isDream
    // Story001    Dia_1             TRUE
    // Story001    Dia_1                     TRUE
    // Story002    Dia_2       TRUE

    public string Mid;                  // 메인 Id
    public string dialogueId;           // 다이얼로그 아이디
    public bool isEnd = false;          // 게임의 끝을 나타내는 bool
    //public bool isAuto = false;         // 게임이 시작하자마자 진행되는 텍스트인지 아닌지
    public bool isDream = false;        // 해당 Mid가 꿈인지 아닌지
}

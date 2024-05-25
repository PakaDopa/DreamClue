using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DialogueData : ScriptableObject
{
    // 다이얼로그 ID
    [SerializeField] public SubFlowData[] datas;
}

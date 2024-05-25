
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    // 어디에서나 접근이 읽기 데이터 자료구조입니다.
    //Key : Mid or Did
    //Value : 해당하는 ID의 데이터
    private Dictionary<string, List<MainFlowData>> mainFlowData;
    private Dictionary<string, List<SubFlowData>> subFlowData;

    public Dictionary<string, List<MainFlowData>> GetMainFlowData() => mainFlowData;
    public List<SubFlowData> GetSubFlowData(string DId) => subFlowData[DId];

    /// <summary>
    /// 저장된 Prefab을 자료구조로 옮기는 Init 작업 함수 <PreloadScene>에서 호출 요망</PreloadScene>
    /// </summary>
    public void Init(MainDialogueData mDatas, DialogueData sDatas)
    {
        mainFlowData = new Dictionary<string, List<MainFlowData>>();
        subFlowData = new Dictionary<string, List<SubFlowData>>();

        foreach(MainFlowData data in mDatas.datas)
        {
            string Mid = data.Mid;

            if (!mainFlowData.ContainsKey(Mid))
                mainFlowData.Add(Mid, new List<MainFlowData>());
            
            mainFlowData[Mid].Add(data);

        }

        foreach (SubFlowData data in sDatas.datas)
        {
            string Mid = data.Did;

            if (!subFlowData.ContainsKey(Mid))
                subFlowData.Add(Mid, new List<SubFlowData>());

            subFlowData[Mid].Add(data);

        }
    }
}


using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class PreloadData : MonoBehaviour
{
    [Header("Bulid Version Must Check False")]
    [SerializeField] private bool _isDebug = true;

    const string _prefixPath = "Assets/Resources/CSV/";
    const string _mainFileName = "MainFlowCSV.csv";
    const string _subFileName = "SubFlowCSV.csv";

    const string _mainFlowPrefabPath = "Assets/Resources/Prefabs/MainFlowPrefabs/";
    const string _subFlowPrefabPath = "Assets/Resources/Prefabs/SubFlowPrefabs/";

    private void Start()
    {
        try
        {
            CustomDebug.Log("프리팹 데이터 로드 시작");

            if (_isDebug == true)
            {
                Read_MainFlowCSV();
                Read_SubFlowCSV();
            }

            string mPath = "Prefabs/MainFlowPrefabs/MainDialogue";
            string sPath = "Prefabs/SubFlowPrefabs/SubDialogue";

            var mDatas = GetResource<MainDialogueData>(mPath);
            var sDatas = GetResource<DialogueData>(sPath);

            if (mDatas == null || sDatas == null)
                throw new System.Exception("    프리팹 데이터를 불러오지 못했습니다.");

            GameManager.Instance.Init(mDatas, sDatas);

            CustomDebug.Log("프리팹 데이터 로드 완료");

            SceneManager.LoadScene("GameScene");
        }
        catch(System.Exception e)
        {
            CustomDebug.ErrorLog(e.Message);
        }
    }

    private void Read_MainFlowCSV()
    {
        string fullPath = _prefixPath + _mainFileName;
        List<Dictionary<string, object>> obj = CSVReader.Read(fullPath);
        if (obj == null || obj.Count == 0)
            throw new System.Exception("Main Flow CSV 파일을 찾을 수 없습니다. 경로를 확인하십시오.");

        // Make Prefab
        MainDialogueData dialogue = ScriptableObject.CreateInstance<MainDialogueData>();

        List<MainFlowData> datas = new List<MainFlowData>();
        foreach (Dictionary<string, object> row in obj)
        {
            MainFlowData data = new MainFlowData();

            data.Mid = row["Mid"].ToString();
            data.dialogueId = row["dialogueId"].ToString();
            bool.TryParse(row["isEnd"].ToString(), out data.isEnd);
            bool.TryParse(row["isAuto"].ToString(), out data.isAuto);
            bool.TryParse(row["isDream"].ToString(), out data.isDream);

            datas.Add(data);
        }
        dialogue.datas = datas.ToArray();

        AssetDatabase.CreateAsset(dialogue, _mainFlowPrefabPath + "MainDialogue.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return;
    }
    private void Read_SubFlowCSV()
    {
        string fullPath = _prefixPath + _subFileName;
        List<Dictionary<string, object>> obj = CSVReader.Read(fullPath);
        if (obj == null || obj.Count == 0)
            throw new System.Exception("Sub Flow CSV 파일을 찾을 수 없습니다. 경로를 확인하십시오.");

        // Make Prefab
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        List<SubFlowData> datas = new List<SubFlowData>();
        foreach (Dictionary<string, object> row in obj)
        {
            SubFlowData data = new SubFlowData();

            data.Did = row["Did"].ToString();
            data.actorName = row["name"].ToString();
            bool.TryParse(row["isInteraction"].ToString(), out data.isInteraction);
            data.sfxFileName = row["Sfx"].ToString();
            data.bgmFileName = row["bgm"].ToString();

            List<string> contexts = new List<string>();
            foreach(string key in row.Keys)
                if(key.Contains("context"))
                    contexts.Add(row[key].ToString());
            data.contexts = contexts.ToArray();

            datas.Add(data);
        }

        dialogue.datas = datas.ToArray();
        AssetDatabase.CreateAsset(dialogue, _subFlowPrefabPath + "SubDialogue.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return;
    }

    private T GetResource<T>(string path) where T : Object
    {
        var resource = Resources.Load(path, typeof(ScriptableObject));
        return (T)resource;

    }
}
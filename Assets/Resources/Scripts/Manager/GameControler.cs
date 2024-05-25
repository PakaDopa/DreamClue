using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

public class GameControler : MonoBehaviour
{
    [SerializeField] private string _startMidName = "";

    List<MainFlowData> _currentMain;
    int _currentMainIndex = 0;

    List<SubFlowData> _currentSub;
    int _currentSubIndex = 0;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_MAIN_INDEX, UpdateMainIndex);
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_SUB_INDEX, UpdateSubIndex);
        EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_MAIN_EVENT, EndMain);
        EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_SUB_EVENT, EndSub);

        Dictionary<string, List<MainFlowData>> datas = GameManager.Instance.GetMainFlowData();
        _currentMain = datas.First().Value;

        _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[_currentMainIndex].dialogueId);
        string name = _currentSub[_currentSubIndex].actorName;
        List<string> contexts = _currentSub[_currentSubIndex].contexts.ToList();

        CustomDebug.Log(string.Format("{0}  {1}", name, contexts[0]));  
        EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
            this,
            new TransformEventArgs(null, name, contexts));
    }

    void UpdateMainIndex(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {

    }

    void UpdateSubIndex(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {

    }

    void EndSub(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {

    }

    void EndMain(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {

    }
}

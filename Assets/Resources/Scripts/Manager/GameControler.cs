
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Actor
{
    NAME0 = 0,
    NAME1 = 1,
    NAME2 = 2,
    NAME3 = 3,
}

public class GameControler : MonoBehaviour
{
    [SerializeField] private string _startMidName = "";

    List<MainFlowData> _currentMain;
    int _currentMainIndex = 0;

    List<SubFlowData> _currentSub;
    int _currentSubIndex = 0;

    [Header("UI Component")]
    public TMP_Text mainText;
    public TMP_Text[] actorText;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_MAIN_INDEX, UpdateMainIndex);
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_SUB_INDEX, UpdateSubIndex);
        EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_MAIN_EVENT, EndMain);
        EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_SUB_EVENT, EndSub);

        Dictionary<string, List<MainFlowData>> datas = GameManager.Instance.GetMainFlowData();
        _currentMain = datas.First().Value;

        _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[_currentMainIndex].dialogueId);

        EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
            this,
            new TransformEventArgs(null, _currentSub[_currentSubIndex].actorName, _currentSub[_currentSubIndex].contexts));
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

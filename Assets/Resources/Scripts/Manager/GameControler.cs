using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Utility;

public class GameControler : MonoBehaviour
{
    List<MainFlowData> _currentMain;
    int _currentMainIndex = 0;

    List<SubFlowData> _currentSub;
    int _currentSubIndex = 0;

    Dictionary<string, List<MainFlowData>> _datas;
    List<string> _keys;

    void Start()
    {
        //EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_MAIN_INDEX, UpdateMainIndex);
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_SUB_INDEX, UpdateSubIndex);
        //EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_MAIN_EVENT, EndMain);
        //EventManager.Instance.AddListener(EVENT_TYPE.COMPLETE_SUB_EVENT, EndSub);

        //초기 Init
        _datas = GameManager.Instance.GetMainFlowData();
        _keys = _datas.Keys.ToList();

        // 공통되는 스크립트 파트
        _currentMain = _datas[_keys[_currentMainIndex]];
        _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[0].dialogueId);
        string name = _currentSub[_currentSubIndex].actorName;
        List<string> contexts = _currentSub[_currentSubIndex].contexts.ToList();

        EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
            this,
            new TransformEventArgs(null, name, contexts));
    }

    void UpdateMainIndex(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {

    }

    void UpdateSubIndex(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        _currentSubIndex++;
        //CustomDebug.Log("   _currentSubIndex " + _currentSubIndex);
        if (_currentSub.Count <= _currentSubIndex)
            EndSub();
        else
        {
            string name = _currentSub[_currentSubIndex].actorName;
            List<string> contexts = _currentSub[_currentSubIndex].contexts.ToList();
            EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
                this,
                new TransformEventArgs(null, name, contexts));
        }
    }

    //Sub이 끝낫다는 것은 Main이 하나 증가한다는 뜻
    void EndSub()
    {
        _currentMainIndex++;
        if (_currentMainIndex >= _keys.Count)
            EndMain();
        
        string key = _keys[_currentMainIndex];
        _currentMain = _datas[_keys[_currentMainIndex]];

        if (_currentMain[0].isDream)
            CustomDebug.Log(" Dream 이벤트입니다.");

        _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[0].dialogueId);
        _currentSubIndex = 0;

        string name = _currentSub[_currentSubIndex].actorName;
        List<string> contexts = _currentSub[_currentSubIndex].contexts.ToList();
  
        EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
            this,
            new TransformEventArgs(null, name, contexts));
    }
    //게임 종료
    void EndMain()
    {
        CustomDebug.Log("게임 끝!!");
    }
}

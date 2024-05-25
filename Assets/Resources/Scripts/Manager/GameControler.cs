using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private RectTransform _answerPanel;
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_SUB_INDEX, UpdateSubIndex);

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

    void UpdateSubIndex(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        _currentSubIndex++;
        if (bool.Parse(args.value[0].ToString()) == true)
        {
            EndSub();
            CustomDebug.Log("꿈에서 나옵니다.");
        }
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
        {
            EndMain();
            return;
        }
        
        string key = _keys[_currentMainIndex];
        _currentMain = _datas[_keys[_currentMainIndex]];

        if (_currentMain[0].isDream) // 꿈인 경우
        {
            _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[0].dialogueId);

            EventManager.Instance.PostNotification(EVENT_TYPE.ENTER_DREAM, this, new TransformEventArgs(null, _currentSub));
            
        }
        else //꿈이 아닌 경우
        {
            _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[0].dialogueId);
            _currentSubIndex = 0;

            string name = _currentSub[_currentSubIndex].actorName;
            List<string> contexts = _currentSub[_currentSubIndex].contexts.ToList();

            EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_TEXT,
                this,
                new TransformEventArgs(null, name, contexts));
        }
    }
    //게임 종료
    void EndMain()
    {
        CustomDebug.Log("게임 끝!!");
        _answerPanel.gameObject.SetActive(true);
    }
}

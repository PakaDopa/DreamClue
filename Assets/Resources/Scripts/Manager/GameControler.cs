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

        //�ʱ� Init
        _datas = GameManager.Instance.GetMainFlowData();
        _keys = _datas.Keys.ToList();

        // ����Ǵ� ��ũ��Ʈ ��Ʈ
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
            CustomDebug.Log("�޿��� ���ɴϴ�.");
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

    //Sub�� �����ٴ� ���� Main�� �ϳ� �����Ѵٴ� ��
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

        if (_currentMain[0].isDream) // ���� ���
        {
            _currentSub = GameManager.Instance.GetSubFlowData(_currentMain[0].dialogueId);

            EventManager.Instance.PostNotification(EVENT_TYPE.ENTER_DREAM, this, new TransformEventArgs(null, _currentSub));
            
        }
        else //���� �ƴ� ���
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
    //���� ����
    void EndMain()
    {
        CustomDebug.Log("���� ��!!");
        _answerPanel.gameObject.SetActive(true);
    }
}

using DG.Tweening;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public enum ActorState
{
    TEXTING, // 출력중
    TEXT,    // 출력완료
    NONE,    // 
}

public class ActorControler : MonoBehaviour
{
    [SerializeField]
    public string _actorName = "null";

    [Header("UI Component")]
    public TMP_Text _textBox;
    public TMP_Text _nameTextbox;

    [Header("UI Component")]
    [SerializeField] public RectTransform _image;
    [SerializeField] public RectTransform _originImage;

    [SerializeField]
    public List<string> _currentText;
    private int _index = 0;
    public ActorState _state = ActorState.NONE;

    [SerializeField]
    bool _dreamMode = false;

    private void Awake()
    {
        _currentText = new List<string>();

        EventManager.Instance.AddListener(EVENT_TYPE.SHOW_TEXT, ShowText);
        EventManager.Instance.AddListener(EVENT_TYPE.ENTER_DREAM, EnterDream);
        EventManager.Instance.AddListener(EVENT_TYPE.ENTER_NORMAL, EnterNormal);
        EventManager.Instance.AddListener(EVENT_TYPE.SHOW_OFF_TEXT, OffShowText);
        if (_nameTextbox != null)
            _nameTextbox.text = _actorName;
    }
    private void ShowText(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        if (args.value.Length <= 0)
            return;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;
        ScaleUPEffect(false);

        if (args.value[0].ToString() != _actorName)
        {
            // 내가 해당되는 버튼이 아닌거임 =ㅅ=
            return;
        }
        _textBox.enabled = true;
        bnt.enabled = true;
        //_state = ActorState.TEXT;
        CustomDebug.Log("!! Show text " + _actorName);
        //초기화
        _currentText.Clear();
        _currentText = (List<string>)args.value[1];
        _index = 0;

        ClickBnt();
    }
    private void EnterDream(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        //초기화
        _currentText.Clear();
        _index = 0;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;
        ScaleUPEffect(false);

        // 꿈에서 진행되는 대화가 가능한 오브젝트는, 매개변수로부터 전달 받아서 
        // _currentText Init 해줘야함
        _currentText.Clear();
        foreach(SubFlowData data in (List<SubFlowData>)args.value[0])
        {
            if (data.actorName == _actorName)
            {
                _currentText = data.contexts.ToList();
                break;
            }
        }

        if (_currentText.Count != 0)
        {
            bnt.enabled = true;
        }
        // 모드 변경
        _dreamMode = true;
    }
    private void EnterNormal(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        //초기화
        _currentText.Clear();
        _index = 0;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;

        // 모드 변경
        _dreamMode = false;
    }
    private void OffShowText(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        string id = args.value[0].ToString();

        if (id != _actorName)
            _textBox.enabled = false;
    }
    private void ScaleUPEffect(bool enable)
    {
        if(enable)
        {
            _image.DOScale(1.125f, 0.25f).SetEase(Ease.InOutCirc);
        }
        else
        {
            _image.DOScale(1.0f, 0.25f);
        }
    }
    public void ClickBnt()
    {
        if(_dreamMode)
        {
            // 채팅창 다 꺼라
            EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_OFF_TEXT,
                    this,
                    new TransformEventArgs(null, _actorName));

            if (_currentText.Count <= _index || _currentText[_index] == "") // 제공되는 딥 이야기 끝~
            {
                CustomDebug.Log("더 이상 볼 이야기가 없습니다~");
                return;
            }
            else
            {
                //Effect On
                ScaleUPEffect(true);
                _textBox.enabled = true;
                _textBox.text = _currentText[_index].ToString();
                _index++;
            }
        }
        else
        {
            if (_currentText.Count <= _index || _currentText[_index] == "") //다음 Did로 넘어가야함/
            {
                //_textBox.enabled = false;
                EventManager.Instance.PostNotification(EVENT_TYPE.UPDATE_SUB_INDEX, this, new TransformEventArgs(null, false));
            }
            else
            {
                ScaleUPEffect(true);
                _textBox.text = _currentText[_index].ToString();
                _index++;
            }
        }

    }
    public void ExitDream()
    {
        if(_dreamMode)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.ENTER_NORMAL, this, new TransformEventArgs(null));
            EventManager.Instance.PostNotification(EVENT_TYPE.UPDATE_SUB_INDEX, this, new TransformEventArgs(null, true));
        }
    }
}

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
    TEXTING, // �����
    TEXT,    // ��¿Ϸ�
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
            // ���� �ش�Ǵ� ��ư�� �ƴѰ��� =��=
            return;
        }
        _textBox.enabled = true;
        bnt.enabled = true;
        //_state = ActorState.TEXT;
        CustomDebug.Log("!! Show text " + _actorName);
        //�ʱ�ȭ
        _currentText.Clear();
        _currentText = (List<string>)args.value[1];
        _index = 0;

        ClickBnt();
    }
    private void EnterDream(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        //�ʱ�ȭ
        _currentText.Clear();
        _index = 0;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;
        ScaleUPEffect(false);

        // �޿��� ����Ǵ� ��ȭ�� ������ ������Ʈ��, �Ű������κ��� ���� �޾Ƽ� 
        // _currentText Init �������
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
        // ��� ����
        _dreamMode = true;
    }
    private void EnterNormal(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        //�ʱ�ȭ
        _currentText.Clear();
        _index = 0;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;

        // ��� ����
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
            // ä��â �� ����
            EventManager.Instance.PostNotification(EVENT_TYPE.SHOW_OFF_TEXT,
                    this,
                    new TransformEventArgs(null, _actorName));

            if (_currentText.Count <= _index || _currentText[_index] == "") // �����Ǵ� �� �̾߱� ��~
            {
                CustomDebug.Log("�� �̻� �� �̾߱Ⱑ �����ϴ�~");
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
            if (_currentText.Count <= _index || _currentText[_index] == "") //���� Did�� �Ѿ����/
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

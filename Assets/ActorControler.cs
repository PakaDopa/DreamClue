using System.Collections.Generic;
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
    
    [SerializeField]
    public List<string> _currentText;
    private int _index = 0;
    public ActorState _state = ActorState.NONE;

    private void Awake()
    {
        _currentText = new List<string>();

        EventManager.Instance.AddListener(EVENT_TYPE.SHOW_TEXT, ShowText);

        if (_nameTextbox != null)
            _nameTextbox.text = _actorName;
    }
    private void ShowText(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        // 0 index -> actorType
        // 1 index -> text
        if (args.value.Length <= 0)
            return;

        var bnt = GetComponent<Button>();
        bnt.enabled = false;
        _textBox.enabled = false;
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

    public void ClickBnt()
    {
        if (_currentText.Count <= _index || _currentText[_index] == "") //다음 Did로 넘어가야함/
        {
            //_textBox.enabled = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.UPDATE_SUB_INDEX, this, new TransformEventArgs(null));
        }
        else
        {
            _textBox.text = _currentText[_index].ToString();
            _index++;
        }
    }
}

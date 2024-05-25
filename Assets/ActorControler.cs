using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility;

public class ActorControler : MonoBehaviour
{
    [SerializeField]
    public string _actorName = "null";

    [Header("UI Component")]
    public TMP_Text _textBox;
    public TMP_Text _nameTextbox;

    [SerializeField]
    public List<string> _currentText;

    private void Start()
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
        if (args.value[0].ToString() != _actorName)
            return;
        //CustomDebug.Log("Find Object!");
        _currentText.Clear();
        _currentText = (List<string>)args.value[1];

        _textBox.text = _currentText[0];
    }

    private void OnMouseDown()
    {
        if (_actorName == "null")
            return;
        Debug.Log("Click!");
    }
}

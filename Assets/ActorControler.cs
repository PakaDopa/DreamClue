using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorControler : MonoBehaviour
{
    [SerializeField]
    public string _actorName = "";
    public Actor _actorType = Actor.NAME0;

    [Header("UI Component")]
    public TMP_Text _textBox;

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SHOW_TEXT, ShowText);
    }

    private void ShowText(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        // 0 index -> actorType
        // 1 index -> text
        if (args.value.Length <= 0)
            return;
        if ((Actor)args.value[0] != _actorType)
            return;
        string context = args.value[1].ToString();

        _textBox.text = context;
    }
}

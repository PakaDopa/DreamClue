using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NarationText : MonoBehaviour
{
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Vector3 targetPos;

    string dreamText = "<�Ϸ簡 ������ <color=#8b00ff>��</color>�� �ð��� ã�ƿԽ��ϴ�.>";
    string normalText = "<�ٽ� ���� ã�ƿԽ��ϴ�.>";

    Sequence effect;

    public float delayTime = 1f;

    private void Awake()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SHOW_NARATION, Show);
        effect = DOTween.Sequence();
        DOTween.Init();
    }
    void Show(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        bool enable = bool.Parse(args.value[0].ToString()); //�� = true, �� = false
        var transform = GetComponent<RectTransform>();
        
        if(!enable)
            GetComponent<TMP_Text>().text = dreamText;
        else
            GetComponent<TMP_Text>().text = normalText;

        // effect.Append(transform.DOMove(targetPos, 0.5f).SetEase(Ease.InOutExpo));

        Vector3 originPos = transform.position;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMove(targetPos, 0.8f)).SetEase(Ease.OutExpo)
        .Insert(delayTime + 1.25f, transform.DOMove(originPos, 1.25f).SetEase(Ease.InOutExpo));

        //StartCoroutine(OffText());    
    }
    IEnumerator OffText()
    {
        yield return new WaitForSeconds(1.0f);
    }
}

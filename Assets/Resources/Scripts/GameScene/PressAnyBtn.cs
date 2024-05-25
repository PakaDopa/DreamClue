using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyBtn : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textMeshProObject; //===깜빡일 텍스트 메쉬 프로 객체===//

    [SerializeField]
    private string nextSceneName = "StartStory"; //===전환할 씬의 이름===//

    [SerializeField]
    private float blinkDuration = 1.0f; //===깜빡임 지속 시간===//

    private bool isBlinking = false;

    void Start()
    {
        if (textMeshProObject != null)
        {
            StartCoroutine(BlinkText());
        }
        else
        {
            Debug.LogError("텍스트 메쉬 프로 객체가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator BlinkText()
    {
        isBlinking = true;
        while (isBlinking)
        {
            //===페이드 아웃===//
            yield return StartCoroutine(FadeTextAlpha(1, 0));

            //===페이드 인===//
            yield return StartCoroutine(FadeTextAlpha(0, 1));
        }
    }

    IEnumerator FadeTextAlpha(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        Color color = textMeshProObject.color;

        while (elapsedTime < blinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / blinkDuration);
            textMeshProObject.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        textMeshProObject.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}

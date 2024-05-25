using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class StartStory : MonoBehaviour
{
    public Image imageToShow; // 이미지를 보여줄 Image 컴포넌트
    public float imageDisplayTime = 5f; // 이미지를 표시할 시간

    public TextMeshProUGUI textDisplay; // 텍스트를 표시할 Text 컴포넌트
    public string[] textsToDisplay; // 인스펙터에서 설정할 텍스트 배열

    public string nextSceneName = "GameScene"; // 다음으로 전환할 씬 이름

    public GameObject Panel;

    private void Start()
    {
        StartCoroutine(DisplayImageAndText());
    }

    IEnumerator DisplayImageAndText()
    {
        // 이미지 보여주기
        imageToShow.gameObject.SetActive(true);

        // 일정 시간 후에 이미지 비활성화
        yield return new WaitForSeconds(imageDisplayTime);
        imageToShow.gameObject.SetActive(false);
        Panel.SetActive(true);
        textDisplay.gameObject.SetActive(true);


        // 텍스트 표시
        foreach (string text in textsToDisplay)
        {
            textDisplay.text = text;
            yield return new WaitForSeconds(4f);
        }

        // 다음 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}

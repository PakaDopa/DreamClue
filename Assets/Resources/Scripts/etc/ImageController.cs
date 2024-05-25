using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ImageController : MonoBehaviour
{
    public Image imageUI;
    public TextMeshProUGUI textUI;
    public Sprite[] images;
    public string[] texts;
    public float fadeDuration = 1f;
    public float displayTime = 2f;
    public GameObject Panel;

    private int currentIndex = 0;
    private Color transparentColor;
    private Color opaqueColor;

    void Start()
    {
        if (images.Length == 0 || texts.Length == 0 || images.Length != texts.Length)
        {
            Debug.LogError("Image or text array is empty or their lengths don't match!");
            return;
        }

        transparentColor = new Color(1, 1, 1, 0);
        opaqueColor = new Color(1, 1, 1, 1);

        imageUI.color = transparentColor;
        textUI.color = transparentColor;

        imageUI.sprite = images[currentIndex];
        textUI.text = texts[currentIndex];

        StartCoroutine(FadeImages());
    }

    private void Update()
    {
        if (currentIndex == 8)
        {
            Panel.SetActive(false); // 패널 비활성화
            textUI.gameObject.SetActive(false); // 텍스트 비활성화
        }
    }


    IEnumerator FadeImages()
    {
        for (int i = 0; i < images.Length; i++)
        {
            yield return StartCoroutine(FadeIn());

            yield return new WaitForSeconds(displayTime);

            if (i < images.Length - 1)
            {
                yield return StartCoroutine(FadeOut());

                currentIndex = (currentIndex + 1) % images.Length;
                imageUI.sprite = images[currentIndex];
                textUI.text = texts[currentIndex];
            }
            else
            {
                Panel.SetActive(false); // 패널 비활성화
                textUI.gameObject.SetActive(false); // 텍스트 비활성화
            }
        }
    }


    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            imageUI.color = Color.Lerp(transparentColor, opaqueColor, timer / fadeDuration);
            textUI.color = Color.Lerp(transparentColor, opaqueColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        imageUI.color = opaqueColor;
        textUI.color = opaqueColor;
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            imageUI.color = Color.Lerp(opaqueColor, transparentColor, timer / fadeDuration);
            textUI.color = Color.Lerp(opaqueColor, transparentColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        imageUI.color = transparentColor;
        textUI.color = transparentColor;
    }
}

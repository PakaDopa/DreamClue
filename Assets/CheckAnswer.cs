using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

public class CheckAnswer : MonoBehaviour
{
    [Header("UI Component")]
    private RectTransform[] _layouts;

    [SerializeField]
    public int[] answers;

    public void Click()
    {
        bool[] array = new bool[3] { false, false, false };
        
        for(int i = 0; i <  3; i++)
            array[i] = Check(i);

        bool result = true;
        for (int i = 0; i < 3; i++)
        {
            if (array[i] == false)
            {
                result = false;
                break;
            }
        }

        // °ÔÀÓ 2±è
        if(result)
        {
            SceneManager.LoadScene("WinScene");
        }
        else //½ÇÆÐ
        {
            SceneManager.LoadScene("FailScene");
        }
    }

    bool Check(int ind)
    {
        var layout = _layouts[ind];

        for(int i = 0; i < layout.childCount; i++)
        {
            var child = layout.GetChild(i);

            Toggle toggle = child.GetComponentInChildren<Toggle>();
            if (toggle == null)
            {
                CustomDebug.Log("¹º°¡ ¹º°¡.. ¹®Á¦°¡ »ý±è¤Ð Åä±Û¿¡!");
                continue;
            }

            if (toggle.isOn && i == answers[ind])
                return true;
        }
        return false;
        
    }
}

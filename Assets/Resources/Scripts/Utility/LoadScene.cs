using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    public void ChangeScene()
    {
        try
        {
            SceneManager.LoadScene(_sceneName);
        }
        catch(Exception e)
        {
            CustomDebug.ErrorLog(e.Message);
        }
    }
}
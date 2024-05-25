using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformEventArgs : EventArgs
{
    public Transform transform;
    public object[] value;
    public TransformEventArgs(Transform transform, params object[] value)
    {
        this.transform = transform;
        this.value = value;
    }
}
public enum EVENT_TYPE
{
    COMPLETE_MAIN_EVENT, // 현재 처리중인 Mid가 완전히 끝이나서 다음 Mid로 넘어가야 함.
    UPDATE_MAIN_INDEX,   // 현재 처리중인 Mid의 다음으로 넘어감. 인덱스가 끝에 도달하면, COMPLETE 이벤트 실행

    COMPLETE_SUB_EVENT,  // 현재 처리중인 Did가 완전히 끝이나서 Mid가 끝남.
    UPDATE_SUB_INDEX,    // 현재 처리중인 Did의 텍스트를 다 처리해서 다음 Did를 실행

    SHOW_TEXT,           // 현재 가르키고 있는 Did의 텍스트를 알맞은 Textbox에 전시
    
}

public class EventManager : Singleton<EventManager>
{
    protected EventManager() { }

    // 대리자 선언
    public delegate void OnEvent(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null);
    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

    public void AddListener(EVENT_TYPE eventType, OnEvent Listener)
    {
        List<OnEvent> ListenList = null;

        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }

        ListenList = new List<OnEvent>
        {
            Listener
        };
        Listeners.Add(eventType, ListenList);
    }

    public void PostNotification(EVENT_TYPE eventType, Component Sender, TransformEventArgs args = null)
    {
        List<OnEvent> ListenList = null;

        if (!Listeners.TryGetValue(eventType, out ListenList))
            return;


        for (int i = 0; i < ListenList.Count; i++)
        {
            ListenList?[i](eventType, Sender, args);
        }
    }
    public void RemoveListener(EVENT_TYPE eventType, object target)
    {
        if (Listeners.ContainsKey(eventType) == false)
            return;

        foreach (OnEvent ev in Listeners[eventType])
        {
            if (target == ev.Target)
            {
                Listeners[eventType].Remove(ev);
                Debug.Log("Event Delete Success");
                return;
            }
        }

        Debug.Log("Event Delete Fail");
        return;
    }
    public void RemoveEvent(EVENT_TYPE eventType) => Listeners.Remove(eventType);
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<OnEvent>> newListeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

        foreach (KeyValuePair<EVENT_TYPE, List<OnEvent>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                newListeners.Add(Item.Key, Item.Value);
        }

        Listeners = newListeners;
    }

    void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}

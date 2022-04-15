using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestUnityAction : MonoBehaviour
{
    //https://www.urablog.xyz/entry/2016/09/11/080000
    [System.Serializable]
    public class IntUnityEvent : UnityEngine.Events.UnityEvent<int> { }

    [SerializeField] IntUnityEvent m_events = new IntUnityEvent();

    void Start()
    {
        m_events.AddListener(CallbackMethod);
        
        //AddListnerで引数持ちのメソッドを登録することで，実際に処理を実行する
        //登録しないと，引数をわたし，Eventを実行しても，処理する内容が，存在しない．
        m_events.Invoke(100);
    }

    void CallbackMethod(int i_arg)
    {
        Debug.Log($"CallbackMethod arg = {i_arg}");
    }
}

//https://doruby.jp/users/ino/entries/%E3%80%90C--Unity%E3%80%91%E3%82%B3%E3%83%AB%E3%83%BC%E3%83%81%E3%83%B3(Coroutine)%E3%81%A8%E3%81%AF%E4%BD%95%E3%81%AA%E3%81%AE%E3%81%8B

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorutinTest : MonoBehaviour
{
    public Text outText;
    public float waitSecond;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Corutiner));
    }

IEnumerator Corutiner(){
    outText.text = "first ";
    if (waitSecond>0) yield return new WaitForSecondsRealtime(waitSecond);
    else yield return null;

    outText.text += "second ";
}

}

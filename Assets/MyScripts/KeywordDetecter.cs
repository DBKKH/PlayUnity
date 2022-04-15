using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class KeywordDetecter : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        //反応するキーワードを辞書に登録
        keywords.Add("こんにちは", () =>
        {
            Debug.Log("「こんにちは」を指定");
        });
        keywords.Add("おはよう", () =>
        {
            Debug.Log("「こんにちは」を指定");
        });
        keywords.Add("こ", () =>
        {
            Debug.Log("「こ」を指定");
        });

        keywords.Add("お", () =>
        {
            Debug.Log("「お」を指定");
        });

        keywords.Add("は", () =>
        {
            Debug.Log("「は」を指定");
        });
        keywords.Add("わ", () =>
        {
            Debug.Log("「わ」を指定");
        });
        //キーワードを渡す
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        //キーワードを認識したら反応するOnPhraseRecognizedに「KeywordRecognizer_OnPhraseRecognized」処理を渡す
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        //音声認識開始
        keywordRecognizer.Start();
        Debug.Log("音声認識開始");
    }


    //キーワードを認識したときに反応する処理
    void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //デリゲート
        //イベントやコールバック処理の記述をシンプルにしてくれる。クラス・ライブラリを活用するには必須らしい
        System.Action keywordAction;//　keywordActionという処理を行う

        //認識されたキーワードが辞書に含まれている場合に、アクションを呼び出す。
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            // keywordAction.Invoke();
            Debug.Log("認識した");
        }
    }


}
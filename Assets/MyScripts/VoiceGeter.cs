using UnityEngine;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用

public class VoiceGeter : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;

    void Start()
    {
        dictationRecognizer = new DictationRecognizer();

        //ディクテーションを開始
        dictationRecognizer.Start();
        Debug.Log("音声認識開始");
    }

    void Update()
    {
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;//DictationRecognizer_DictationResult処理を行う

        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;//DictationRecognizer_DictationHypothesis処理を行う

        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;//DictationRecognizer_DictationComplete処理を行う

        dictationRecognizer.DictationError += DictationRecognizer_DictationError;//DictationRecognizer_DictationError処理を行う
    }

    //DictationResult：音声が特定の認識精度で認識されたときに発生するイベント
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("認識した音声：" + text);
    }

    //DictationHypothesis：音声入力中に発生するイベント
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.Log("音声認識中：" + text);
    }

    //DictationComplete：音声認識セッションを終了したときにトリガされるイベント
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("音声認識完了");
    }

    //DictationError：音声認識セッションにエラーが発生したときにトリガされるイベント
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("音声認識エラー");
    }
}
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace AsyncSample
{
    // HTTP通信をコルーチンで行い、結果をObservableで扱う
    public class ObservableAsyncSample : MonoBehaviour
    {
        // 通信先のURI
        [SerializeField] private string _uri1;
        [SerializeField] private string _uri2;

        private void Start()
        {
            GetHttpAsync(_uri1)
                // 通信に失敗したらログに出して、
                // １秒待ってからリトライを通信総数が3回になるまで実行する
                .OnErrorRetry((Exception ex) => Debug.LogException(ex),
                    retryCount: 3,
                    delay: TimeSpan.FromSeconds(1))
                // 1つ目のURIにアクセスできたら、2つのURIにアクセスする
                .ContinueWith(_ => GetHttpAsync(_uri2))
                .Subscribe(
                    // 成功時
                    result => Debug.Log(result),
                    // 失敗時
                    ex => Debug.LogException(ex)
                ).AddTo(this);
        }

        // HTTP GETの結果をObservableで返す
        private IObservable<string> GetHttpAsync(string uri)
        {
            // 結果を通知する用のSubjectを生成
            var asyncSubject = new AsyncSubject<string>();

            // コルーチン起動
            StartCoroutine(GetHttpCoroutine(uri, asyncSubject));

            // 結果をObservable経由で伝達する
            return asyncSubject;

            /*
             わかりやすくするためにあえてAsyncSubjectをつかったが、
             Observable.FromCoroutine で書いた方がコードは短くできる
 
             // return Observable.FromCoroutine<string>(observer => GetHttpCoroutine(_uri, observer)); 
            */
        }


        // HTTP GETを実行するコルーチン
        private IEnumerator GetHttpCoroutine(string uri, IObserver<string> observer)
        {
            using var uwr = UnityWebRequest.Get(uri);

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                // 通信失敗時はOnErrorメッセージを発行する
                observer.OnError(new Exception(uwr.error));
                yield break;
            }

            // 成功時は結果を返す
            observer.OnNext(uwr.downloadHandler.text);
            observer.OnCompleted();
        }
    }
}
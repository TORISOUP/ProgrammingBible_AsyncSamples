using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AsyncSample
{
    // HTTP通信をコルーチンで実行する
    public class CoroutineSample : MonoBehaviour
    {
        // 通信先のURI
        [SerializeField] private string _uri;

        private void Start()
        {
            // コルーチン起動時に完了時の処理を登録
            StartCoroutine(GetHttpCoroutine(_uri));
        }

        // HTTP GETを実行するコルーチン
        private IEnumerator GetHttpCoroutine(string uri)
        {
            using var uwr = UnityWebRequest.Get(uri);

            // SendWebRequestの戻り値はUnityWebRequestAsyncOperation
            // 通信が完了するまでyield returnが毎フレーム実行され続ける
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                // 通信失敗時
                Debug.LogException(new Exception(uwr.error));
                yield break;
            }

            // 成功時
            Debug.Log(uwr.downloadHandler.data);
        }
    }
}
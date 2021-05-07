using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AsyncSample
{
    public class AsyncAwaitSample : MonoBehaviour
    {
        [SerializeField] private string _uri;

        private async UniTaskVoid Start()
        {
            try
            {
                // 非同期処理をawaitする
                var result = await GetHttpAsync(_uri);
                Debug.Log(result);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        // HTTP GETを実行する
        private async UniTask<string> GetHttpAsync(string uri)
        {
            using var uwr = UnityWebRequest.Get(uri);

            // UniTaskを導入することで
            // AsyncOperationをawaitできるようになる
            // 通信失敗時は Cysharp.Threading.Tasks.UnityWebRequestException が発行される
            await uwr.SendWebRequest();
            
            // 成功時は結果を返す
            return uwr.downloadHandler.text;
        }
    }
}
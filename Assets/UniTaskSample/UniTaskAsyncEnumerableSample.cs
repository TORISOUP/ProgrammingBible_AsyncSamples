using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace UniTaskSample
{
    public class UniTaskAsyncEnumerableSample : MonoBehaviour
    {
        // URIの配列
        [SerializeField] private string[] _uris;

        private async UniTaskVoid Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await foreach (var result in GetHttpAsyncEnumerable(_uris))
            {
                Debug.Log(result);

                // 結果を表示したら1秒まって次の結果を要求する
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }

            // C# 7.xな環境で利用する場合は
            // ForEachAsync,ForEachAwaitAsync,Subscribe,SubscribeAwait で
            // await foreachの代替となる
            /*
            await GetHttpAsyncEnumerable(_uris)
                .ForEachAwaitWithCancellationAsync(async (result, ct) =>
                {
                    Debug.Log(result);

                    // 結果を表示したら1秒まって次の結果を要求する
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct);
                }, cancellationToken: token);
            */
        }

        private IUniTaskAsyncEnumerable<string> GetHttpAsyncEnumerable(string[] uris)
        {
            // URIの内容を順番にダウンロードする
            return UniTaskAsyncEnumerable.Create<string>(async (writer, ct) =>
            {
                foreach (var uri in _uris)
                {
                    using var uwr = UnityWebRequest.Get(uri);
                    await uwr.SendWebRequest().ToUniTask(cancellationToken: ct);

                    // 非同期イテレータにおける yield return に相当
                    await writer.YieldAsync(uwr.downloadHandler.text);
                }
            });
        }
    }
}
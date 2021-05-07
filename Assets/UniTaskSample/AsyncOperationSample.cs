using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UniTaskSample
{
    public class AsyncOperationSample : MonoBehaviour
    {
        private void Start()
        {
            DoAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DoAsync(CancellationToken token)
        {
            try
            {
                // Resources.LoadAsync
                var texture =
                    (await Resources.LoadAsync<Texture>("SampleImage")
                        .ToUniTask(cancellationToken: token)) as Texture;

                // UnityWebRequest
                var result = await GetHttpAsync("https://github.com/", token);

                // UnityWebRequest with IProgress<float>
                var result2 = await GetHttpWithProgressAsync(
                    "https://github.com/",
                    Progress.Create<float>(x => Debug.Log(x)),
                    token);

                // SceneManager.LoadSceneAsync
                await SceneManager.LoadSceneAsync(sceneBuildIndex: 1, LoadSceneMode.Single);
            }
            // OperationCanceledException はUniTaskの制御に必要なのでcatchせずスルーする
            catch (Exception e) when (!(e is OperationCanceledException))
            {
                Debug.LogException(e);
            }
        }

        // UnityWebRequestによる通信
        private async UniTask<string> GetHttpAsync(string uri, CancellationToken token)
        {
            using var uwr = UnityWebRequest.Get(uri);
            await uwr.SendWebRequest().ToUniTask(cancellationToken: token);
            return uwr.downloadHandler.text;
        }

        // IProgressを用いた通信状態の取得
        private async UniTask<string> GetHttpWithProgressAsync(string uri,
            IProgress<float> progress,
            CancellationToken token)
        {
            using var uwr = UnityWebRequest.Get(uri);
            await uwr.SendWebRequest().ToUniTask(progress, cancellationToken: token);
            return uwr.downloadHandler.text;
        }
    }
}
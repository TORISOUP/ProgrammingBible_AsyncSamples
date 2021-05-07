using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTaskSample
{
    public class ContextSwitchSample : MonoBehaviour
    {
        [SerializeField] private string _path;

        private void Start()
        {
            ReadFileAsync(_path, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid ReadFileAsync(string path, CancellationToken token)
        {
            // これ以降の処理をスレッドプールに切り替え
            await UniTask.SwitchToThreadPool();

            // スレッドプール上にてファイル読み込み
            var text = File.ReadAllText(path);

            // Unityメインスレッドに戻す
            await UniTask.SwitchToMainThread();

            Debug.Log(text);
        }
    }
}
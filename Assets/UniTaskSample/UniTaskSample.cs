using System;
using System.IO;
using System.Threading;
// using System.Threading.Tasks;
using Cysharp.Threading.Tasks; // System.Threading.Tasks の代わりにこちらをusingする
using UnityEngine;

namespace UniTaskSample
{
    public class UniTaskSample : MonoBehaviour
    {
        [SerializeField] private string _path;

        // キャンセルに用いるCancellationTokenSource
        private CancellationTokenSource _cancellationTokenSource;


        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // 非同期読み込み開始
            // Forget()を呼び出すことで、awaitしてない時に発生する警告を抑制できる
            WaitForReadAsync(_path, _cancellationTokenSource.Token).Forget();        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        // ファイルの読み込みを待機してログに出す
        private async UniTask WaitForReadAsync(string path, CancellationToken token)
        {
            try
            {
                var result = await ReadFileAsync(path, token);
                Debug.Log(result);
            }
            catch (Exception e) when (!(e is OperationCanceledException))
            {
                Debug.LogException(e);
            }
        }

        // ファイルを非同期に読み取る
        private async UniTask<string> ReadFileAsync(string path, CancellationToken token)
        {
            // UniTask.Run でも動くがObsoleteなので
            // UniTask.RunOnThreadPool を推奨
            return await UniTask.RunOnThreadPool(() => File.ReadAllText(path), cancellationToken: token);
        }
    }
}
using System;
using System.IO;
using System.Threading;
// using System.Threading.Tasks;
using Cysharp.Threading.Tasks; // System.Threading.Tasks の代わりにこちらをusingする
using UnityEngine;

namespace UniTaskSample
{
    public class UniTaskVoidSample : MonoBehaviour
    {
        [SerializeField] private string _path;

        // キャンセルに用いるCancellationTokenSource
        private CancellationTokenSource _cancellationTokenSource;

        // void Start() は
        // async UniTaskVoid Start() に置換しても動く
        private async UniTaskVoid Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            try
            {
                var result = await ReadFileAsync(_path, token);
                Debug.Log(result);
            }
            catch (Exception e) when (!(e is OperationCanceledException))
            {
                Debug.LogException(e);
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        // ファイルを非同期に読み取る
        private async UniTask<string> ReadFileAsync(string path, CancellationToken token)
        {
            return await UniTask.RunOnThreadPool(() => File.ReadAllText(path), cancellationToken: token);
        }
    }
}
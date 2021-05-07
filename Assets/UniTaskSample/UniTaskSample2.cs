using System;
using System.IO;
using System.Threading;
// using System.Threading.Tasks;
using Cysharp.Threading.Tasks; // System.Threading.Tasks の代わりにこちらをusingする
using UnityEngine;

namespace UniTaskSample
{
    public class UniTaskSample2 : MonoBehaviour
    {
        [SerializeField] private string _path;

        private async UniTaskVoid Start()
        {
            // OnDestroy()時に自動的にキャンセルされる
            // CancellationToken を発行する
            var token = this.GetCancellationTokenOnDestroy();

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

        // ファイルを非同期に読み取る
        private async UniTask<string> ReadFileAsync(string path, CancellationToken token)
        {
            return await UniTask.RunOnThreadPool(() => File.ReadAllText(path), cancellationToken: token);
        }
    }
}
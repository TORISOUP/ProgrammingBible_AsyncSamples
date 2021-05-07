using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTaskSample
{
    // 標準のTaskを用いた非同期処理のサンプル
    public class TaskSample : MonoBehaviour
    {
        [SerializeField] private string _path;

        // キャンセルに用いるCancellationTokenSource
        private CancellationTokenSource _cancellationTokenSource;


        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            // 非同期読み込み開始
            _ = WaitForReadAsync(_path, _cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        // ファイルの読み込みを待機してログに出す
        private async Task WaitForReadAsync(string path, CancellationToken token)
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
        private async Task<string> ReadFileAsync(string path, CancellationToken token)
        {
            return await Task.Run(() => File.ReadAllText(path), token);
        }
    }
}
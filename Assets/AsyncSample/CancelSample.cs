using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AsyncSample
{
    public class CancelSample : MonoBehaviour
    {
        // キャンセルに用いるCancellationTokenSource
        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            // CancellationTokenSource を用意
            _cancellationTokenSource = new CancellationTokenSource();

            // 移動開始
            // CancellationTokenを引数で渡す
            _ = MoveAsync(_cancellationTokenSource.Token);
        }

        // 1秒ごとに1mずつ移動する
        private async Task MoveAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                transform.position += Vector3.up;
                
                // 引数で渡されたCancellationTokenをバケツリレーのように
                // await対象に渡していく
                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
        }

        private void OnDestroy()
        {
            // Destroy時にキャンセル命令発行
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
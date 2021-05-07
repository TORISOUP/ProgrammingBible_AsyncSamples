using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTaskSample
{
    public class CoroutineToUniTaskSample : MonoBehaviour
    {
        private void Start()
        {
            // 非同期メソッド起動
            DoAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        // コルーチンとほぼ同等の機能
        private async UniTaskVoid DoAsync(CancellationToken token)
        {
            // yield return null に相当
            await UniTask.Yield();

            // CancellationToken を指定する場合
            await UniTask.Yield(token);

            // 実行タイミングを変更することも可能
            // yield return new WaitForFixedUpdate() 相当
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            await UniTask.WaitForFixedUpdate(token);

            // UniTask.NextFrameの場合は必ず1フレーム経過することを保障する
            await UniTask.NextFrame(token);
            await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, token);

            // 指定時間待機する
            // yield return new WaitForSeconds() 相当
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
        }
    }
}
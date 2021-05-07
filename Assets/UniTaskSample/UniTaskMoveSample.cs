using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTaskSample
{
    // UniTaskを用いた移動処理
    public class UniTaskMoveSample : MonoBehaviour
    {
        private void Start()
        {
            // CancellationTokenを忘れてはいけない
            MoveLoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        // 同じ場所をぐるぐる移動する
        private async UniTaskVoid MoveLoopAsync(CancellationToken token)
        {
            // CancellationTokenにより停止するため
            // while(true)で無限ループしても問題ない
            while (true)
            {
                await MoveAsync(Vector3.forward, 2, token);
                await MoveAsync(Vector3.right, 2, token);
                await MoveAsync(Vector3.back, 2, token);
                await MoveAsync(Vector3.left, 2, token);
            }
        }

        // 指定速度で一定時間移動する
        private async UniTask MoveAsync(Vector3 velocity, float duration, CancellationToken token)
        {
            var startTime = Time.time;
            while ((Time.time - startTime) <= duration)
            {
                transform.position += velocity * Time.deltaTime;

                // CancellationTokenの指定を忘れないこと
                await UniTask.Yield(token);
            }
        }
    }
}
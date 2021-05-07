using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace UniTaskSample
{
    public class MonoBehaviourEventSample : MonoBehaviour
    {
        private async UniTaskVoid Start()
        {
            var token = this.GetCancellationTokenOnDestroy();

            // Updateイベントのawait
            await this.GetAsyncUpdateTrigger().UpdateAsync(token);
            await this.GetAsyncFixedUpdateTrigger().FixedUpdateAsync(token);
            await this.GetAsyncLateUpdateTrigger().LateUpdateAsync(token);
            
            // OnCollision*** のawait
            var collisionEnter = await this.GetAsyncCollisionEnterTrigger()
                .OnCollisionEnterAsync(token);
            var collisionStay = await this.GetAsyncCollisionStayTrigger()
                .OnCollisionStayAsync(token);
            var collisionExit = await this.GetAsyncCollisionExitTrigger()
                .OnCollisionExitAsync(token);

            // OnDestroy のawait
            await this.GetAsyncDestroyTrigger().OnDestroyAsync();
            
            // 他にも多数あるが省略
        }
    }
}
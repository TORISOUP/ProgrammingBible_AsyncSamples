using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskSample
{
    public class UGuiEventSample : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            WaitForInputFieldAsync(token).Forget();
        }

        private async UniTaskVoid WaitForInputFieldAsync(CancellationToken token)
        {
            // InputFieldから「AsyncHandler」を取得する
            // AsyncHandlerを用いることで各種イベントがawait可能になる
            using var handler = _inputField.GetAsyncEndEditEventHandler(token);
            
            while (!token.IsCancellationRequested)
            {
                // OnEndEditイベントのawait
                var text = await handler.OnEndEditAsync();
                Debug.Log(text);
            }
        }
    }
}
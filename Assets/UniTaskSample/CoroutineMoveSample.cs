using System.Collections;
using UnityEngine;

namespace UniTaskSample
{
    public class CoroutineMoveSample : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(MoveLoopCoroutine());
        }

        // 同じ場所をぐるぐる移動するコルーチン
        private IEnumerator MoveLoopCoroutine()
        {
            while (true)
            {
                yield return MoveCoroutine(Vector3.forward, 2);
                yield return MoveCoroutine(Vector3.right, 2);
                yield return MoveCoroutine(Vector3.back, 2);
                yield return MoveCoroutine(Vector3.left, 2);
            }
        }

        // 指定速度で一定時間移動するコルーチン
        private IEnumerator MoveCoroutine(Vector3 velocity, float duration)
        {
            var startTime = Time.time;
            while ((Time.time - startTime) <= duration)
            {
                transform.position += velocity * Time.deltaTime;
                yield return null;
            }
        }
    }
}
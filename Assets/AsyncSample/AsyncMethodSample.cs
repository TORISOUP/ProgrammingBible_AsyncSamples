using System.IO;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AsyncMethodSample : MonoBehaviour
{
    // 元のメソッド
    public async Task<string> ReadFileAsync(string path)
    {
        var result = await Task.Run(() => File.ReadAllText(path));
        Console.WriteLine(result);
        return result;
    }
    
    // ↓　でコンパイル結果をさらに整理したもの

    // 非同期メソッドより生成されるステートマシンがわかりにくいので同等の挙動をするメソッドとして書き下したもの
    // 実際は構造体が生成されもっと効率的に動作する
    private Task<string> ReadFileAsync_(string path)
    {
        // TaskのAwaiterを取得する
        var awaiter = Task.Run(() => File.ReadAllText(path)).GetAwaiter();

        // 結果としてTaskを返すために用いる構造体
        var taskMethodBuilder = AsyncTaskMethodBuilder<string>.Create();

        if (awaiter.IsCompleted)
        {
            // await対象がすでに完了しているなら後続の処理を同期的に呼ぶ
            ReadFileAsync_Continuous(awaiter, taskMethodBuilder);
        }
        else
        {
            // await対象の非同期処理が実行中なら
            // その非同期処理のコールバックに後続の処理を追加する
            awaiter.OnCompleted(() => ReadFileAsync_Continuous(awaiter, taskMethodBuilder));
        }

        return taskMethodBuilder.Task;
    }

    // awaitより後に定義された処理内容
    private void ReadFileAsync_Continuous(TaskAwaiter<string> awaiter, AsyncTaskMethodBuilder<string> builder)
    {
        try
        {
            var result = awaiter.GetResult();
            
            // ここからawait後に定義された処理内容
            
            Console.WriteLine(result);
            
            // ここまで
            
            builder.SetResult(result);
        }
        catch (Exception e)
        {
            builder.SetException(e);
        }
    }
}
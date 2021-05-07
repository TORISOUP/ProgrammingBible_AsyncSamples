using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace AsyncSample
{
    public class AsyncSample
    {
        // 複数の指定パスのファイルを非同期で読み取る
        public async Task<IList<string>> ReadFileASync(string[] paths)
        {
            try
            {
                var results = new List<string>();
                foreach (var path in paths)
                {
                    using var reader = new StreamReader(path);
                    var text = await reader.ReadToEndAsync();

                    results.Add(text);
                }

                return results;
            }
            catch (Exception e)
            {
                // 例外発生時
                Debug.LogException(e);
                return null;
            }
        }
    }
}
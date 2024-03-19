/*
MIT License

created by : Stvp Ric

Copyright(c) 2023

Permission is hereby granted, free of charge, to any person obtaining a copy of this
software and associated documentation files (the "Software"), to deal in the Software
without restriction, including without limitation the rights to use, copy, modify,
merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace CompactTween.Extension
{
    public class CTmono : MonoBehaviour
    {
        [SerializeField] public List<(UnityEvent<int, float> callback, int index)> callbacks = new List<(UnityEvent<int, float>, int index)>();
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        public void BackupMonoCallback(Action<int, float> callback, int index)
        {
            var evt = new UnityEvent<int, float>();
            callback += (x, y)=> {Debug.Log("Testing testing testing");};
            evt.AddListener(new UnityAction<int, float>(callback));
            callbacks.Add((evt, index));
        }
        public void PokeTesting()
        {
            for(int i = 0; i < callbacks.Count; i++)
            {   
                callbacks[i].callback.Invoke(2, 1f);
            }
        }
        public void PassSecondCoroutine(WaitForSeconds iyield)
        {
            StartCoroutine(SecondPass(iyield));
        }
        public void PassSecondCoroutine(WaitForSeconds iyield, Action func)
        {
            StartCoroutine(SecondPass(iyield, func));
        }
        public void PassTaskCoroutine(TaskCompletionSource<bool> tcs)
        {
            StartCoroutine(NullPass(tcs));
        }
        IEnumerator NullPass(TaskCompletionSource<bool> tcs)
        {
            yield return null;

            if(tcs != null)
            {
                tcs.SetResult(true);
            }
        }
        /// <summary>Pass waitforseconds for yield waiting extension.</summary>
        IEnumerator SecondPass(WaitForSeconds iyield)
        {
            yield return iyield;
        }
        /// <summary>Pass waitforseconds for yield waiting extension.</summary>
        IEnumerator SecondPass(WaitForSeconds iyield, Action func)
        {
            yield return iyield;
        }
    }

}
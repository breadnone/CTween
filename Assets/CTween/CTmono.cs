using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CTween.Extension
{
    public class CTmono : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
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
using System.Collections;
using UnityEngine;
using CompactTween;
using CompactTween.Extension;
using TMPro;
using System.Runtime.InteropServices;

public class ctest_script : MonoBehaviour
{
    [SerializeField] private GameObject mov;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject rotateObject;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float angle;
    [SerializeField] private bool pingpong;
    [SerializeField] private int loopCount;
    [SerializeField] private bool infiniteLoop = false;
    [SerializeField] private float duration;
    [SerializeField] private int framerate = 12;
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Ease ease;
    [SerializeField] private RectTransform[] sprites;
    [SerializeField] private RectTransform spritecontainer;
    [SerializeField] private bool oncompleterepeat;
    [SerializeField] private bool exeOnCompleteCancel;
    [SerializeField] private GameObject from3d;
    [SerializeField] private GameObject to3d;

    Vector3 defPos;
    Vector3 rotDefPos;
    Vector3 targetdefpos;
    Vector3 defpos3d;
    Vector3 targetdefpos3d;
    Quaternion defrot3d;
    void Start()
    {
        defpos3d = from3d.transform.position;
        targetdefpos3d = to3d.transform.position;
        defPos = mov.transform.position;
        rotDefPos = rotateObject.transform.position;
        targetdefpos = target.transform.position;
        defrot3d = from3d.transform.rotation;
    }
    public void MoveToTarget()
    { 
        int counter = 0;
        mov.transform.position = defPos;
        var t =  CTween.move(mov.transform, targetdefpos, duration).onEase(ease).onPingPong(pingpong).onLoopCount(loopCount).onComplete(()=>
        {
            counter++;
            Debug.Log("Complete --  " + counter);
        }).onCompleteRepeat(oncompleterepeat).onInfinite(infiniteLoop);
        
        activeIndex = t.index;
    }
    public void MoveToTarget3D()
    { 
        int counter = 0;
        from3d.transform.position = defpos3d;
        var t =  CTween.move(from3d.transform, to3d.transform.position, duration).onEase(ease).onPingPong(pingpong).onLoopCount(loopCount).onComplete(()=>
        {
            counter++;
            Debug.Log("Complete --  " + counter);
        }).onCompleteRepeat(oncompleterepeat).onInfinite(infiniteLoop);
        
        activeIndex = t.index;
    }
    public void Translate()
    {
        mov.transform.position = defPos;
        CTween.translate(mov.transform, targetdefpos, duration, false).onPingPong(true).onLoopCount(loopCount).onEase(ease);
    }
    public void MoveSpeed()
    {
        mov.transform.position = defPos;
        CTween.move(mov.transform, target.transform.position, duration).onSpeed(2)
        .next(CTween.move(mov.transform, defPos, duration).onSpeed(2));
    }
    public void MoveToTargetOnCompleteRepeat()
    {
        int counter = 0;
        mov.transform.position = defPos;
        CTween.move(mov.transform, target.transform.position, duration).onEase(ease).onPingPong(pingpong).onLoopCount(loopCount).onComplete(()=>
        {
            counter++;
            Debug.Log("Complete --  " + counter);
        }).onCompleteRepeat(true);
    }
    public void MoveStressTest()
    {
        StartCoroutine(LoopTest());
        Debug.Log(Marshal.SizeOf<CTcore>());
    }
    public void Queueu()
    {
        mov.transform.position = defPos;
        ct = CTween.move(mov.transform, target.transform.position, duration).next(CTween.move(mov.transform, defPos, duration)).onEase(ease);
    }

    IEnumerator LoopTest()
    {
        for(int i = 0; i < 390; i++)
        {       
            var go = GameObject.Instantiate(mov, defPos, mov.transform.rotation);
            go.transform.SetParent(mov.transform.parent.transform, true);
            var y = UnityEngine.Random.Range(defPos.y - 400f, defPos.y + 400f);
            var rdm = UnityEngine.Random.Range(0.5f, duration);
            UnityEngine.Profiling.Profiler.BeginSample("CTween-Stress test");
            CTween.move(go.transform, new Vector3(target.transform.position.x, y, target.transform.position.z), rdm).onEase(ease).onLoopCount(1).onPingPong(true);
            UnityEngine.Profiling.Profiler.EndSample();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Rotate()
    {
        CTween.rotate(rotateObject.transform, direction, angle, duration).onEase(ease).onPingPong(pingpong).onLoopCount(loopCount);
    }
    public void Rotate3D()
    {
        from3d.transform.rotation = defrot3d;
        CTween.rotate(from3d.transform, direction, angle, duration).onEase(ease).onPingPong(pingpong).onLoopCount(loopCount);
    }
    public void FloatValue()
    {
        CTween.value(0, angle, duration, (float value)=>
        {
            text.SetText("value is -->   " + value);
        }).onLoopCount(loopCount).onPingPong(pingpong).onEase(ease);
    }
    public void CanvasAlpha()
    {
        CTween.alpha(canvas, 0f, duration).onLoopCount(loopCount).onPingPong(pingpong).onEase(ease);
    }
    public void RotateAround()
    {
        mov.transform.position = defPos;
        mov.transform.rotation = Quaternion.identity;
        CTween.rotateAround(mov.transform, target.transform.position, Vector3.forward, angle, duration).onLoopCount(loopCount).onEase(ease).onPingPong(pingpong);
    }
    public void Punch()
    {
        rotateObject.transform.position = rotDefPos;
        rotateObject.transform.rotation = Quaternion.identity;
        rotateObject.transform.localScale = new Vector3(1f, 1f, 1f);
        rotateObject.ctPunch2D(2f, duration);
    }
    public void Shake()
    {
        rotateObject.ctShake(2f, 25f, duration);
    }
    public void PlayAnim()
    {
        Sprite[] spr = new Sprite[sprites.Length];
        
        for(int i = 0; i < sprites.Length; i++)
        {
            spr[i] = sprites[i].GetComponent<UnityEngine.UI.Image>().sprite;
            sprites[i].gameObject.SetActive(false);
        }

        spritecontainer.ctAnim(spr, framerate).onPingPong(pingpong).onLoopCount(loopCount);
    }
    public void FollowTest()
    {
        CTween.follow(target.transform, mov.transform, 0.07f, Vector3.zero);
    }
    public void CancelTween()
    {
        if(!exeOnCompleteCancel)
            CTween.CancelAll();
        else
            CTween.CancelFirst(mov, true);
    }
    int activeIndex = -1;
    CoreTween ct = default;
    public void MoveCancelExecOnComplete()
    {
        int counter = 0;
        mov.transform.position = defPos;
        var t = CTween.move(mov.transform, targetdefpos, duration * 2f).onPingPong(pingpong).onLoopCount(loopCount).onComplete(()=>
        {
            counter++;
            Debug.Log("Complete was executed --  " + counter);
        }).onInfinite(infiniteLoop);

        activeIndex = t.index;
    }
    public void PauseTween()
    {
        CTween.PauseAll();
    }
    public void CancelQueue()
    {
        ct.TryCancelQueues();
    }
    public void ResumeTween()
    {
        CTween.ResumeAll();
    }
}

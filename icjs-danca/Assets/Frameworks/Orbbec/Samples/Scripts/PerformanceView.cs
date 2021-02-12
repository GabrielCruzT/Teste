using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PerformanceView : MonoBehaviour
{
    public Text fps;
    public Text cpu;
    public Text memory;
    public Text skeleton;

    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            cpu.gameObject.SetActive(false);
            memory.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            fps.text = string.Format("应用帧率 : {0}", PerformanceViewModel.Instance.GetFPS());
            if (Application.platform == RuntimePlatform.Android)
            {
                cpu.text = string.Format("CPU使用率 : {0}%", PerformanceViewModel.Instance.GetCpuUsage());
                memory.text = string.Format("内存占用(MB) : {0}", PerformanceViewModel.Instance.GetMemoryAverage());
            }
            skeleton.text = string.Format("骨架帧率 : {0}", PerformanceViewModel.Instance.GetSkeletonFPS());
        }
        else
        {
            fps.text = string.Format("App FPS : {0}", PerformanceViewModel.Instance.GetFPS());
            if (Application.platform == RuntimePlatform.Android)
            {
                cpu.text = string.Format("CPU usage : {0}%", PerformanceViewModel.Instance.GetCpuUsage());
                memory.text = string.Format("Memory(MB) average : {0}", PerformanceViewModel.Instance.GetMemoryAverage());
            }
            skeleton.text = string.Format("Skeleton FPS : {0}", PerformanceViewModel.Instance.GetSkeletonFPS());
        }
    }
}

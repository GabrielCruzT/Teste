using UnityEngine;
using System.Collections;

namespace OrbbecEx
{
    public class FpsLogger
    {
        private static FpsLoggerView _debuggerView = null;
        
        public static void Init(
            int fpsShowFrame = 20, 
            int cpuShowFrame = 1000,
            int memShowFrame = 1000,
            bool ignoreSlowFrame = true, 
            int sortOrder = 999, 
            int fpsRecordFrame = 100, 
            int cpuRecordFrame = 1000, 
            int memRecordFrame = 1000,
            FpsLoggerInfoMode defaultLoggerInfoMode = FpsLoggerInfoMode.Base)
        {
            if (_debuggerView == null)
            {
                // 统一挂在/OrbbecEx节点下
                GameObject orbbecExGO = GameObject.Find ("/OrbbecEx");
                if (orbbecExGO == null)
                {
                    orbbecExGO = new GameObject ("OrbbecEx");
                    GameObject.DontDestroyOnLoad (orbbecExGO);
                }

                GameObject go = null;
                GameObject prefab = Resources.Load<GameObject>("OrbbecEx/FpsLogger/FpsLogger");

                if (prefab != null)
                {
                    go = GameObject.Instantiate<GameObject>(prefab);
                    prefab = null;
                    Resources.UnloadUnusedAssets ();
                }

                if (go != null)
                {
                    go.transform.SetParent (orbbecExGO.transform, false);
                    _debuggerView = go.GetComponent<FpsLoggerView> ();

                    _debuggerView._fpsShowTotalFrame = fpsShowFrame;
                    _debuggerView._cpuShowTotalFrame = cpuShowFrame;
                    _debuggerView._memShowTotalFrame = memShowFrame;
                    _debuggerView._ignoreSlowFrame = ignoreSlowFrame;
                    _debuggerView._sortOrder = sortOrder;
                    _debuggerView._fpsRecordTotalFrame = Mathf.Max (fpsRecordFrame, fpsShowFrame);
                    _debuggerView._cpuRecordTotalFrame = Mathf.Max (cpuRecordFrame, cpuShowFrame);
                    _debuggerView._memRecordTotalFrame = Mathf.Max (memRecordFrame, memShowFrame);
                    _debuggerView.SetInfoMode (defaultLoggerInfoMode);
                }
            }
        }

        public static void Destroy()
        {
            if (_debuggerView != null)
            {
                GameObject.Destroy (_debuggerView.gameObject);
            }
        }

        public static void OpenLogger(FpsLoggerInfoMode infoMode = FpsLoggerInfoMode.Base, bool isRecordToFile = true)
        {
            if (_debuggerView != null)
            {
                _debuggerView.OpenLogger (infoMode, isRecordToFile);
            }
        }

        public static void BeginSample(string name)
        {
            if (_debuggerView != null)
            {
                _debuggerView.BeginSample (name);
            }
        }

        public static void EndSample()
        {
            if (_debuggerView != null)
            {
                _debuggerView.EndSample ();
            }
        }
    }
}

#if UNITY_ANDROID && !UNITY_EDITOR
#define RUNTIME_ANDROID
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace OrbbecEx
{
    public enum FpsLoggerInfoMode
    {
        None,
        Base,
        Detail
    }
    
    public class FpsLoggerView : MonoBehaviour , IPointerDownHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
    {
        #region region UI元素...
        [SerializeField] RectTransform _fpsLoggerRoot = null;
        [SerializeField] Canvas _fpsLoggerRootCanvas = null;
        [SerializeField] RectTransform _fpsLoggerPanel = null;
        [SerializeField] Text _fpsInfoText = null;
        [SerializeField] Text _modelInfoText = null;
        [SerializeField] Dropdown _showInfoDropDown = null;
        [SerializeField] List<Button> _colorButtons = null;

        internal int _sortOrder = 999;
        #endregion

        #region region 设备信息...
        string _productName;
        string _openDate;
        string _packageName;
        string _activityName;
        string _appVersion;
        string _deviceModel;
        string _deviceId;
        string _operatingSystem;
        string _systemMemorySize;
        string _cpuModel;
        string _cpuCount;
        string _cpuFrequency;
        string _gpuModel;
        string _gpuMemorySize;
        #endregion

        #region region fps内存cpu信息... 
        int _fps = 0;
        int _showFps = 0;
        float _fpsDeltaTime = 0.0f;
        #if UNITY_ANDROID && !UNITY_EDITOR
        int _appMemory = 0;
        int _availMemory = 0;
        int _cpuRate = 0;
        int _showAppMemory = 0;
        int _showAvailMemory = 0;
        int _showCpuRate = 0;
        #endif
        #endregion

        #region region 文字显示...
        float _showFpsWidth = 0.0f;
        float _showFpsHeight = 0.0f;
        float _showModelWidth = 0.0f;
        float _showModelHeight = 0.0f;
        FpsLoggerInfoMode _infoMode = FpsLoggerInfoMode.None;
        const string STR_FPS = "FPS : ";
        const string STR_CPU = "CPU : ";
        const string STR_MEM = "内存 : ";
        const string STR_LINE_END = "\n";
        const string STR_PERCENT_SYMBOL = "%";
        const string STR_MEM_SYMBOL = " / ";
        StringJointer _fpsStringJointer = new StringJointer (40);
        #endregion

        #region region 上下左右键开启... 
        bool _isOpen = false;
        int _openStat = 0;
        float _openTime = 0f;
        float _openTotalTime = 5f;
        #endregion

        #region region 储存帧率等信息... 
        int _curFrame = 0;
        int _fpsShowFrame = 0;
        #if UNITY_ANDROID && !UNITY_EDITOR
        int _cpuShowFrame = 0;
        int _memShowFrame = 0;
        #endif
        internal int _fpsShowTotalFrame = 30;
        internal int _cpuShowTotalFrame = 1000;
        internal int _memShowTotalFrame = 1000;
        bool _isSlowFrame = false;
        internal bool _ignoreSlowFrame = true;
        #endregion

        int i;
        Vector2 _dragOffset = new Vector3(); 

        #region  region 储存帧率等信息... 
        internal bool _isRecordToFile = true;
        internal int _fpsRecordTotalFrame = 100;
        internal int _cpuRecordTotalFrame = 1000;
        internal int _memRecordTotalFrame = 1000;
        #if UNITY_ANDROID && !UNITY_EDITOR
        int _fpsRecordFrame = 0;
        int _cpuRecordFrame = 0;
        int _memRecordFrame = 0;

        /*
         * 二进制文件定义标记：
         * 
         * _productName         : id,1=11 len,1 str   
         * _openDate            : id,1=12 len,1 str   
         * _packageName         : id,1=13 len,1 str   
         * _appVersion          : id,1=14 len,1 str   
         * _activityName        : id,1=15 len,1 str   
         * 
         * _deviceModel         : id,1=1 len,1 str   
         * _deviceId            : id,1=2 len,1 str
         * _operatingSystem     : id,1=3 len,1 str
         * _systemMemorySize    : id,1=4 len,1 str
         * _cpuModel            : id,1=5 len,1 str
         * _cpuCount            : id,1=6 len,1 str
         * _cpuFrequency        : id,1=7 len,1 str
         * _gpuModel            : id,1=8 len,1 str
         * _gpuMemorySize       : id,1=9 len,1 str
         * 
         * fpsRecordTotalFrame : id,1=21 ,2=100
         * cpuRecordTotalFrame : id,1=22 ,2=1000
         * memRecordTotalFrame : id,1=23 ,2=1000
         * 
         * _fps                 : id,1=41 ,1=60
         * _cpuRate             : id,1=42 ,1=18
         * _appMemory           : id,1=43 ,2=265
         * _availMemory         : id,1=44 ,2=300
         * 
         * _SampleName          : id,1=50 len,1 str idx,1
         * _BeginSample         : id,1=51 idx,1
         * _EndSample           : id,1=52
         */ 
        FileStream fs = null;
        BinaryWriter bw = null;

        int _sampleNameInsertCount = 0;
        List<string> _sampleNameList = new List<string>();
        List<int> _sampleIdList = new List<int>();
        #endif
        #endregion

        #if UNITY_ANDROID && !UNITY_EDITOR
        string GetSaveFilePath()
        {
            DateTime nowTime = DateTime.Now;
            StringJointer sj = new StringJointer(100);
            sj.Append("/sdcard/OrbbecExFps/").Append(AndroidInfo.packageName).Append('_');

            sj.Append(nowTime.Year);
            sj.Append ('-');
            if (nowTime.Month >= 10)
            {
                sj.Append(nowTime.Month);
            }
            else
            {
                sj.Append('0').Append(nowTime.Month);
            }
            sj.Append ('-');
            if (nowTime.Day >= 10)
            {
                sj.Append(nowTime.Day);
            }
            else
            {
                sj.Append('0').Append(nowTime.Day);
            }

            sj.Append ('_');

            if (nowTime.Hour >= 10)
            {
                sj.Append(nowTime.Hour);
            }
            else
            {
                sj.Append('0').Append(nowTime.Hour);
            }

            if (nowTime.Minute >= 10)
            {
                sj.Append(nowTime.Minute);
            }
            else
            {
                sj.Append('0').Append(nowTime.Minute);
            }

            if (nowTime.Second >= 10)
            {
                sj.Append(nowTime.Second);
            }
            else
            {
                sj.Append('0').Append(nowTime.Second);
            }

            sj.Append(".fps");

            return sj.ToString();
        }
        #endif

        public void OnColorChange(Image img)
        {
            _fpsInfoText.color = _modelInfoText.color = img.color;
        }

        private void OnShowInfoValueChanged(int value)
        {
            switch (value)
            {
                case 0:
                    SetInfoMode (FpsLoggerInfoMode.None);
                    break;
                case 2:
                    SetInfoMode (FpsLoggerInfoMode.Detail);
                    break;
                case 1:
                default:
                    SetInfoMode (FpsLoggerInfoMode.Base);
                    break;
            }
        }

        public void SetInfoMode(FpsLoggerInfoMode infoMode)
        {
            _infoMode = infoMode;

            if (!_isOpen)
            {
                return;
            }

            switch (infoMode)
            {
                case FpsLoggerInfoMode.Base:
                    _fpsInfoText.gameObject.SetActive (true);
                    _modelInfoText.gameObject.SetActive (false);
                    for (i = 0; i < _colorButtons.Count; i++)
                    {
                        _colorButtons [i].gameObject.SetActive (true);
                    }

                    if (_showFpsWidth <= 0.0f)
                    {
                        _showFpsWidth = Mathf.Max (170f, _fpsInfoText.preferredWidth + 20f);
                        _showFpsHeight = 120 + _fpsInfoText.preferredHeight + 10f;
                    }

//                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _fpsLoggerPanel.sizeDelta = new Vector2 (_showFpsWidth, _showFpsHeight);
//                    #else
//                    _fpsLoggerPanel.sizeDelta = new Vector2 (_showFpsWidth, 150.0f);
//                    #endif

                    RefreshFps ();

                    break;
                case FpsLoggerInfoMode.Detail:
                    _fpsInfoText.gameObject.SetActive (true);
                    _modelInfoText.gameObject.SetActive (true);
                    for (i = 0; i < _colorButtons.Count; i++)
                    {
                        _colorButtons [i].gameObject.SetActive (true);
                    }

                    if (_showFpsWidth <= 0.0f)
                    {
                        _showFpsWidth = Mathf.Max (170f, _fpsInfoText.preferredWidth + 20f);
                        _showFpsHeight = 120 + _fpsInfoText.preferredHeight + 10f;
                    }
                    if (_showModelWidth <= 0.0f)
                    {
                        _modelInfoText.text = "应用名 : " + _productName + STR_LINE_END
                        + "记录时间 : " + _openDate + STR_LINE_END
                        + "包名 : " + _packageName + STR_LINE_END
                            #if UNITY_ANDROID && !UNITY_EDITOR
                        + "Actitity : " + _activityName + STR_LINE_END
                            #endif
                        + "版本号 : " + _appVersion + STR_LINE_END
                        + "设备型号 : " + _deviceModel + STR_LINE_END
                        + "设备ID : " + _deviceId + STR_LINE_END
                        + "操作系统 : " + _operatingSystem + STR_LINE_END
                        + "系统内存 : " + _systemMemorySize + STR_LINE_END
                        + "CPU型号 : " + _cpuModel + STR_LINE_END
                        + "CPU核数 : " + _cpuCount + STR_LINE_END
                        + "CPU频率 : " + _cpuFrequency + STR_LINE_END
                        + "GPU型号 : " + _gpuModel + STR_LINE_END
                        + "GPU频率 : " + _gpuMemorySize;

                        _showModelWidth = Mathf.Max (_showFpsWidth, _modelInfoText.preferredWidth + 20f);
                        _showModelHeight = _showFpsHeight + _modelInfoText.preferredHeight;
                    }

//                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _modelInfoText.rectTransform.anchoredPosition = new Vector2(10f,-_showFpsHeight + 10);
                    _fpsLoggerPanel.sizeDelta = new Vector2 (_showModelWidth, _showModelHeight);
//                    #else
//                    _modelInfoText.rectTransform.anchoredPosition = new Vector2(10f,-150f);
//                    _fpsLoggerPanel.sizeDelta = new Vector2 (_showModelWidth, 415.0f);
//                    #endif

                    RefreshFps ();

                    break;
                case FpsLoggerInfoMode.None:
                default:
                    _fpsInfoText.gameObject.SetActive (false);
                    _modelInfoText.gameObject.SetActive (false);
                    for (i = 0; i < _colorButtons.Count; i++)
                    {
                        _colorButtons [i].gameObject.SetActive (false);
                    }

                    _fpsLoggerPanel.sizeDelta = new Vector2 (170.0f, 88.0f);

                    break;
            }

            _isSlowFrame = true;
        }

        void RefreshFps()
        {
            if (_infoMode != FpsLoggerInfoMode.None)
            {
                _fpsStringJointer.Clear ();
                #if UNITY_ANDROID && !UNITY_EDITOR
                _fpsStringJointer.Append (STR_FPS).Append (_fps).Append (STR_LINE_END);
                _fpsStringJointer.Append (STR_CPU).Append (_cpuRate).Append (STR_PERCENT_SYMBOL).Append (STR_LINE_END);
                _fpsStringJointer.Append (STR_MEM).Append (_appMemory).Append (STR_MEM_SYMBOL).Append (_availMemory);
                #else
                _fpsStringJointer.Append (STR_FPS).Append (_fps);
                #endif
                _fpsInfoText.text = _fpsStringJointer.stringValue;
                _fpsInfoText.FontTextureChanged ();
            }
        }

        void Update ()
        {
            _curFrame++;

            if (_ignoreSlowFrame && _isSlowFrame)
            {
                // 当上一帧受到Debugger影响比较慢时，不记录上一帧的数据
                _isSlowFrame = false;
                return;
            }

            if (!_isOpen)
            {
                // 上下左右打开界面
                CheckOpen ();
            }
            else
            {
                // 正常的记录逻辑
                RecordInfo();
            }
        }

        void CheckOpen()
        {
            if (_openTime >= _openTotalTime)
            {
                _openStat = 0;
            }
            else
            {
                _openTime += Time.unscaledDeltaTime;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_openStat == 1)
                {
                    _openStat = 2;
                }
                else
                {
                    _openStat = 1;
                    _openTime = 0f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _openStat = (_openStat == 2) ? 3 : (_openStat == 3) ? 4 : 0;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _openStat = (_openStat == 4) ? 5 : (_openStat == 5) ? 6 : 0;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _openStat = (_openStat == 6) ? 7 : (_openStat == 7) ? 8 : 0;
                if (_openStat == 8)
                {
                    OpenLogger (_infoMode, true);
                }
            }
        }

        public void OpenLogger(FpsLoggerInfoMode infoMode, bool isRecordToFile)
        {
            if (_isOpen)
                return;
            
            _isOpen = true;

            _isRecordToFile = isRecordToFile;

            _fpsLoggerRoot.gameObject.SetActive (true);
            _fpsLoggerRootCanvas.sortingOrder = _sortOrder;

            RecordInfoFirst ();

            SetInfoMode (infoMode);
            switch (infoMode)
            {
                case FpsLoggerInfoMode.None:
                    _showInfoDropDown.value = 0;
                    break;
                case FpsLoggerInfoMode.Detail:
                    _showInfoDropDown.value = 2;
                    break;
                case FpsLoggerInfoMode.Base:
                default:
                    _showInfoDropDown.value = 1;
                    break;
            }
            _showInfoDropDown.onValueChanged.AddListener (OnShowInfoValueChanged);
        }

        void RecordInfoFirst()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidInfo.AddCacheMethod (AndroidInfo.CacheMethod.getAppMemory);
            AndroidInfo.AddCacheMethod (AndroidInfo.CacheMethod.getAvailMemory);
            AndroidInfo.AddCacheMethod (AndroidInfo.CacheMethod.getCpuRate);
            #endif

            // 记录deviceInfo

            _deviceModel = ApplicationInfo.deviceModel;
            _deviceId = ApplicationInfo.deviceId;
            _operatingSystem = ApplicationInfo.operatingSystem;
            _systemMemorySize = ApplicationInfo.systemMemorySize + "M";
            _cpuModel = ApplicationInfo.cpuModel;
            _cpuCount = ApplicationInfo.cpuCount + "";
            _cpuFrequency = ApplicationInfo.cpuFrequency + "KHz";
            _gpuModel = ApplicationInfo.gpuModel;
            _gpuMemorySize = ApplicationInfo.gpuMemorySize + "M";

            //新增应用名，时间，包名/Activity名，versionName，versionCode，
            _productName = ApplicationInfo.productName;
            _openDate = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
            _packageName = ApplicationInfo.packageName;
            #if UNITY_ANDROID && !UNITY_EDITOR
            _activityName = AndroidInfo.activityName;
            _appVersion = ApplicationInfo.appVersion + " / " + AndroidInfo.versionCode;
            #else
            _appVersion = ApplicationInfo.appVersion;
            #endif

            // 记录fps
            _fpsDeltaTime = Time.unscaledDeltaTime;
            _fps = (int)(1.0f / _fpsDeltaTime);

            #if UNITY_ANDROID && !UNITY_EDITOR
            _cpuRate = AndroidInfo.cpuRate;
            _appMemory = AndroidInfo.appMemory;
            _availMemory = AndroidInfo.availMemory;
            #endif

            RefreshFps ();

            #region region 首次存储Android文件
            #if UNITY_ANDROID && !UNITY_EDITOR
            if (_isRecordToFile)
            {
                string saveFilePath = GetSaveFilePath();
                string dirName = Path.GetDirectoryName (saveFilePath);
                try
                {
                    if (!Directory.Exists (dirName))
                    {
                        Directory.CreateDirectory (dirName);
                    }

                    byte[] byteStr;
                    fs = new FileStream(saveFilePath,FileMode.Append,FileAccess.Write);
                    bw = new BinaryWriter(fs);

                    // 记录deviceInfo

                    bw.Write((byte)11);
                    byteStr = Encoding.UTF8.GetBytes(_productName);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)12);
                    byteStr = Encoding.UTF8.GetBytes(_openDate);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)13);
                    byteStr = Encoding.UTF8.GetBytes(_packageName);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)14);
                    byteStr = Encoding.UTF8.GetBytes(_appVersion);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)15);
                    byteStr = Encoding.UTF8.GetBytes(_activityName);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)1);
                    byteStr = Encoding.UTF8.GetBytes(_deviceModel);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)2);
                    byteStr = Encoding.UTF8.GetBytes(_deviceId);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)3);
                    byteStr = Encoding.UTF8.GetBytes(_operatingSystem);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)4);
                    byteStr = Encoding.UTF8.GetBytes(_systemMemorySize);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)5);
                    byteStr = Encoding.UTF8.GetBytes(_cpuModel);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)6);
                    byteStr = Encoding.UTF8.GetBytes(_cpuCount);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)7);
                    byteStr = Encoding.UTF8.GetBytes(_cpuFrequency);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)8);
                    byteStr = Encoding.UTF8.GetBytes(_gpuModel);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    bw.Write((byte)9);
                    byteStr = Encoding.UTF8.GetBytes(_gpuMemorySize);
                    bw.Write((byte)byteStr.Length);
                    bw.Write(byteStr);

                    // 记录_fpsRecordFrame\_cpuRecordFrame\_memRecordFrame

                    bw.Write((byte)21);
                    bw.Write((ushort)_fpsRecordTotalFrame);
                    bw.Write((byte)22);
                    bw.Write((ushort)_cpuRecordTotalFrame);
                    bw.Write((byte)23);
                    bw.Write((ushort)_memRecordTotalFrame);

                    // 记录第一次fps、cpu等

                    bw.Write((byte)41);
                    bw.Write((byte)_fps);
                    bw.Write((byte)42);
                    bw.Write((byte)_cpuRate);
                    bw.Write((byte)43);
                    bw.Write((ushort)_appMemory);
                    bw.Write((byte)44);
                    bw.Write((ushort)_availMemory);

                    bw.Flush();
    //                bw.Close();
    //                fs.Close();

                }
                catch (System.Exception ex)
                {
                    Debug.LogError ("Debugger SaveFile error : " + dirName + " --> " + ex.Message);
                    CloseFileStream();
                }
            }
            #endif
            #endregion

            _isSlowFrame = true;
        }

        void RecordInfo()
        {
            _fpsShowFrame++;
            _fpsDeltaTime += (Time.unscaledDeltaTime - _fpsDeltaTime) * 0.1f;
            _fps = (int)(1.0f / _fpsDeltaTime);

            #if UNITY_ANDROID && !UNITY_EDITOR
            _cpuShowFrame++;
            if (_cpuShowFrame >= _cpuShowTotalFrame)
            {
                _cpuRate = AndroidInfo.cpuRate;
                _isSlowFrame = true;
            }

            _memShowFrame++;
            if (_memShowFrame >= _memShowTotalFrame)
            {
                _appMemory = AndroidInfo.appMemory;
                _availMemory = AndroidInfo.availMemory;
                _isSlowFrame = true;
            }
            #endif

            #if UNITY_ANDROID && !UNITY_EDITOR
            if (_fpsShowFrame >= _fpsShowTotalFrame  || _cpuShowFrame >= _cpuShowTotalFrame || _memShowFrame >= _memShowTotalFrame)
            #else
            if (_fpsShowFrame >= _fpsShowTotalFrame)
            #endif
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                if (_showFps != _fps || _showCpuRate != _cpuRate || _showAppMemory != _appMemory || _showAvailMemory != _availMemory)
                #else
                if (_showFps != _fps)
                #endif
                {
                    _showFps = _fps;
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _showCpuRate = _cpuRate;
                    _showAppMemory = _appMemory;
                    _showAvailMemory = _availMemory;
                    #endif

                    RefreshFps ();

                    _isSlowFrame = true;
                }

                if (_fpsShowFrame >= _fpsShowTotalFrame)
                {
                    _fpsShowFrame = 0;
                }
                #if UNITY_ANDROID && !UNITY_EDITOR
                if (_cpuShowFrame >= _cpuShowTotalFrame)
                {
                    _cpuShowFrame = 0;
                }
                if (_memShowFrame >= _memShowTotalFrame)
                {
                    _memShowFrame = 0;
                }
                #endif
            }

            #if UNITY_ANDROID && !UNITY_EDITOR

            int scampleCount;

            if (_isRecordToFile)
            {
                scampleCount = _sampleNameList.Count;
                if (scampleCount > _sampleNameInsertCount)
                {
                    for (i = _sampleNameInsertCount; i < scampleCount; i++)
                    {
                        if (bw != null)
                        {
                            byte[] byteStr;
                            bw.Write ((byte)50);
                            byteStr = Encoding.UTF8.GetBytes (_sampleNameList[i]);
                            bw.Write ((byte)byteStr.Length);
                            bw.Write (byteStr);
                            bw.Write ((byte)i);
                            bw.Flush ();
                            _isSlowFrame = true;
                        }
                    }
                    _sampleNameInsertCount = scampleCount;
                }
            }

            scampleCount = _sampleIdList.Count;
            if (scampleCount>0)
            {
                if (_isRecordToFile)
                {
                    for (i = 0; i < scampleCount; i++)
                    {
                        if (_sampleIdList [i] != -1)
                        {
                            if (bw != null)
                            {
                                bw.Write ((byte)51);
                                bw.Write ((byte)_sampleIdList[i]);
                                bw.Flush ();
                                _isSlowFrame = true;
                            }
                        }
                        else
                        {
                            if (bw != null)
                            {
                                bw.Write ((byte)52);
                                bw.Flush ();
                                _isSlowFrame = true;
                            }
                        }
                    }
                }

                _sampleIdList.Clear();
            }

            if (_isRecordToFile)
            {
                _fpsRecordFrame++;
                if (_fpsRecordFrame >= _fpsRecordTotalFrame)
                {
                    _fpsRecordFrame = 0;
                    // 记录fps到文件
                    if (bw != null)
                    {
                        bw.Write((byte)41);
                        bw.Write((byte)_fps);
                        bw.Flush();
                    }
                    _isSlowFrame = true;
                }

                _cpuRecordFrame++;
                if (_cpuRecordFrame >= _cpuRecordTotalFrame)
                {
                    _cpuRecordFrame = 0;
                    // 记录cpu到文件
                    if (bw != null)
                    {
                        bw.Write((byte)42);
                        bw.Write((byte)_cpuRate);
                        bw.Flush();
                    }
                    _isSlowFrame = true;
                }

                _memRecordFrame++;
                if (_memRecordFrame >= _memRecordTotalFrame)
                {
                    _memRecordFrame = 0;
                    // 记录内存到文件
                    if (bw != null)
                    {
                        bw.Write((byte)43);
                        bw.Write((ushort)_appMemory);
                        bw.Write((byte)44);
                        bw.Write((ushort)_availMemory);
                        bw.Flush();
                    }
                    _isSlowFrame = true;
                }
            }
            #endif
        }


        //当鼠标按下时调用 接口对应  IPointerDownHandler
        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 mouseDown = eventData.position;    //记录鼠标按下时的屏幕坐标
            Vector2 uguiPos = new Vector2();   //定义一个接收返回的ugui坐标
            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(_fpsLoggerRoot, mouseDown, eventData.enterEventCamera, out uguiPos);
            if (isRect)   //如果在
            {
                //计算图片中心和鼠标点的差值
                _dragOffset = _fpsLoggerPanel.anchoredPosition - uguiPos;
            }
        }

        //当鼠标拖动时调用   对应接口 IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 mouseDrag = eventData.position;   //当鼠标拖动时的屏幕坐标
            Vector2 uguiPos = new Vector2();   //用来接收转换后的拖动坐标
            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(_fpsLoggerRoot, mouseDrag, eventData.enterEventCamera, out uguiPos);
            if (isRect && _dragOffset != Vector2.zero)
            {
                //设置图片的ugui坐标与鼠标的ugui坐标保持不变
                _fpsLoggerPanel.anchoredPosition = _dragOffset + uguiPos;
            }
        }

        //当鼠标抬起时调用  对应接口  IPointerUpHandler
        public void OnPointerUp(PointerEventData eventData)
        {
            _dragOffset = Vector2.zero;
        }

        //当鼠标结束拖动时调用   对应接口  IEndDragHandler
        public void OnEndDrag(PointerEventData eventData)
        {
            _dragOffset = Vector2.zero;
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
        void CloseFileStream()
        {
            if (bw != null)
            {
                bw.Close ();
                bw = null;
            }

            if (fs != null)
            {
                fs.Close ();
                fs = null;
            }
        }
        #endif

        void OnDestroy()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            CloseFileStream ();
            #endif
        }

        public void BeginSample(string name)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            if (!_sampleNameList.Contains(name))
            {
                _sampleNameList.Add (name);
            }
            _sampleIdList.Add(_sampleNameList.IndexOf(name));
            #endif
        }

        public void EndSample()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            if (_sampleIdList.Count == 0)
            {
                _sampleIdList.Add (-1);
            }
            else
            {
                if (_sampleIdList [_sampleIdList.Count - 1] == -1)
                {
                    _sampleIdList.Add (-1);
                }
                else
                {
                    _sampleIdList.RemoveAt (_sampleIdList.Count - 1);
                }
            }
            #endif

        }
    }
}

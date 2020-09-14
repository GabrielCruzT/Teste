using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using Astra;

namespace Astra.Simple
{
    /// <summary>
    /// 可开关的流类型.
    /// </summary>
    public enum StreamType
    {
        Depth = 0x1,
        Color = 0x2,
        Infrared = 0x4,
        MaskedColor = 0x8,
        ColorizedBody = 0x10,
        Body = 0x20,
    }

    /// <summary>
    /// 纹理类型.
    /// </summary>
    public enum TextureType
    {
        Depth = 0x1,
        Color = 0x2,
        Infrared = 0x4,
        MaskedColor = 0x8,
        ColorizedBody = 0x10,
    }

    /// <summary>
    /// 流管理器.
    /// </summary>
    public class StreamManager : MonoBehaviour
    {
        private string _license = null/*"oJ2yhcJmF7DNYxes5BFZd40QmU919j4ftTlEj71pp3u0pLz44flgzpbU1yFCuOCsgFBXq0LSSj+Hg+fxQlkBDU5hbWU9T1JCQkVDfE9yZz1PcmJiZWN8Q29tbWVudD17Y29tbWVudDosY3VzdG9tZXJfdHlwZTpvcmRpbmFyeSxsaWNlbnNlX3R5cGU6b2ZmbGluZSx2ZXJzaW9uOjF9fEV4cGlyYXRpb249MTU5MDg4OTU2MA=="*/;

        #region PublicInterface相关的变量

        private StreamData _streamData;

        private Action _initSuccessCallback = null;
        private Action _initFailureCallback = null;

        private Astra.BodyTrackingFeatures _defaultBodyFeatures = Astra.BodyTrackingFeatures.Skeleton;
        private Astra.SkeletonOptimization _skeletonOptimization = Astra.SkeletonOptimization.Balanced;
        private Astra.SkeletonProfile _skeletonProfile = Astra.SkeletonProfile.Full;

        private ImageMode _depthMode = null;
        private ImageMode _colorMode = null;
        private ImageMode _infraredMode = null;

        #endregion

        void Awake()
        {
            _streamData = new StreamData(this);
        }

        #region StreamManager Public Interface...

        /// <summary>
        /// 初始化体感器.
        /// </summary>
        public void Initialize()
        {
            _Initialize ();
        }

        /// <summary>
        /// 终止体感器.
        /// </summary>
        public void Terminate()
        {
            if (_initializing || _initialized)
            {
                _WaitForUpdate (waitIndefinitely);
                _Terminate ();
            }
        }

        /// <summary>
        /// 设置初始化成功的回调.
        /// </summary>
        /// <param name="action">Action.</param>
        public void SetInitSuccessCallback (Action action)
        {
            _initSuccessCallback = action;
        }

        /// <summary>
        /// 设置初始化失败的回调.
        /// </summary>
        /// <param name="action">Action.</param>
        public void SetInitFailureCallback (Action action)
        {
            _initFailureCallback = action;
        }

        /// <summary>
        /// 设置深度图分辨率模式.需初始化体感器成功后通过GetAvailableDepthModes获取列表.
        /// </summary>
        /// <param name="mode">Mode.</param>
        public void SetDepthMode(Astra.ImageMode mode)
        {
            _depthMode = mode;

            if (_isDepthOpened)
            {
                _WaitForUpdate (waitIndefinitely);

                if (_depthMode == null)
                {
                    _depthMode = _depthStream.AvailableModes [0];
                }
                _depthStream.SetMode (_depthMode);
            }
        }

        /// <summary>
        /// 返回当前设置的深度分辨率模式.
        /// </summary>
        /// <returns>The depth mode.</returns>
        public Astra.ImageMode GetDepthMode()
        {
            return _depthMode;
        }

        /// <summary>
        /// 设置彩色图分辨率模式.需初始化体感器成功后通过GetAvailableColorModes获取列表.
        /// </summary>
        /// <param name="mode">Mode.</param>
        public void SetColorMode(Astra.ImageMode mode)
        {
            _colorMode = mode;

            if (_isColorOpened)
            {
                _WaitForUpdate (waitIndefinitely);

                if (_colorMode == null)
                {
                    _colorMode = _colorStream.AvailableModes [0];
                }
                _colorStream.SetMode (_colorMode);
            }
        }


        /// <summary>
        /// 返回当前设置的彩色图分辨率模式.
        /// </summary>
        /// <returns>The color mode.</returns>
        public Astra.ImageMode GetColorMode()
        {
            return _colorMode;
        }

        /// <summary>
        /// 设置IR图分辨率模式.需初始化体感器成功后通过GetAvailableInfraredModes获取列表.
        /// </summary>
        /// <param name="mode">Mode.</param>
        public void SetInfraredMode(Astra.ImageMode mode)
        {
            _infraredMode = mode;

            if (_isInfraredOpened)
            {
                _WaitForUpdate(waitIndefinitely);

                if (_infraredMode == null)
                {
                    _infraredMode = _infraredStream.AvailableModes[0];
                }
                _infraredStream.SetMode(_infraredMode);
            }
        }

        /// <summary>
        /// 返回当前设置的IR图分辨率模式.
        /// </summary>
        /// <returns>The infrared mode.</returns>
        public Astra.ImageMode GetInfraredMode()
        {
            return _infraredMode;
        }

        /// <summary>
        /// 获取可用的深度图分辨率模式列表.需初始化体感器成功后调用才有效.
        /// </summary>
        /// <returns>The available depth modes.</returns>
        public Astra.ImageMode[] GetAvailableDepthModes()
        {
            if (_initialized)
            {
                return _depthStream.AvailableModes;
            }
            else
            {
                return null;
            }
        }
    
        /// <summary>
        /// 获取可用的彩色图分辨率模式列表.需初始化体感器成功后调用才有效.
        /// </summary>
        /// <returns>The available color modes.</returns>
        public Astra.ImageMode[] GetAvailableColorModes()
        {
            if (_initialized)
            {
                return _colorStream.AvailableModes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取可用的IR图分辨率模式列表.需初始化体感器成功后调用才有效.
        /// </summary>
        /// <returns>The available infrared modes.</returns>
        public Astra.ImageMode[] GetAvailableInfraredModes()
        {
            if (_initialized)
            {
                return _infraredStream.AvailableModes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 打开一个数据流
        /// </summary>
        /// <param name="type">数据流类型.</param>
        public void OpenStream(StreamType type)
        {
            if ((int)(type & StreamType.Depth) != 0)
            {
                _openDepth = true;
                _streamData.UpdateDepthStreamData(null, false);
            }

            if ((int)(type & StreamType.Color) != 0)
            {
                _openColor = true;
                _streamData.UpdateColorStreamData(null, false);
            }

            if ((int)(type & StreamType.Infrared) != 0)
            {
                _openInfrared = true;
                _streamData.UpdateInfraredStreamData(null, false);
            }

            if ((int)(type & StreamType.MaskedColor) != 0)
            {
                _openMaskedColor = true;
                _streamData.UpdateMaskedColorStreamData(null, false);
            }

            if ((int)(type & StreamType.ColorizedBody) != 0)
            {
                _openColorizedBody = true;
                _streamData.UpdateColorizedBodyStreamData(null, false);
            }

            if ((int)(type & StreamType.Body) != 0)
            {
                _openBody = true;
                _streamData.UpdateBodyStreamData(null);
            }
        }

        /// <summary>
        /// 关闭一个数据流.
        /// </summary>
        /// <param name="type">数据流类型.</param>
        public void CloseStream(StreamType type)
        {
            if ((int)(type & StreamType.Depth) != 0)
            {
                _openDepth = false;
                _streamData.UpdateDepthStreamData(null, false);
            }

            if ((int)(type & StreamType.Color) != 0)
            {
                _openColor = false;
                _streamData.UpdateColorStreamData(null, false);
            }

            if ((int)(type & StreamType.Infrared) != 0)
            {
                _openInfrared = false;
                _streamData.UpdateInfraredStreamData(null, false);
            }

            if ((int)(type & StreamType.MaskedColor) != 0)
            {
                _openMaskedColor = false;
                _streamData.UpdateMaskedColorStreamData(null, false);
            }

            if ((int)(type & StreamType.ColorizedBody) != 0)
            {
                _openColorizedBody = false;
                _streamData.UpdateColorizedBodyStreamData(null, false);
            }

            if ((int)(type & StreamType.Body) != 0)
            {
                _openBody = false;
                _streamData.UpdateBodyStreamData(null);
            }
        }

        /// <summary>
        /// 判断该数据流是否已开启.
        /// </summary>
        /// <returns><c>true</c> if this instance is stream opend the specified type; otherwise, <c>false</c>.</returns>
        /// <param name="type">数据流.</param>
        public bool IsStreamOpend(StreamType type)
        {
            bool ret = true;
            while (true)
            {
                if ((int)(type & StreamType.Depth) != 0 && !_openDepth)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & StreamType.Color) != 0 && !_openColor)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & StreamType.Infrared) != 0 && !_openInfrared)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & StreamType.MaskedColor) != 0 && !_openMaskedColor)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & StreamType.ColorizedBody) != 0 && !_openColorizedBody)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & StreamType.Body) != 0 && !_openBody)
                {
                    ret = false;
                    break;
                }

                break;
            }
            return ret;
        }

        /// <summary>
        /// 是否开启自动刷新纹理.开启后StreamData中的texture将自动刷新.
        /// </summary>
        /// <param name="type">纹理类型.</param>
        /// <param name="use">If set to <c>true</c> use.</param>
        public void UsingTexture(TextureType type, bool use)
        {
            if ((int)(type & TextureType.Depth) != 0)
            {
                _usingDepthTexture = use;
            }

            if ((int)(type & TextureType.Color) != 0)
            {
                _usingColorTexture = use;
            }

            if ((int)(type & TextureType.Infrared) != 0)
            {
                _usingInfraredTexture = use;
            }

            if ((int)(type & TextureType.MaskedColor) != 0)
            {
                _usingMaskedColorTexture = use;
            }

            if ((int)(type & TextureType.ColorizedBody) != 0)
            {
                _usingColorizedBodyTexture = use;
            }
        }

        /// <summary>
        /// 判断当前纹理类型是否已开启自动刷新.
        /// </summary>
        /// <returns><c>true</c> if this instance is using texture the specified type; otherwise, <c>false</c>.</returns>
        /// <param name="type">纹理类型.</param>
        public bool IsUsingTexture(TextureType type)
        {
            bool ret = true;
            while (true)
            {
                if ((int)(type & TextureType.Depth) != 0 && !_usingDepthTexture)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & TextureType.Color) != 0 && !_usingColorTexture)
                {
                    ret = false;
                    break;
                }

                if((int)(type & TextureType.Infrared) != 0 && !_usingInfraredTexture)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & TextureType.MaskedColor) != 0 && !_usingMaskedColorTexture)
                {
                    ret = false;
                    break;
                }

                if ((int)(type & TextureType.ColorizedBody) != 0 && !_usingColorizedBodyTexture)
                {
                    ret = false;
                    break;
                }

                break;
            }
            return ret;
        }

        /// <summary>
        /// 设置body特征.  人体分割/ 人体分割+骨架 / 人体分割+骨架+手抓握状态.
        /// </summary>
        /// <param name="bodyId">Body identifier.</param>
        /// <param name="features">Features.</param>
        public void SetBodyFeatures(byte bodyId, Astra.BodyTrackingFeatures features)
        {
            if (_isBodyOpened)
            {
                _WaitForUpdate(waitIndefinitely);
                _bodyStream.SetBodyFeatures (bodyId, features);
            }
        }

        /// <summary>
        /// 获取body特征. 人体分割/ 人体分割+骨架 / 人体分割+骨架+手抓握状态.
        /// </summary>
        /// <returns>The body features.</returns>
        /// <param name="bodyId">Body identifier.</param>
        public Astra.BodyTrackingFeatures GetBodyFeatures(byte bodyId)
        {
            if (_isBodyOpened)
            {
                _WaitForUpdate(waitIndefinitely);
                _bodyStream.GetBodyFeatures (bodyId);
            }
            return Astra.BodyTrackingFeatures.Segmentation;
        }

        /// <summary>
        /// 设置默认body特征. 人体分割/ 人体分割+骨架 / 人体分割+骨架+手抓握状态.
        /// </summary>
        /// <param name="features">Features.</param>
        public void SetDefaultBodyFeatures(Astra.BodyTrackingFeatures features)
        {
            _defaultBodyFeatures = features;

            if (_isBodyOpened)
            {
                _WaitForUpdate(waitIndefinitely);
                _bodyStream.SetDefaultBodyFeatures (_defaultBodyFeatures);
            }
        }

        /// <summary>
        /// 获取默认body特征. 人体分割/ 人体分割+骨架 / 人体分割+骨架+手抓握状态.
        /// </summary>
        /// <returns>The default body features.</returns>
        public Astra.BodyTrackingFeatures GetDefaultBodyFeatures()
        {
            return _defaultBodyFeatures;
        }

        /// <summary>
        /// 设置骨架计算精度.精度越高对CPU要求越高.
        /// </summary>
        /// <param name="optimization">Optimization.</param>
        public void SetSkeletonOptimization(Astra.SkeletonOptimization optimization)
        {
            _skeletonOptimization = optimization;

            if (_isBodyOpened)
            {
                _WaitForUpdate(waitIndefinitely);
                _bodyStream.SetSkeletonOptimization (_skeletonOptimization);
            }
        }

        /// <summary>
        /// 获取骨架计算精度.
        /// </summary>
        /// <returns>The skeleton optimization.</returns>
        public Astra.SkeletonOptimization GetSkeletonOptimization()
        {
            return _skeletonOptimization;
        }

        /// <summary>
        /// 设置骨架轮廓. 全身模式 / 4点模式 / 上半身模式.
        /// </summary>
        /// <param name="profile">Profile.</param>
        public void SetSkeletonProfile(Astra.SkeletonProfile profile)
        {
            _skeletonProfile = profile;

            if (_isBodyOpened)
            {
                _WaitForUpdate(waitIndefinitely);
                _bodyStream.SetSkeletonProfile (_skeletonProfile);
            }
        }

        /// <summary>
        /// 获取骨架轮廓. 全身模式 / 4点模式 / 上半身模式.
        /// </summary>
        /// <returns>The skeleton profile.</returns>
        public Astra.SkeletonProfile GetSkeletonProfile()
        {
            return _skeletonProfile;
        }

        /// <summary>
        /// 获取包含所有流数据的入口.
        /// </summary>
        /// <returns>The stream data.</returns>
        public StreamData GetStreamData()
        {
            return _streamData;
        }

        /// <summary>
        /// 是否已成功的初始化完成.
        /// </summary>
        /// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
        public bool isInitialized
        {
            get 
            {
                return _initialized;
            }
        }

        /// <summary>
        /// 设置序列号，需要在初始化之前调用
        /// </summary>
        /// <param name="licenseString">License string.</param>
        public void SetLicense(String licenseString) 
        {
            _license = licenseString;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// 设置要使用的UVC摄像头.
        /// </summary>
        /// <param name="index">摄像头索引</param>
        public void SetCamera(int index)
        {
            Debug.Log("Begin Set camera index:" + index);
            if (javaActivity == null)
            {
                javaClass = new AndroidJavaClass("com.orbbec.astra.android.unity3d.AstraUnityPlayerActivity");
                javaActivity = javaClass.GetStatic<AndroidJavaObject>("Instance");
            }
            javaActivity.Call<bool>("setCamera", index);
            Debug.Log("End Set camera index:" + index);
        }
#endif

        #endregion



        #region 流线程相关的变量

        // background updater
        private const int waitIndefinitely = -1;

        private Thread _streamThread;
        private volatile bool _isStreamThreadStarted = false;
        private Mutex _streamUpdateMutex = new Mutex();
        private AutoResetEvent _updateRequestedEvent = new AutoResetEvent(false);
        private AutoResetEvent _updateCompletedEvent = new AutoResetEvent(false);
        private bool _updateRequested = false;
        private bool _updateCompleted = false;

        private bool _initialized = false;
        private bool _initializing = false;

        #if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass javaClass;
        private static AndroidJavaObject javaActivity;
        #endif

        #endregion

        #region 流数据相关的变量

        private Astra.StreamSet _streamSet;
        private Astra.StreamReader _depthStreamReader;
        private Astra.StreamReader _colorStreamReader;
        private Astra.StreamReader _infraredStreamReader;
        private Astra.StreamReader _maskedColorStreamReader;
        private Astra.StreamReader _colorizedBodyStreamReader;
        private Astra.StreamReader _bodyStreamReader;

        private DepthStream _depthStream;
        private ColorStream _colorStream;
        private InfraredStream _infraredStream;
        private MaskedColorStream _maskedColorStream;
        private ColorizedBodyStream _colorizedBodyStream;
        private BodyStream _bodyStream;

        private bool _openDepth = false;
        private bool _openColor = false;
        private bool _openInfrared = false;
        private bool _openMaskedColor = false;
        private bool _openColorizedBody = false;
        private bool _openBody = false;

        private bool _isDepthOpened = false;
        private bool _isColorOpened = false;
        private bool _isInfraredOpened = false;
        private bool _isMaskedColorOpened = false;
        private bool _isColorizedBodyOpened = false;
        private bool _isBodyOpened = false;

        private long _lastDepthFrameIndex = -1;
        private long _lastColorFrameIndex = -1;
        private long _lastInfraredFrameIndex = -1;
        private long _lastMaskedColorFrameIndex = -1;
        private long _lastColorizedBodyFrameIndex = -1;
        private long _lastBodyFrameIndex = -1;

        private bool _usingDepthTexture = false;
        private bool _usingColorTexture = false;
        private bool _usingInfraredTexture = false;
        private bool _usingMaskedColorTexture = false;
        private bool _usingColorizedBodyTexture = false;
        private byte[] _depthTextureBuffer = null;

        private int _frameCount = 0;

        #endregion

        #region Private...

        // 初始化 体感器
        private void _Initialize()
        {
            // 已经初始化过了，那么不再重复初始化。
            if (_initialized)
            {
                Debug.Log("StreamManager previously initialized");
                return;
            }

            // 正在初始化中，不再重复初始化。
            if (_initializing)
                return;
            _initializing = true;

            Debug.Log("StreamManager initializing.");

            _updateRequested = false;
            _updateCompleted = false;

            // 初始化 体感器Context
            Context.Initialize();

            // 获取usb授权
            #if UNITY_ANDROID && !UNITY_EDITOR
            _RequestUsbDeviceAccessFromAndroid();
            #endif

            // 初始化 StreamData
//            _streamData = new StreamData();

            // 初始化 流数据 直到能获取到深度彩色分辨率mode
//            _InitializeFirstStreams();

            // 启动 流工作线程
            _StartStreamThread();

            // 标记为已初始化
//            _initialized = true;
//            _initializing = false;
//
//            if (_initSuccessCallback != null)
//            {
//                _initSuccessCallback ();
//            }

            // 接下来尝试获取深度/彩色ImageMode列表，当能获取到时，才是真正初始化成功。
        }

        // 销毁 体感器
        private void _Terminate()
        {
            // 未初始化，跳过。
            if (!_initializing && !_initialized)
                return;

            Debug.Log("StreamManager terminating.");

            // 释放 StreamData
//            _streamData = null;

            // 停止 流工作线程
            _StopStreamThread();

            // 销毁 体感器Context
            Context.Terminate();

            // 销毁 流数据
            _DisposeAllStreams();
            //            _CloseAllStreams();

            _updateRequested = false;
            _updateCompleted = false;

            // 标记为未初始化
            _initializing = false;
            _initialized = false;
        }

        // 启动 流工作线程
        private void _StartStreamThread()
        {
            // 已启动，先关闭
            if (_isStreamThreadStarted)
            {
                _StopStreamThread();
            }

            // 标记为已启动
            _isStreamThreadStarted = true;

            // 创建新线程
            _streamThread = new Thread(_StreamThreadFunc);

            // 启动线程
            _streamThread.Start();
        }

        // 停止 流工作线程
        private void _StopStreamThread()
        {
            // 未启动，则无视。
            if (!_isStreamThreadStarted)
            {
                return;
            }

            // 标记为未启动
            _isStreamThreadStarted = false;

            // 销毁当前线程
            if (_streamThread != null && _streamThread.ThreadState != ThreadState.Unstarted)
            {
                _streamThread.Join(TimeSpan.FromMilliseconds(1000));
            }
            _streamThread = null;
        }

        // 流工作线程运行函数
        private void _StreamThreadFunc()
        {
            // 当流线程标记为启动时，则循环获取数据。否则直接结束线程函数。
            while(_isStreamThreadStarted)
            {
                // 等待获取数据的信号（来自主线程的信号）。
                if (_updateRequestedEvent.WaitOne(100) && _isStreamThreadStarted)
                {
                    // 开启互斥锁
                    _streamUpdateMutex.WaitOne();

                    // 更新数据
                    Context.Update();

                    // 标记已更新完数据
                    _updateCompleted = true;

                    // 标记未开始获取信号
                    _updateRequested = false;

                    // 释放互斥锁
                    _streamUpdateMutex.ReleaseMutex();

                    // 发送更新完毕信号
                    _updateCompletedEvent.Set();
                }
            }
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
        // 获取USB授权
        private void _RequestUsbDeviceAccessFromAndroid()
        {
//            _Initialize();

            Debug.Log("Auto-requesting usb device access.");

            // 如正在更新数据，等待更新完毕。
            _WaitForUpdate (waitIndefinitely);
            
            // 获取Activity
            EnsureJavaActivity();
            
            // 打开设备
            Debug.Log("AstraUnityContext.RequestUsbDeviceAccessFromAndroid() calling openAllDevices");
            //TODO use AndroidJavaProxy to do a callback
            System.Object[] args = new System.Object[0];
            javaActivity.Call("openAllDevices", args);
            Debug.Log("AstraUnityContext.RequestUsbDeviceAccessFromAndroid() called openAllDevices");

            //TODO: only call this in the callback with the success/fail results
            //RaisePermissionRequestCompleted(true);
        }

        // 获取Activity
        private void EnsureJavaActivity()
        {
            if (javaActivity == null)
            {
                Debug.Log("AstraUnityContext.EnsureJavaActivity() Getting Java activity");
                javaClass = new AndroidJavaClass("com.orbbec.astra.android.unity3d.AstraUnityPlayerActivity");
                javaActivity = javaClass.GetStatic<AndroidJavaObject>("Instance");
                Debug.Log("AstraUnityContext.EnsureJavaActivity() Got Java activity");
            }
        }

        #endif

        // 等待更新完毕信号(来自数据刷新子线程).
        private bool _WaitForUpdate(int timeoutMilliseconds)
        {
            if (_updateCompleted) { return true; }

            if (!_updateRequested) { return true; }

            _updateCompletedEvent.WaitOne(timeoutMilliseconds);
            return _updateCompleted;
        }


        // 申请刷新流数据
        private void _UpdateAsync()
        {
            // 互斥锁开启
            _streamUpdateMutex.WaitOne();

            // 改变状态
            _updateCompleted = false;
            _updateRequested = true;

            // 互斥锁释放
            _streamUpdateMutex.ReleaseMutex();

            // 发送请求信号(主线程发给子线程，通知子线程开始刷新数据)
            _updateRequestedEvent.Set();
        }

        // 初始化环节二，检查是否能获取到ImageMode列表
        private void _InitializeAllStreams()
        {
            Debug.Log("StreamManager: _InitializeAllStreams");

            try
            {
                _WaitForUpdate(waitIndefinitely);

                if (_streamSet == null)
                {
                    _streamSet = Astra.StreamSet.Open();
                }
                _depthStreamReader = _streamSet.CreateReader();
                _depthStream = _depthStreamReader.GetStream<DepthStream>();
                _colorStreamReader = _streamSet.CreateReader();
                _colorStream = _colorStreamReader.GetStream<ColorStream>();
                _infraredStreamReader = _streamSet.CreateReader();
                _infraredStream = _infraredStreamReader.GetStream<InfraredStream>();
                _maskedColorStreamReader = _streamSet.CreateReader();
                _maskedColorStream = _maskedColorStreamReader.GetStream<MaskedColorStream>();
                _colorizedBodyStreamReader = _streamSet.CreateReader();
                _colorizedBodyStream = _colorizedBodyStreamReader.GetStream<ColorizedBodyStream>();
                _bodyStreamReader = _streamSet.CreateReader();
                _bodyStream = _bodyStreamReader.GetStream<BodyStream>();

                var depthModes = _depthStream.AvailableModes;
                var colorModes = _colorStream.AvailableModes;
                var infraredModes = _infraredStream.AvailableModes;
                if (depthModes != null && depthModes.Length != 0
                    && colorModes != null && colorModes.Length != 0 && infraredModes != null && infraredModes.Length != 0)
                {
                    // 标记为已初始化
                    _initialized = true;
                    _initializing = false;

                    if (!string.IsNullOrEmpty(_license))
                    {
                        //设置License
                        Debug.Log("Your license is: " + _license);
                        Astra.BodyTracking.SetLicense(_license);
                    }

                    if (_initSuccessCallback != null)
                    {
                        _initSuccessCallback ();
                    }
                }
                else
                {
                    _DisposeAllStreams();
                }
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: _CheckStreamImageModes Failed :" + e.ToString());

                _DisposeAllStreams ();
            }
        }

        private void _DisposeAllStreams()
        {
            _WaitForUpdate(waitIndefinitely);

            if (_depthStream != null)
            {
                _depthStream.Stop();
                _depthStream = null;
            }
            _isDepthOpened = false;

            if (_colorStream != null)
            {
                _colorStream.Stop();
                _colorStream = null;
            }
            _isColorOpened = false;

            if(_infraredStream != null)
            {
                _infraredStream.Stop();
                _infraredStream = null;
            }
			_isInfraredOpened = false;

            if (_maskedColorStream != null)
            {
                _maskedColorStream.Stop();
                _maskedColorStream = null;
            }
            _isMaskedColorOpened = false;

            if (_colorizedBodyStream != null)
            {
                _colorizedBodyStream.Stop ();
                _colorizedBodyStream = null;
            }
            _isColorizedBodyOpened = false;

            if (_bodyStream != null)
            {
                _bodyStream.Stop();
                _bodyStream = null;
            }
            _isBodyOpened = false;

            if (_depthStreamReader != null)
            {
                _depthStreamReader.Dispose();
                _depthStreamReader = null;
            }

            if (_colorStreamReader != null)
            {
                _colorStreamReader.Dispose();
                _colorStreamReader = null;
            }

            if(_infraredStreamReader != null)
            {
                _infraredStreamReader.Dispose();
                _infraredStreamReader = null;
            }

            if (_maskedColorStreamReader != null)
            {
                _maskedColorStreamReader.Dispose();
                _maskedColorStreamReader = null;
            }

            if (_colorizedBodyStreamReader != null)
            {
                _colorizedBodyStreamReader.Dispose();
                _colorizedBodyStreamReader = null;
            }

            if (_bodyStreamReader != null)
            {
                _bodyStreamReader.Dispose();
                _bodyStreamReader = null;
            }

            if (_streamSet != null)
            {
                _streamSet.Dispose ();
                _streamSet = null;
            }
        }

        // 开深度流
        private void _OpenDepthStream()
        {
            if (_isDepthOpened)
                return;

            Debug.Log("StreamManager: open depth stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);

                if (_depthMode == null)
                {
                    _depthMode = _depthStream.AvailableModes [0];
                }
                _depthStream.SetMode (_depthMode);

                _depthStream.Start();

                _isDepthOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open depth stream: " + e.ToString());
                _CloseDepthStream();
            }
        }

        // 关深度流
        private void _CloseDepthStream()
        {
            if (!_isDepthOpened)
                return;

            Debug.Log("StreamManager: close depth stream");

            _WaitForUpdate(waitIndefinitely);
            _depthStream.Stop();
            _isDepthOpened = false;
        }

        // 开彩色流
        private void _OpenColorStream()
        {
            if (_isColorOpened)
                return;

            Debug.Log("StreamManager: open color stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);

               if (_colorMode == null)
               {
                   _colorMode = _colorStream.AvailableModes [0];
               }
               _colorStream.SetMode (_colorMode);

                _colorStream.Start();

                _isColorOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open color stream: " + e.ToString());
                _CloseColorStream();
            }
        }

        // 关彩色流
        private void _CloseColorStream()
        {
            if (!_isColorOpened)
                return;
            
            Debug.Log("StreamManager: close color stream");

            _WaitForUpdate(waitIndefinitely);
            _colorStream.Stop();
            _isColorOpened = false;

        }

        // 开IR流
        private void _OpenInfraredStream()
        {
            if (_isInfraredOpened)
                return;

            Debug.Log("StreamManager: open infrared stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);

                if (_infraredMode == null)
                {
                    _infraredMode = _infraredStream.AvailableModes[0];
                }
                _infraredStream.SetMode(_infraredMode);

                _infraredStream.Start();

                _isInfraredOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open infrared stream: " + e.ToString());
                _CloseInfraredStream();
            }
        }

        // 关彩色流
        private void _CloseInfraredStream()
        {
            if (!_isInfraredOpened)
                return;

            Debug.Log("StreamManager: close infrared stream");

            _WaitForUpdate(waitIndefinitely);
            _infraredStream.Stop();
            _isInfraredOpened = false;

        }

        // 开抠图流
        private void _OpenMaskedColorStream()
        {
            if (_isMaskedColorOpened)
                return;
            
            Debug.Log("StreamManager: open maskedColor stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);
                _maskedColorStream.Start();
                _isMaskedColorOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open maskedColor stream: " + e.ToString());
                _CloseMaskedColorStream();
            }
        }

        private void _CloseMaskedColorStream()
        {
            if (!_isMaskedColorOpened)
                return;
            
            Debug.Log("StreamManager: close maskedColor stream");

            _WaitForUpdate(waitIndefinitely);
            _maskedColorStream.Stop();
            _isMaskedColorOpened = false;
        }


        private void _OpenColorizedBodyStream()
        {
            if (_isColorizedBodyOpened)
                return;
            
            Debug.Log("StreamManager: open colorizedBody stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);
                _colorizedBodyStream.Start();
                _isColorizedBodyOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open colorizedBody stream: " + e.ToString());
                _CloseColorizedBodyStream();
            }
        }

        private void _CloseColorizedBodyStream()
        {
            if (!_isColorizedBodyOpened)
                return;
            
            Debug.Log("StreamManager: close colorizedBody stream");

            _WaitForUpdate(waitIndefinitely);
            _colorizedBodyStream.Stop();
            _isColorizedBodyOpened = false;
        }


        private void _OpenBodyStream()
        {
            if (_isBodyOpened)
                return;
            
            Debug.Log("StreamManager: open body stream");

            try
            {
                _WaitForUpdate(waitIndefinitely);

                _bodyStream.SetSkeletonProfile(_skeletonProfile);
                _bodyStream.SetSkeletonOptimization(_skeletonOptimization);
                _bodyStream.SetDefaultBodyFeatures(_defaultBodyFeatures);

                _bodyStream.Start();

                _isBodyOpened = true;
            }
            catch (AstraException e)
            {
                Debug.Log("StreamManager: Couldn't open body stream: " + e.ToString());
                _CloseBodyStream();
            }
        }

        private void _CloseBodyStream()
        {
            if (!_isBodyOpened)
                return;
            
            Debug.Log("StreamManager: close body stream");

            _WaitForUpdate(waitIndefinitely);
            _bodyStream.Stop();
            _isBodyOpened = false;
        }

        private void _CheckStreamOpenOrClose()
        {
            if (_openDepth && !_isDepthOpened)
            {
                _OpenDepthStream ();
            }
            else if(!_openDepth && _isDepthOpened)
            {
                _CloseDepthStream ();
            }

            if (_openColor && !_isColorOpened)
            {
                _OpenColorStream ();
            }
            else if(!_openColor && _isColorOpened)
            {
                _CloseColorStream ();
            }

            if(_openInfrared && !_isInfraredOpened)
            {
                _OpenInfraredStream ();
            }
            else if(!_openInfrared && _isInfraredOpened)
            {
                _CloseInfraredStream();
            }

            if (_openMaskedColor && !_isMaskedColorOpened)
            {
                _OpenMaskedColorStream ();
            }
            else if(!_openMaskedColor && _isMaskedColorOpened)
            {
                _CloseMaskedColorStream ();
            }

            if (_openColorizedBody && !_isColorizedBodyOpened)
            {
                _OpenColorizedBodyStream ();
            }
            else if(!_openColorizedBody && _isColorizedBodyOpened)
            {
                _CloseColorizedBodyStream ();
            }

            if (_openBody && !_isBodyOpened)
            {
                _OpenBodyStream ();
            }
            else if(!_openBody && _isBodyOpened)
            {
                _CloseBodyStream ();
            }
        }

        private void _CheckForNewFrames()
        {
            if (_WaitForUpdate(5) && _updateCompleted)
            {
                if (_openDepth && _isDepthOpened)
                {
                    _CheckDepthFrame ();
                }

                if (_openColor && _isColorOpened)
                {
                    _CheckColorFrame ();
                }

                if(_openInfrared && _isInfraredOpened)
                {
                    _CheckInfraredFrame();
                }

                if (_openMaskedColor && _isMaskedColorOpened)
                {
                    _CheckMaskedColorFrame ();
                }

                if (_openColorizedBody && _isColorizedBodyOpened)
                {
                    _CheckColorizedBodyFrame ();
                }

                if (_openBody && _isBodyOpened)
                {
                    _CheckBodyFrame ();
                }

                _frameCount++;
            }
        }

//        private TextureFormat _GetTextureFormatByPixelFormat(Astra.PixelFormat pixelFormat)
//        {
//            TextureFormat ret = TextureFormat.RGBA32;
//            switch (pixelFormat)
//            {
//                case PixelFormat.RGB888:
//                    ret = TextureFormat.RGB24;
//                    break;
//
//                case PixelFormat.Gray8:
//                    ret = TextureFormat.Alpha8;
//                    break;
//
//                default:
//                    ret = TextureFormat.RGBA32;
//                    break;
//            }
//            return ret;
//        }
//
//            public enum PixelFormat
//            {
//                Unknown,
//                DepthMM = 100,
//                RGB888 = 200,
//                YUV422,
//                YUVY,
//                RGBA,
//                Gray8 = 300,
//                Gray16,
//                Point = 400
//            }
//        }

        // 刷新深度数据
        private void _CheckDepthFrame()
        {
            ReaderFrame frame;
            if (_depthStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    DepthFrame depthFrame = frame.GetFrame<DepthFrame>();

                    if (depthFrame != null)
                    {
                        if(_lastDepthFrameIndex != depthFrame.FrameIndex)
                        {
                            if (depthFrame.Width != 0 && depthFrame.Height != 0)
                            {
                                _lastDepthFrameIndex = depthFrame.FrameIndex;

                                _streamData.UpdateDepthStreamData(depthFrame, _usingDepthTexture);
                            }
                        }
                    }
                }
            }
        }

        // 刷新彩色图数据
        private void _CheckColorFrame()
        {
            ReaderFrame frame;
            if (_colorStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    ColorFrame colorFrame = frame.GetFrame<ColorFrame>();

                    if (colorFrame != null)
                    {
                        if(_lastColorFrameIndex != colorFrame.FrameIndex)
                        {
                            if (colorFrame.Width != 0 && colorFrame.Height != 0)
                            {
                                _lastColorFrameIndex = colorFrame.FrameIndex;

                                _streamData.UpdateColorStreamData(colorFrame, _usingColorTexture);
                            }
                        }
                    }
                }
            }
        }

        // 刷新IR数据
        private void _CheckInfraredFrame()
        {
            ReaderFrame frame;
            if (_infraredStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    InfraredFrame infraredFrame = frame.GetFrame<InfraredFrame>();

                    if (infraredFrame != null)
                    {
                        if (_lastInfraredFrameIndex != infraredFrame.FrameIndex)
                        {
                            if (infraredFrame.Width != 0 && infraredFrame.Height != 0)
                            {
                                _lastInfraredFrameIndex = infraredFrame.FrameIndex;

                                _streamData.UpdateInfraredStreamData(infraredFrame, _usingInfraredTexture);
                            }
                        }
                    }
                }
            }
        }

        // 刷新抠图数据
        private void _CheckMaskedColorFrame()
        {
            ReaderFrame frame;
            if (_maskedColorStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    MaskedColorFrame maskedColorFrame = frame.GetFrame<MaskedColorFrame>();

                    if (maskedColorFrame != null)
                    {
                        if(_lastMaskedColorFrameIndex != maskedColorFrame.FrameIndex)
                        {
                            if (maskedColorFrame.Width != 0 && maskedColorFrame.Height != 0)
                            {
                                _lastMaskedColorFrameIndex = maskedColorFrame.FrameIndex;

                                _streamData.UpdateMaskedColorStreamData(maskedColorFrame, _usingMaskedColorTexture);
                            }
                        }
                    }
                }
            }
        }

        // 刷新label图数据
        private void _CheckColorizedBodyFrame()
        {
            ReaderFrame frame;
            if (_colorizedBodyStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    ColorizedBodyFrame colorizedBodyFrame = frame.GetFrame<ColorizedBodyFrame>();

                    if (colorizedBodyFrame != null)
                    {
                        if(_lastColorizedBodyFrameIndex != colorizedBodyFrame.FrameIndex)
                        {
                            if (colorizedBodyFrame.Width != 0 && colorizedBodyFrame.Height != 0)
                            {
                                _lastColorizedBodyFrameIndex = colorizedBodyFrame.FrameIndex;

                                _streamData.UpdateColorizedBodyStreamData(colorizedBodyFrame, _usingColorizedBodyTexture);
                            }
                        }
                    }
                }
            }
        }

        // 刷新用户数据
        private void _CheckBodyFrame()
        {
            ReaderFrame frame;
            if (_bodyStreamReader.TryOpenFrame(0, out frame))
            {
                using (frame)
                {
                    BodyFrame bodyFrame = frame.GetFrame<BodyFrame>();

                    if (bodyFrame != null)
                    {
                        if(_lastBodyFrameIndex != bodyFrame.FrameIndex)
                        {
                            _lastBodyFrameIndex = bodyFrame.FrameIndex;

                            _streamData.UpdateBodyStreamData(bodyFrame);
                        }
                    }
                }
            }
        }


        private void LateUpdate()
        {
            if (_initializing)
            {
                _InitializeAllStreams ();
            }

            if (!_initialized)
                return;

            if (_isDepthOpened
                || _isColorOpened
				|| _isInfraredOpened
                || _isMaskedColorOpened
                || _isColorizedBodyOpened
                || _isBodyOpened)
            {
                _CheckForNewFrames();
            }

            if (!_updateRequested)
            {
                _CheckStreamOpenOrClose ();
                //                UpdateStreamStartStop();

                if (_isDepthOpened
                    || _isColorOpened
					|| _isInfraredOpened
                    || _isMaskedColorOpened
                    || _isColorizedBodyOpened
                    || _isBodyOpened)
                {
                    _UpdateAsync ();
                }
            }
        }

        private void OnDestroy()
        {
            Debug.Log("StreamManager.OnDestroy");

            if (_initializing || _initialized)
            {
                _WaitForUpdate (waitIndefinitely);
                _Terminate ();
            }
        }

        private void OnApplicationQuit()
        {
            Debug.Log("StreamManager handling OnApplicationQuit");

            if (_initializing || _initialized)
            {
                _WaitForUpdate (waitIndefinitely);
                _Terminate ();
            }
        }

        #endregion
    }
}
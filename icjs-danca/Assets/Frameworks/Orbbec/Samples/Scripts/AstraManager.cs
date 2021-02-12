#if UNITY_ANDROID && !UNITY_EDITOR
#define ASTRA_UNITY_ANDROID_NATIVE
#endif

using Astra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using System;

[System.Serializable]
public class NewDepthFrameEvent : UnityEvent<DepthFrame> { }
[System.Serializable]
public class NewColorFrameEvent : UnityEvent<ColorFrame> { }
[System.Serializable]
public class NewBodyFrameEvent : UnityEvent<BodyFrame> { }
[System.Serializable]
public class NewMaskedColorFrameEvent : UnityEvent<MaskedColorFrame> { }
[System.Serializable]
public class NewColorizedBodyFrameEvent : UnityEvent<ColorizedBodyFrame> { }

public class AstraManager : Singleton<AstraManager>
{
    private Astra.StreamSet _streamSet;
    private Astra.StreamReader _readerDepth;
    private Astra.StreamReader _readerColor;
    private Astra.StreamReader _readerBody;
    private Astra.StreamReader _readerMaskedColor;
    private Astra.StreamReader _readerColorizedBody;

    private DepthStream _depthStream;
    //Get depth stream
    public DepthStream DepthStream
    {
        get
        {
            return _depthStream;
        }
    }

    private ColorStream _colorStream;
    //Get color stream
    public ColorStream ColorStream
    {
        get
        {
            return _colorStream;
        }
    }

    private BodyStream _bodyStream;
    //Get body stream
    public BodyStream BodyStream
    {
        get
        {
            return _bodyStream;
        }
    }

    private MaskedColorStream _maskedColorStream;
    //Get masked color stream
    public MaskedColorStream MaskedColorStream
    {
        get
        {
            return _maskedColorStream;
        }
    }

    private ColorizedBodyStream _colorizedBodyStream;
    //Get colorized body stream
    public ColorizedBodyStream ColorizedBodyStream
    {
        get
        {
            return _colorizedBodyStream;
        }
    }

    private ImageMode[] _depthModes;
    //Get available depth modes
    public ImageMode[] AvailableDepthModes
    {
        get
        {
            return _depthModes;
        }
    }

    private ImageMode[] _colorModes;
    //Get available color modes
    public ImageMode[] AvailableColorModes
    {
        get
        {
            return _colorModes;
        }
    }

    private ImageMode _depthMode;
    //Get and set depth mode
    public ImageMode DepthMode
    {
        get
        {
            return _depthMode;
        }
        set
        {
            _depthMode = value;
            if (_depthStream != null)
            {
                _depthStream.SetMode(_depthMode);
            }
        }
    }

    private ImageMode _colorMode;
    //Get and set color mode
    public ImageMode ColorMode
    {
        get
        {
            return _colorMode;
        }
        set
        {
            _colorMode = value;
            if (_colorStream != null)
            {
                _colorStream.SetMode(_colorMode);
            }
        }
    }

    bool _isDepthOn = false;
    //Get and set depth stream on
    public bool IsDepthOn
    {
        get
        {
            return _isDepthOn;
        }
        set
        {
            _isDepthOn = value;
        }
    }

    bool _isColorOn = false;
    //Get and set color stream on
    public bool IsColorOn
    {
        get
        {
            return _isColorOn;
        }
        set
        {
            _isColorOn = value;
        }
    }

    bool _isBodyOn = false;
    //Get and set body stream on
    public bool IsBodyOn
    {
        get
        {
            return _isBodyOn;
        }
        set
        {
            _isBodyOn = value;
        }
    }

    bool _isMaskedColorOn = false;
    //Get and set masked color stream on
    public bool IsMaskedColorOn
    {
        get
        {
            return _isMaskedColorOn;
        }
        set
        {
            _isMaskedColorOn = value;
        }
    }

    bool _isColorizedBodyOn = false;
    //Get and set colorized body stream on
    public bool IsColorizedBodyOn
    {
        get
        {
            return _isColorizedBodyOn;
        }
        set
        {
            _isColorizedBodyOn = value;
        }
    }

    private Texture2D _depthTexture;
    //Get depth texture
    public Texture2D DepthTexture
    {
        get
        {
            if (_depthTexture == null)
            {
                _depthTexture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            }
            return _depthTexture;
        }
    }

    private Texture2D _colorTexture;
    //Get color texture
    public Texture2D ColorTexture
    {
        get
        {
            if (_colorTexture == null)
            {
                _colorTexture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            }
            return _colorTexture;
        }
    }

    private Texture2D _maskedColorTexture;
    //Get masked color texture
    public Texture2D MaskedColorTexture
    {
        get
        {
            if (_maskedColorTexture == null)
            {
                _maskedColorTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            }
            return _maskedColorTexture;
        }
    }

    private Texture2D _colorizedBodyTexture;
    //Get colorized body texture
    public Texture2D ColorizedBodyTexture
    {
        get
        {
            if (_colorizedBodyTexture == null)
            {
                _colorizedBodyTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            }
            return _colorizedBodyTexture;
        }
    }

    private Body[] _bodies = { };
    //Get bodies
    public Body[] Bodies
    {
        get
        {
            return _bodies;
        }
    }

    private long _lastBodyFrameIndex = -1;
    private long _lastDepthFrameIndex = -1;
    private long _lastColorFrameIndex = -1;
    private long _lastMaskedColorFrameIndex = -1;
    private long _lastColorizedBodyFrameIndex = -1;

    private int _lastWidth = 0;
    private int _lastHeight = 0;
    private short[] _buffer;
    private int _frameCount = 0;
    private int _initializeCount = 500;

    private bool _areStreamsInitialized = false;
    //Get whether initialize
    public bool Initialized
    {
        get
        {
            return _areStreamsInitialized;
        }
    }

    [SerializeField]
    private string _license = "";
    //Get and set license
    public string License
    {
        get
        {
            return _license;
        }
        set
        {
            _license = value;
        }
    }

    //Initialize success event
    public UnityEvent OnInitializeSuccess = new UnityEvent();
    //Initialize failed event
    public UnityEvent OnInitializeFailed = new UnityEvent();
    //New depth frame event
    public NewDepthFrameEvent OnNewDepthFrame = new NewDepthFrameEvent();
    //New color frame event
    public NewColorFrameEvent OnNewColorFrame = new NewColorFrameEvent();
    //New body frame event
    public NewBodyFrameEvent OnNewBodyFrame = new NewBodyFrameEvent();
    //New masked color frame event
    public NewMaskedColorFrameEvent OnMaskedColorFrame = new NewMaskedColorFrameEvent();
    //New colorized body frame event
    public NewColorizedBodyFrameEvent OnNewColorizedBodyFrame = new NewColorizedBodyFrameEvent();


    private void Start()
    {
        AstraUnityContext.Instance.Initializing += OnAstraInitializing;
        AstraUnityContext.Instance.Terminating += OnAstraTerminating;

        Debug.Log("AstraUnityContext initialize");
        AstraUnityContext.Instance.Initialize();
    }

    private void OnAstraInitializing(object sender, AstraInitializingEventArgs e)
    {
        if (!string.IsNullOrEmpty(_license))
        {
            Debug.Log("Set license: " + _license);
            Astra.BodyTracking.SetLicense(_license);
        }

#if ASTRA_UNITY_ANDROID_NATIVE
        // if (AutoRequestAndroidUsbPermission)
        // {
            Debug.Log("Auto-requesting usb device access.");
            AstraUnityContext.Instance.RequestUsbDeviceAccessFromAndroid();
        // }
#endif
        Debug.Log("Initialize streams");
        InitializeStreams();
    }

    private void InitializeStreams()
    {
        try
        {
            AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

            _streamSet = Astra.StreamSet.Open();

            _readerDepth = _streamSet.CreateReader();
            _readerColor = _streamSet.CreateReader();
            _readerBody = _streamSet.CreateReader();
            _readerMaskedColor = _streamSet.CreateReader();
            _readerColorizedBody = _streamSet.CreateReader();

            _depthStream = _readerDepth.GetStream<DepthStream>();

            _depthModes = _depthStream.AvailableModes;
            ImageMode selectedDepthMode = _depthModes[0];

#if ASTRA_UNITY_ANDROID_NATIVE
            int targetDepthWidth = 160;
            int targetDepthHeight = 120;
            int targetDepthFps = 30;
#else
            int targetDepthWidth = 320;
            int targetDepthHeight = 240;
            int targetDepthFps = 30;
#endif

            foreach (var m in _depthModes)
            {
                Debug.Log("Depth mode: " + m.Width + "x" + m.Height + "@" + m.FramesPerSecond);
                if (m.Width == targetDepthWidth &&
                    m.Height == targetDepthHeight)
                {
                    selectedDepthMode = m;
                    break;
                }
            }

            _depthStream.SetMode(selectedDepthMode);

            _colorStream = _readerColor.GetStream<ColorStream>();
            try
            {
                _colorModes = _colorStream.AvailableModes;
                ImageMode selectedColorMode = _colorModes[0];

#if ASTRA_UNITY_ANDROID_NATIVE
                int targetColorWidth = 320;
                int targetColorHeight = 240;
                int targetColorFps = 30;
#else
                int targetColorWidth = 640;
                int targetColorHeight = 480;
                int targetColorFps = 30;
#endif

                foreach (var m in _colorModes)
                {
                    Debug.Log("Color mode: " + m.Width + "x" + m.Height + "@" + m.FramesPerSecond);
                    if (m.Width == targetColorWidth &&
                        m.Height == targetColorHeight)
                    {
                        selectedColorMode = m;
                        break;
                    }
                }

                _colorStream.SetMode(selectedColorMode);
            }
            catch (System.Exception e)
            {
                Debug.Log("Couldn't initialize color stream: " + e.ToString());
            }

            _bodyStream = _readerBody.GetStream<BodyStream>();

            _maskedColorStream = _readerMaskedColor.GetStream<MaskedColorStream>();

            _colorizedBodyStream = _readerColorizedBody.GetStream<ColorizedBodyStream>();

            _areStreamsInitialized = true;

            Debug.Log("Stream initialize success");
            OnInitializeSuccess.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.Log("Couldn't initialize streams: " + e.ToString());
            UninitializeStreams();

            if (_initializeCount > 0)
            {
                _initializeCount--;
            }
            else
            {
                Debug.Log("Initialize failed");
                OnInitializeFailed.Invoke();
            }
        }
    }

    private void OnAstraTerminating(object sender, AstraTerminatingEventArgs e)
    {
        Debug.Log("Astra is tearing down");
        UninitializeStreams();
    }

    private void UninitializeStreams()
    {
        AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

        Debug.Log("Uninitializing streams");
        if (_readerDepth != null)
        {
            _readerDepth.Dispose();
            _readerDepth = null;
        }
        if (_readerColor != null)
        {
            _readerColor.Dispose();
            _readerColor = null;
        }
        if (_readerBody != null)
        {
            _readerBody.Dispose();
            _readerBody = null;
        }
        if (_readerMaskedColor != null)
        {
            _readerMaskedColor.Dispose();
            _readerMaskedColor = null;
        }
        if (_readerColorizedBody != null)
        {
            _readerColorizedBody.Dispose();
            _readerColorizedBody = null;
        }
        if (_streamSet != null)
        {
            _streamSet.Dispose();
            _streamSet = null;
        }
    }

    private void CheckDepthReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerDepth.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                DepthFrame depthFrame = frame.GetFrame<DepthFrame>();

                if (depthFrame != null)
                {
                    if (_lastDepthFrameIndex != depthFrame.FrameIndex)
                    {
                        _lastDepthFrameIndex = depthFrame.FrameIndex;

                        UpdateDepthTexture(depthFrame);
                        OnNewDepthFrame.Invoke(depthFrame);
                    }
                }
            }
        }
    }

    private void CheckColorReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerColor.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                ColorFrame colorFrame = frame.GetFrame<ColorFrame>();

                if (colorFrame != null)
                {
                    if (_lastColorFrameIndex != colorFrame.FrameIndex)
                    {
                        _lastColorFrameIndex = colorFrame.FrameIndex;

                        UpdateColorTexture(colorFrame);
                        OnNewColorFrame.Invoke(colorFrame);
                    }
                }
            }
        }
    }

    private void CheckBodyReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerBody.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                BodyFrame bodyFrame = frame.GetFrame<BodyFrame>();

                if (bodyFrame != null)
                {
                    if (_lastBodyFrameIndex != bodyFrame.FrameIndex)
                    {
                        _lastBodyFrameIndex = bodyFrame.FrameIndex;

                        UpdateBody(bodyFrame);
                        OnNewBodyFrame.Invoke(bodyFrame);
                    }
                }
            }
        }
    }

    private void CheckMaskedColorReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerMaskedColor.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                MaskedColorFrame maskedColorFrame = frame.GetFrame<MaskedColorFrame>();

                if (maskedColorFrame != null)
                {
                    if (_lastMaskedColorFrameIndex != maskedColorFrame.FrameIndex)
                    {
                        _lastMaskedColorFrameIndex = maskedColorFrame.FrameIndex;

                        UpdateMaskedColorTexture(maskedColorFrame);
                        OnMaskedColorFrame.Invoke(maskedColorFrame);
                    }
                }
            }
        }
    }

    private void CheckColorizedBodyReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerColorizedBody.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                ColorizedBodyFrame colorizedBodyFrame = frame.GetFrame<ColorizedBodyFrame>();

                if (colorizedBodyFrame != null)
                {
                    if (_lastColorizedBodyFrameIndex != colorizedBodyFrame.FrameIndex)
                    {
                        _lastColorizedBodyFrameIndex = colorizedBodyFrame.FrameIndex;

                        UpdateColorizedBodyTexture(colorizedBodyFrame);
                        OnNewColorizedBodyFrame.Invoke(colorizedBodyFrame);
                    }
                }
            }
        }
    }

    private bool UpdateUntilDelegate()
    {
        return true;
        // Check if any readers have new frames.
        // StreamReader.HasNewFrame() is thread-safe and can be called
        // from any thread.
        bool hasNewFrameDepth = _readerDepth != null && _readerDepth.HasNewFrame();
        bool hasNewFrameColor = _readerColor != null && _readerColor.HasNewFrame();
        bool hasNewFrameBody = _readerBody != null && _readerBody.HasNewFrame();
        bool hasNewFrameMaskedColor = _readerMaskedColor != null && _readerMaskedColor.HasNewFrame();
        bool hasNewFrameColorizedBody = _readerColorizedBody != null && _readerColorizedBody.HasNewFrame();

        Debug.Log("ND: " + hasNewFrameDepth +
                  " NC: " + hasNewFrameColor +
                  " NB: " + hasNewFrameBody +
                  " NMC: " + hasNewFrameMaskedColor +
                  " NCB: " + hasNewFrameColorizedBody);
        Debug.Log("DO: " + _isDepthOn +
                  " CO: " + _isColorOn +
                  " BO: " + _isBodyOn +
                  " MCO: " + _isMaskedColorOn +
                  " CBO: " + _isColorizedBodyOn);
        bool hasNewFrame = true;
        if (_isColorizedBodyOn)
        {
            hasNewFrame = hasNewFrameColorizedBody;
        }
        else if (_isMaskedColorOn)
        {
            hasNewFrame = hasNewFrameMaskedColor;
        }
        else if (_isBodyOn)
        {
            hasNewFrame = hasNewFrameBody;
        }
        else if (_isDepthOn)
        {
            hasNewFrame = hasNewFrameDepth;
        }

        if (_isColorOn)
        {
            hasNewFrame = hasNewFrame && hasNewFrameColor;
        }

        // If no streams are started (during start up or shutdown)
        // then allow updateUntil to be complete
        bool noStreamsStarted = !_isDepthOn &&
                                !_isColorOn &&
                                !_isBodyOn &&
                                !_isMaskedColorOn &&
                                !_isColorizedBodyOn;

        return hasNewFrame || noStreamsStarted;
    }

    private void CheckForNewFrames()
    {
        if (AstraUnityContext.Instance.WaitForUpdate(5) && AstraUnityContext.Instance.IsUpdateAsyncComplete)
        {
            // Inside this block until UpdateAsync() call below, we can use the Astra API safely

            CheckDepthReader();
            CheckColorReader();
            CheckBodyReader();
            CheckMaskedColorReader();
            CheckColorizedBodyReader();

            _frameCount++;

        }

        if (!AstraUnityContext.Instance.IsUpdateRequested)
        {
            UpdateStreamStartStop();
            // After calling UpdateAsync() the Astra API will be called from a background thread
            AstraUnityContext.Instance.UpdateAsync(UpdateUntilDelegate);
        }
    }

    void PrintBody(Astra.BodyFrame bodyFrame)
    {
        if (bodyFrame != null)
        {
            Body[] bodies = { };
            bodyFrame.CopyBodyData(ref bodies);
            foreach (Body body in bodies)
            {
                Astra.Joint headJoint = body.Joints[(int)JointType.Head];

                Debug.Log("Body " + body.Id + " COM " + body.CenterOfMass +
                    " Head Depth: " + headJoint.DepthPosition.X + "," + headJoint.DepthPosition.Y +
                    " World: " + headJoint.WorldPosition.X + "," + headJoint.WorldPosition.Y + "," + headJoint.WorldPosition.Z +
                    " Status: " + headJoint.Status.ToString());
            }
        }
    }

    void PrintDepth(Astra.DepthFrame depthFrame,
                    Astra.CoordinateMapper mapper)
    {
        if (depthFrame != null)
        {
            int width = depthFrame.Width;
            int height = depthFrame.Height;
            long frameIndex = depthFrame.FrameIndex;

            //determine if buffer needs to be reallocated
            if (width != _lastWidth || height != _lastHeight)
            {
                _buffer = new short[width * height];
                _lastWidth = width;
                _lastHeight = height;
            }
            depthFrame.CopyData(ref _buffer);

            int index = (int)((width * (height / 2.0f)) + (width / 2.0f));
            short middleDepth = _buffer[index];

            Vector3D worldPoint = mapper.MapDepthPointToWorldSpace(new Vector3D(width / 2.0f, height / 2.0f, middleDepth));
            Vector3D depthPoint = mapper.MapWorldPointToDepthSpace(worldPoint);

            Debug.Log("depth frameIndex: " + frameIndex
                      + " width: " + width
                      + " height: " + height
                      + " middleDepth: " + middleDepth
                      + " wX: " + worldPoint.X
                      + " wY: " + worldPoint.Y
                      + " wZ: " + worldPoint.Z
                      + " dX: " + depthPoint.X
                      + " dY: " + depthPoint.Y
                      + " dZ: " + depthPoint.Z + " frameCount: " + _frameCount);
        }
    }

    private void UpdateStreamStartStop()
    {
        // This methods assumes it is called from a safe location to call Astra API

        if (_depthStream != null)
        {
            if (_isDepthOn)
            {
                _depthStream.Start();
            }
            else
            {
                _depthStream.Stop();
            }
        }

        if (_colorStream != null)
        {
            if (_isColorOn)
            {
                _colorStream.Start();
            }
            else
            {
                _colorStream.Stop();
            }
        }

        if (_bodyStream != null)
        {
            if (_isBodyOn)
            {
                _bodyStream.Start();
            }
            else
            {
                _bodyStream.Stop();
            }
        }

        if (_maskedColorStream != null)
        {
            if (_isMaskedColorOn)
            {
                _maskedColorStream.Start();
            }
            else
            {
                _maskedColorStream.Stop();
            }
        }

        if (_colorizedBodyStream != null)
        {
            if (_isColorizedBodyOn)
            {
                _colorizedBodyStream.Start();
            }
            else
            {
                _colorizedBodyStream.Stop();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_areStreamsInitialized)
        {
            InitializeStreams();
        }

        if (_areStreamsInitialized)
        {
            CheckForNewFrames();
        }
    }

    void OnDestroy()
    {
        AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

        if (_depthStream != null)
        {
            _depthStream.Stop();
        }

        if (_colorStream != null)
        {
            _colorStream.Stop();
        }

        if (_bodyStream != null)
        {
            _bodyStream.Stop();
        }

        if (_maskedColorStream != null)
        {
            _maskedColorStream.Stop();
        }

        if (_colorizedBodyStream != null)
        {
            _colorizedBodyStream.Stop();
        }

        UninitializeStreams();

        AstraUnityContext.Instance.Initializing -= OnAstraInitializing;
        AstraUnityContext.Instance.Terminating -= OnAstraTerminating;

        Debug.Log("AstraUnityContext terminate");
        AstraUnityContext.Instance.Terminate();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Handling OnApplicationQuit");
        AstraUnityContext.Instance.Terminate();
    }

    #region Texture Update
    private short[] _depthFrameData;
    private byte[] _depthTextureBuffer;
    public void UpdateDepthTexture(DepthFrame depthFrame)
    {
        if (depthFrame == null)
        {
            return;
        }
        // 拷贝深度流数据
        if (_depthFrameData == null || _depthFrameData.Length != depthFrame.Width * depthFrame.Height)
        {
            _depthFrameData = new short[depthFrame.Width * depthFrame.Height];
        }
        depthFrame.CopyData(ref _depthFrameData);

        // 深度纹理
        if (_depthTexture == null)
        {
            _depthTexture = new Texture2D(depthFrame.Width, depthFrame.Height, TextureFormat.RGB24, false);
        }
        else if (_depthTexture.width != depthFrame.Width || _depthTexture.height != depthFrame.Height)
        {
            _depthTexture.Resize(depthFrame.Width, depthFrame.Height, TextureFormat.RGB24, false);
        }

        if (_depthTextureBuffer == null || _depthTextureBuffer.Length != depthFrame.Width * depthFrame.Height * 3)
        {
            _depthTextureBuffer = new byte[depthFrame.Width * depthFrame.Height * 3];
        }
        int length = _depthFrameData.Length;
        for (int i = 0; i < length; i++)
        {
            short depth = _depthFrameData[i];
            byte depthByte = (byte)0;
            if (depth != 0)
            {
                depthByte = (byte)(255 - (255 * depth / 10000.0f));
            }
            _depthTextureBuffer[i * 3 + 0] = depthByte;
            _depthTextureBuffer[i * 3 + 1] = depthByte;
            _depthTextureBuffer[i * 3 + 2] = depthByte;
        }

        if (_depthTextureBuffer != null && _depthTextureBuffer.Length > 0)
        {
            _depthTexture.LoadRawTextureData(_depthTextureBuffer);
            _depthTexture.Apply(false);
        }
    }

    private byte[] _colorFrameData;
    public void UpdateColorTexture(ColorFrame colorFrame)
    {
        if (colorFrame == null)
        {
            return;
        }
        // 拷贝彩色流数据
        if (_colorFrameData == null || _colorFrameData.Length != colorFrame.ByteLength)
        {
            _colorFrameData = new byte[colorFrame.ByteLength];
        }
        colorFrame.CopyData(ref _colorFrameData);

        // 彩色纹理
        if (_colorTexture == null)
        {
            _colorTexture = new Texture2D(colorFrame.Width, colorFrame.Height, TextureFormat.RGB24, false);
        }
        else if (_colorTexture.width != colorFrame.Width || _colorTexture.height != colorFrame.Height)
        {
            _colorTexture.Resize(colorFrame.Width, colorFrame.Height, TextureFormat.RGB24, false);
        }

        if (_colorFrameData != null && _colorFrameData.Length > 0)
        {
            _colorTexture.LoadRawTextureData(_colorFrameData);
            _colorTexture.Apply(false);
        }
    }

    private byte[] _maskedColorFrameData;
    public void UpdateMaskedColorTexture(MaskedColorFrame maskedColorFrame)
    {
        if (maskedColorFrame == null)
        {
            return;
        }
        // 拷贝抠图流数据
        if (_maskedColorFrameData == null || _maskedColorFrameData.Length != maskedColorFrame.ByteLength)
        {
            _maskedColorFrameData = new byte[maskedColorFrame.ByteLength];
        }
        maskedColorFrame.CopyData(ref _maskedColorFrameData);

        // 抠图纹理
        if (_maskedColorTexture == null)
        {
            _maskedColorTexture = new Texture2D(maskedColorFrame.Width, maskedColorFrame.Height, TextureFormat.RGBA32, false);
        }
        else if (_maskedColorTexture.width != maskedColorFrame.Width || _maskedColorTexture.height != maskedColorFrame.Height)
        {
            _maskedColorTexture.Resize(maskedColorFrame.Width, maskedColorFrame.Height, TextureFormat.RGBA32, false);
        }

        if (_maskedColorFrameData != null && _maskedColorFrameData.Length > 0)
        {
            _maskedColorTexture.LoadRawTextureData(_maskedColorFrameData);
            _maskedColorTexture.Apply(false);
        }
    }

    private byte[] _colorizedBodyFrameData;
    public void UpdateColorizedBodyTexture(ColorizedBodyFrame colorizedBodyFrame)
    {
        if (colorizedBodyFrame == null)
        {
            return;
        }
        // 拷贝label图流数据
        if (_colorizedBodyFrameData == null || _colorizedBodyFrameData.Length != colorizedBodyFrame.ByteLength)
        {
            _colorizedBodyFrameData = new byte[colorizedBodyFrame.ByteLength];
        }
        colorizedBodyFrame.CopyData(ref _colorizedBodyFrameData);

        // label图纹理
        if (_colorizedBodyTexture == null)
        {
            _colorizedBodyTexture = new Texture2D(colorizedBodyFrame.Width, colorizedBodyFrame.Height, TextureFormat.RGBA32, false);
        }
        else if (_colorizedBodyTexture.width != colorizedBodyFrame.Width || _colorizedBodyTexture.height != colorizedBodyFrame.Height)
        {
            _colorizedBodyTexture.Resize(colorizedBodyFrame.Width, colorizedBodyFrame.Height, TextureFormat.RGBA32, false);
        }

        if (_colorizedBodyFrameData != null && _colorizedBodyFrameData.Length > 0)
        {
            _colorizedBodyTexture.LoadRawTextureData(_colorizedBodyFrameData);
            _colorizedBodyTexture.Apply(false);
        }
    }

    public void UpdateBody(BodyFrame bodyFrame)
    {
        if (bodyFrame == null)
        {
            return;
        }
        bodyFrame.CopyBodyData(ref _bodies);
    }
    #endregion
}

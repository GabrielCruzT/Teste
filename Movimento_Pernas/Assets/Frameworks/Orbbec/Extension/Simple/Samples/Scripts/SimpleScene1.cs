using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Astra.Simple;
using System;

public class SimpleScene1 : MonoBehaviour
{
    [SerializeField]
    Toggle DepthResolutionToggle1 = null;
    [SerializeField]
    Toggle DepthResolutionToggle2 = null;
    [SerializeField]
    Toggle DepthResolutionToggle3 = null;
    [SerializeField]
    Toggle DepthResolutionToggle4 = null;

    [SerializeField]
    Toggle ColorResolutionToggle1 = null;
    [SerializeField]
    Toggle ColorResolutionToggle2 = null;
    [SerializeField]
    Toggle ColorResolutionToggle3 = null;
    [SerializeField]
    Toggle ColorResolutionToggle4 = null;

    [SerializeField]
    Toggle DepthStreamToggle = null;
    [SerializeField]
    Toggle ColorStreamToggle = null;
    [SerializeField]
    Toggle MaskedColorStreamToggle = null;
    [SerializeField]
    Toggle ColorizedBodyStreamToggle = null;
    [SerializeField]
    Toggle BodyStreamToggle = null;

    [SerializeField]
    Toggle DepthTextureToggle = null;
    [SerializeField]
    Toggle ColorTextureToggle = null;
    [SerializeField]
    Toggle MaskedColorTextureToggle = null;
    [SerializeField]
    Toggle ColorizedBodyTextureToggle = null;

    [SerializeField]
    Toggle BodyFeaturesToggle1 = null;
    [SerializeField]
    Toggle BodyFeaturesToggle2 = null;
    [SerializeField]
    Toggle BodyFeaturesToggle3 = null;

    [SerializeField]
    Toggle BodyProfileToggle1 = null;
    [SerializeField]
    Toggle BodyProfileToggle2 = null;
    [SerializeField]
    Toggle BodyProfileToggle3 = null;

    [SerializeField]
    Text BodyDepthText = null;
    [SerializeField]
    Slider BodyDepthSlider = null;

    [SerializeField]
    Button SwitchUVCCameraButton = null;

    [SerializeField]
    Button InitializeButton = null;
    [SerializeField]
    Button TerminateButton = null;

    [SerializeField]
    RawImage DepthImage = null;
    [SerializeField]
    RawImage ColorImage = null;
    [SerializeField]
    RawImage MaskedColorImage = null;
    [SerializeField]
    RawImage ColorizedBodyImage = null;

    [SerializeField]
    RectTransform JointContainer = null;
    [SerializeField]
    Image Joint = null;


    [SerializeField]
    Toggle PublicTrackingToggle = null;
    
    GameObject PublicTrackingObj = null;

    [SerializeField]
    Text GameFpsText = null;
    [SerializeField]
    Text SkeletonFpsText = null;

    private long _tmpTime;
    private long _tmpTime2;
    private int _gameFPS;
    private int _skeletonFPS;
    private long _lastTimeSkeletonDataId;

    private List<List<Image>> jointLists = new List<List<Image>>();

    void Start()
    {
        DepthResolutionToggle1.onValueChanged.AddListener(OnDepthResolutionToggleChanged);
        DepthResolutionToggle2.onValueChanged.AddListener(OnDepthResolutionToggleChanged);
        DepthResolutionToggle3.onValueChanged.AddListener(OnDepthResolutionToggleChanged);
        DepthResolutionToggle4.onValueChanged.AddListener(OnDepthResolutionToggleChanged);

        ColorResolutionToggle1.onValueChanged.AddListener(OnColorResolutionToggleChanged);
        ColorResolutionToggle2.onValueChanged.AddListener(OnColorResolutionToggleChanged);
        ColorResolutionToggle3.onValueChanged.AddListener(OnColorResolutionToggleChanged);
        ColorResolutionToggle4.onValueChanged.AddListener(OnColorResolutionToggleChanged);

        DepthStreamToggle.onValueChanged.AddListener(OnDepthStreamToggleChanged);
        ColorStreamToggle.onValueChanged.AddListener(OnColorStreamToggleChanged);
        MaskedColorStreamToggle.onValueChanged.AddListener(OnMaskedColorStreamToggleChanged);
        ColorizedBodyStreamToggle.onValueChanged.AddListener(OnColorizedBodyStreamToggleChanged);
        BodyStreamToggle.onValueChanged.AddListener(OnBodyStreamToggleChanged);

        DepthTextureToggle.onValueChanged.AddListener(OnDepthTextureToggleChanged);
        ColorTextureToggle.onValueChanged.AddListener(OnColorTextureToggleChanged);
        MaskedColorTextureToggle.onValueChanged.AddListener(OnMaskedColorTextureToggleChanged);
        ColorizedBodyTextureToggle.onValueChanged.AddListener(OnColorizedBodyTextureToggleChanged);

        BodyFeaturesToggle1.onValueChanged.AddListener(OnBodyFeaturesToggle1Changed);
        BodyFeaturesToggle2.onValueChanged.AddListener(OnBodyFeaturesToggle2Changed);
        BodyFeaturesToggle3.onValueChanged.AddListener(OnBodyFeaturesToggle3Changed);

        BodyProfileToggle1.onValueChanged.AddListener(OnBodyProfileToggle1Changed);
        BodyProfileToggle2.onValueChanged.AddListener(OnBodyProfileToggle2Changed);
        BodyProfileToggle3.onValueChanged.AddListener(OnBodyProfileToggle3Changed);

        InitializeButton.onClick.AddListener(OnInitializeButtonClick);
        TerminateButton.onClick.AddListener(OnTerminateButtonClick);
        SwitchUVCCameraButton.onClick.AddListener(OnSwitchUVCCameraButtonClick);

        BodyDepthSlider.onValueChanged.AddListener(OnBodyDepthValueChange);

        PublicTrackingToggle.onValueChanged.AddListener(OnPublicTrackingValueChange);

        //        OrbbecManager.instance.streamManager.UsingTexture (TextureType.Color 
        //            | TextureType.Depth 
        //            | TextureType.MaskedColor 
        //            | TextureType.ColorizedBody, true);


        Color[] colors = new Color[10] { Color.red, Color.yellow, Color.blue, Color.black, Color.gray, Color.white, Color.green, Color.red, Color.red, Color.red };

        jointLists = new List<List<Image>>();
        for (int i = 0; i < 10; i++)
        {
            jointLists.Add(new List<Image>());
            for (int z = 0; z < 19; z++)
            {
                Image img = GameObject.Instantiate<Image>(Joint);
                img.rectTransform.SetParent(JointContainer, false);
                img.color = colors[i];
                jointLists[i].Add(img);
            }
        }

        Joint.gameObject.SetActive(false);

        Astra.Tracking.OrbbecTrackingManager.Init();
    }


    void OnPublicTrackingValueChange(bool isOn)
    {
        if (PublicTrackingObj == null)
        {
            PublicTrackingObj = FindObjectOfType<Astra.Tracking.OrbbecTrackingManager>().gameObject;
        }
        if (PublicTrackingObj != null)
        {
            PublicTrackingObj.SetActive(isOn);
        }
    }

    void OnBodyDepthValueChange(float depth)
    {
        int tmpDepth = (int)depth;
        AstraSimpleSDK.streamManager.SetSkeletonOptimization((Astra.SkeletonOptimization)tmpDepth);
        BodyDepthText.text = "骨架精简权重:" + (Astra.SkeletonOptimization)tmpDepth;
    }


    void OnDepthResolutionToggleChanged(bool isOn)
    {
        if (AstraSimpleSDK.streamManager.isInitialized)
        {
            SetDepthMode();
        }
    }
    void OnColorResolutionToggleChanged(bool isOn)
    {
        if (AstraSimpleSDK.streamManager.isInitialized)
        {
            SetColorMode();
        }
    }

    void OnDepthStreamToggleChanged(bool isOn)
    {
        ChangeDepthStream();
    }
    void OnColorStreamToggleChanged(bool isOn)
    {
        ChangeColorStream();
    }
    void OnMaskedColorStreamToggleChanged(bool isOn)
    {
        ChangeMaskedColorStream();
    }
    void OnColorizedBodyStreamToggleChanged(bool isOn)
    {
        ChangeColorizedBodyStream();
    }
    void OnBodyStreamToggleChanged(bool isOn)
    {
        ChangeBodyStream();
    }

    void OnDepthTextureToggleChanged(bool isOn)
    {
        ChangeDepthTexture();
    }
    void OnColorTextureToggleChanged(bool isOn)
    {
        ChangeColorTexture();
    }
    void OnMaskedColorTextureToggleChanged(bool isOn)
    {
        ChangeMaskedColorTexture();
    }
    void OnColorizedBodyTextureToggleChanged(bool isOn)
    {
        ChangeColorizedBodyTexture();
    }

    void OnBodyFeaturesToggle1Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetDefaultBodyFeatures(Astra.BodyTrackingFeatures.Segmentation);
        }
    }
    void OnBodyFeaturesToggle2Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetDefaultBodyFeatures(Astra.BodyTrackingFeatures.Skeleton);
        }
    }
    void OnBodyFeaturesToggle3Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetDefaultBodyFeatures(Astra.BodyTrackingFeatures.HandPose);
        }
    }


    void OnBodyProfileToggle1Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetSkeletonProfile(Astra.SkeletonProfile.Full);
        }
    }
    void OnBodyProfileToggle2Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetSkeletonProfile(Astra.SkeletonProfile.Basic);
        }
    }
    void OnBodyProfileToggle3Changed(bool isOn)
    {
        if (isOn)
        {
            AstraSimpleSDK.streamManager.SetSkeletonProfile(Astra.SkeletonProfile.UpperBody);
        }
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass CameraClass;
    private int cameraIndex;
#endif
    void OnSwitchUVCCameraButtonClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (CameraClass == null)
        {
            CameraClass = new AndroidJavaClass("android.hardware.Camera");
        }
        int numOfCamera = CameraClass.CallStatic<int>("getNumberOfCameras");
        Debug.Log("Num of cameras:" + numOfCamera);
        if (++cameraIndex >= numOfCamera)
        {
            cameraIndex = 0;
        }
        AstraSimpleSDK.streamManager.SetCamera(cameraIndex);
#endif
    }

    void OnInitializeButtonClick()
    {
        AstraSimpleSDK.streamManager.SetInitSuccessCallback(OnInitSuccess);
        AstraSimpleSDK.streamManager.Initialize();
        SwitchUVCCameraButton.enabled = false;
    }
    void OnTerminateButtonClick()
    {
        AstraSimpleSDK.streamManager.Terminate();
        SwitchUVCCameraButton.enabled = true;
    }


    void OnInitSuccess()
    {
        SetDepthMode();
        SetColorMode();

        ChangeDepthStream();
        ChangeColorStream();
        ChangeMaskedColorStream();
        ChangeColorizedBodyStream();
        ChangeBodyStream();

        ChangeDepthTexture();
        ChangeColorTexture();
        ChangeMaskedColorTexture();
        ChangeColorizedBodyTexture();

        OnPublicTrackingValueChange(PublicTrackingToggle.isOn);
    }

    Vector2 GetDepthSize()
    {
        Vector2 ret = Vector2.zero;
        if (DepthResolutionToggle1.isOn)
        {
            ret.x = 160;
            ret.y = 120;
        }
        else if (DepthResolutionToggle2.isOn)
        {
            ret.x = 320;
            ret.y = 240;
        }
        else if (DepthResolutionToggle3.isOn)
        {
            ret.x = 640;
            ret.y = 480;
        }
        else if (DepthResolutionToggle4.isOn)
        {
            ret.x = 640;
            ret.y = 400;
        }
        return ret;
    }

    Vector2 GetColorSize()
    {
        Vector2 ret = Vector2.zero;
        if (ColorResolutionToggle1.isOn)
        {
            ret.x = 320;
            ret.y = 240;
        }
        else if (ColorResolutionToggle2.isOn)
        {
            ret.x = 640;
            ret.y = 480;
        }
        else if (ColorResolutionToggle3.isOn)
        {
            ret.x = 1280;
            ret.y = 720;
        }
        else if (ColorResolutionToggle4.isOn)
        {
            ret.x = 1920;
            ret.y = 1080;
        }
        return ret;
    }

    void SetDepthMode()
    {
        Vector2 size = GetDepthSize();

        Astra.ImageMode[] modes = AstraSimpleSDK.streamManager.GetAvailableDepthModes();
        foreach (var mode in modes)
        {
            if (mode.Width == size.x && mode.Height == size.y && mode.FramesPerSecond == 30)
            {
                AstraSimpleSDK.streamManager.SetDepthMode(mode);
                break;
            }
        }
    }

    void SetColorMode()
    {
        Vector2 size = GetColorSize();

        Astra.ImageMode[] modes = AstraSimpleSDK.streamManager.GetAvailableColorModes();
        foreach (var mode in modes)
        {
            if (mode.Width == size.x && mode.Height == size.y && mode.FramesPerSecond == 30)
            {
                AstraSimpleSDK.streamManager.SetColorMode(mode);
                break;
            }
        }
    }

    void ChangeDepthStream()
    {
        bool isOn = DepthStreamToggle.isOn;
        DepthImage.gameObject.SetActive(isOn);
        if (isOn)
        {
            AstraSimpleSDK.streamManager.OpenStream(StreamType.Depth);
            DepthImage.texture = AstraSimpleSDK.streamManager.GetStreamData().DepthData.texture;
        }
        else
        {
            AstraSimpleSDK.streamManager.CloseStream(StreamType.Depth);
            //            DepthImage.texture = null;
        }
    }
    void ChangeColorStream()
    {
        bool isOn = ColorStreamToggle.isOn;
        ColorImage.gameObject.SetActive(isOn);
        if (isOn)
        {
            AstraSimpleSDK.streamManager.OpenStream(StreamType.Color);
            ColorImage.texture = AstraSimpleSDK.streamManager.GetStreamData().ColorData.texture;
        }
        else
        {
            AstraSimpleSDK.streamManager.CloseStream(StreamType.Color);
            //            ColorImage.texture = null;
        }
    }
    void ChangeMaskedColorStream()
    {
        bool isOn = MaskedColorStreamToggle.isOn;
        MaskedColorImage.gameObject.SetActive(isOn);
        if (isOn)
        {
            AstraSimpleSDK.streamManager.OpenStream(StreamType.MaskedColor);
            MaskedColorImage.texture = AstraSimpleSDK.streamManager.GetStreamData().MaskedColorData.texture;
        }
        else
        {
            AstraSimpleSDK.streamManager.CloseStream(StreamType.MaskedColor);
            //            MaskedColorImage.texture = null;
        }
    }
    void ChangeColorizedBodyStream()
    {
        bool isOn = ColorizedBodyStreamToggle.isOn;
        ColorizedBodyImage.gameObject.SetActive(isOn);
        if (isOn)
        {
            AstraSimpleSDK.streamManager.OpenStream(StreamType.ColorizedBody);
            ColorizedBodyImage.texture = AstraSimpleSDK.streamManager.GetStreamData().ColorizedBodyData.texture;
        }
        else
        {
            AstraSimpleSDK.streamManager.CloseStream(StreamType.ColorizedBody);
            //            ColorizedBodyImage.texture = null;
        }
    }
    void ChangeBodyStream()
    {
        bool isOn = BodyStreamToggle.isOn;
        JointContainer.gameObject.SetActive(isOn);
        if (isOn)
        {
            AstraSimpleSDK.streamManager.OpenStream(StreamType.Body);

        }
        else
        {
            AstraSimpleSDK.streamManager.CloseStream(StreamType.Body);
        }
    }


    void ChangeDepthTexture()
    {
        bool isOn = DepthTextureToggle.isOn;
        AstraSimpleSDK.streamManager.UsingTexture(TextureType.Depth, isOn);
    }
    void ChangeColorTexture()
    {
        bool isOn = ColorTextureToggle.isOn;
        AstraSimpleSDK.streamManager.UsingTexture(TextureType.Color, isOn);
    }
    void ChangeMaskedColorTexture()
    {
        bool isOn = MaskedColorTextureToggle.isOn;
        AstraSimpleSDK.streamManager.UsingTexture(TextureType.MaskedColor, isOn);
    }
    void ChangeColorizedBodyTexture()
    {
        bool isOn = ColorizedBodyTextureToggle.isOn;
        AstraSimpleSDK.streamManager.UsingTexture(TextureType.ColorizedBody, isOn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GameObject.Find("Camera").transform.position += Vector3.forward * 10;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GameObject.Find("Camera").transform.position += Vector3.back * 10;
        }

        if (JointContainer.gameObject.activeSelf)
        {

            for (int i = 0; i < jointLists.Count; i++)
            {
                for (int z = 0; z < jointLists[i].Count; z++)
                {
                    jointLists[i][z].rectTransform.anchoredPosition = new Vector2(-10000f, 2000.0f);
                }
            }


            StreamData streamData = AstraSimpleSDK.streamManager.GetStreamData();
            if (streamData != null && streamData.BodyData.frameIndex > 0)
            {
                Astra.Body[] bodys = streamData.BodyData.bodies;

                ClearLine();

                for (int z = 0; z < bodys.Length; z++)
                {
                    Astra.Body body = bodys[z];
                    if (body.Status == Astra.BodyStatus.Tracking)
                    {
                        for (int i = 0; i < body.Joints.Length; i++)
                        {
                            Astra.Vector2D depthPos = body.Joints[i].DepthPosition;
                            float dx = depthPos.X - streamData.BodyData.width / 2f;
                            float dy = depthPos.Y - streamData.BodyData.height / 2f;
                            jointLists[z][i].rectTransform.anchoredPosition = new Vector2(dx * 480 / streamData.BodyData.width, dy * 360 / streamData.BodyData.height);
                            if ((int)Astra.JointType.LeftHand == i)
                            {
                                if (body.HandPoseInfo.LeftHand == Astra.HandPose.Grip)
                                {
                                    jointLists[z][i].color = Color.red;
                                }
                                else
                                {
                                    jointLists[z][i].color = Color.cyan;
                                }
                            }

                            if ((int)Astra.JointType.RightHand == i)
                            {
                                if (body.HandPoseInfo.RightHand == Astra.HandPose.Grip)
                                {
                                    jointLists[z][i].color = Color.red;
                                }
                                else
                                {
                                    jointLists[z][i].color = Color.cyan;
                                }


                            }
                        }

                        int index = z;
                        DrawLine(index, body);
                    }
                }
            }
        }
        UpdateFPS();
    }


    void UpdateFPS()
    {
        #region 游戏帧率
        if (_tmpTime == 0)
        {
            _tmpTime = DateTime.Now.Ticks;
        }
        if (DateTime.Now.Ticks - _tmpTime >= 10000000)
        {
            GameFpsText.text = "Game FPS = " + _gameFPS;
            _tmpTime = DateTime.Now.Ticks;
            _gameFPS = 0;
        }
        else
        {
            _gameFPS++;
        }
        #endregion

        #region  骨架帧率
        if (!AstraSimpleSDK.streamManager.isInitialized)
        {
            SkeletonFpsText.text = "Skeleton FPS = " + 0;
            return;
        }

        StreamData streamData = AstraSimpleSDK.streamManager.GetStreamData();

        if (_tmpTime2 == 0)
        {
            _tmpTime2 = DateTime.Now.Ticks;
        }
        if (DateTime.Now.Ticks - _tmpTime2 >= 10000000)
        {
            SkeletonFpsText.text = "Skeleton FPS = " + _skeletonFPS;
            _tmpTime2 = DateTime.Now.Ticks;
            _skeletonFPS = 0;
        }
        else
        {
            if (streamData.BodyData != null)
            {
                if (streamData.BodyData.frameIndex != _lastTimeSkeletonDataId)
                {
                    _skeletonFPS++;
                    _lastTimeSkeletonDataId = streamData.BodyData.frameIndex;
                }
            }
        }
        #endregion

    }

    void ClearLine()
    {
        for (int i = 0; i < jointLists.Count; i++)
        {
            for (int z = 0; z < jointLists[i].Count; z++)
            {
                jointLists[i][z].GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                jointLists[i][z].GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            }
        }
    }

    void DrawLine(int index,Astra.Body body)
    {
        if (body == null) return;
        DrawLine(jointLists[index][(int)Astra.JointType.LeftHand].gameObject, jointLists[index][(int)Astra.JointType.LeftHand].transform.position, jointLists[index][(int)Astra.JointType.LeftElbow].transform.position, Color.cyan);
        DrawLine(jointLists[index][(int)Astra.JointType.LeftElbow].gameObject, jointLists[index][(int)Astra.JointType.LeftElbow].transform.position, jointLists[index][(int)Astra.JointType.LeftShoulder].transform.position, Color.cyan);
        DrawLine(jointLists[index][(int)Astra.JointType.LeftShoulder].gameObject, jointLists[index][(int)Astra.JointType.LeftShoulder].transform.position, jointLists[index][(int)Astra.JointType.ShoulderSpine].transform.position, Color.cyan);
        DrawLine(jointLists[index][(int)Astra.JointType.LeftFoot].gameObject, jointLists[index][(int)Astra.JointType.LeftFoot].transform.position, jointLists[index][(int)Astra.JointType.LeftKnee].transform.position, Color.cyan);
        DrawLine(jointLists[index][(int)Astra.JointType.LeftKnee].gameObject, jointLists[index][(int)Astra.JointType.LeftKnee].transform.position, jointLists[index][(int)Astra.JointType.LeftHip].transform.position, Color.cyan);
        DrawLine(jointLists[index][(int)Astra.JointType.LeftHip].gameObject, jointLists[index][(int)Astra.JointType.LeftHip].transform.position, jointLists[index][(int)Astra.JointType.BaseSpine].transform.position, Color.cyan);

        DrawLine(jointLists[index][(int)Astra.JointType.RightHand].gameObject, jointLists[index][(int)Astra.JointType.RightHand].transform.position, jointLists[index][(int)Astra.JointType.RightElbow].transform.position, Color.red);
        DrawLine(jointLists[index][(int)Astra.JointType.RightElbow].gameObject, jointLists[index][(int)Astra.JointType.RightElbow].transform.position, jointLists[index][(int)Astra.JointType.RightShoulder].transform.position, Color.red);
        DrawLine(jointLists[index][(int)Astra.JointType.RightShoulder].gameObject, jointLists[index][(int)Astra.JointType.RightShoulder].transform.position, jointLists[index][(int)Astra.JointType.ShoulderSpine].transform.position, Color.red);
        DrawLine(jointLists[index][(int)Astra.JointType.RightFoot].gameObject, jointLists[index][(int)Astra.JointType.RightFoot].transform.position, jointLists[index][(int)Astra.JointType.RightKnee].transform.position, Color.red);
        DrawLine(jointLists[index][(int)Astra.JointType.RightKnee].gameObject, jointLists[index][(int)Astra.JointType.RightKnee].transform.position, jointLists[index][(int)Astra.JointType.RightHip].transform.position, Color.red);
        DrawLine(jointLists[index][(int)Astra.JointType.RightHip].gameObject, jointLists[index][(int)Astra.JointType.RightHip].transform.position, jointLists[index][(int)Astra.JointType.BaseSpine].transform.position, Color.red);


        DrawLine(jointLists[index][(int)Astra.JointType.Neck].gameObject, jointLists[index][(int)Astra.JointType.Neck].transform.position, jointLists[index][(int)Astra.JointType.ShoulderSpine].transform.position, Color.yellow);
        DrawLine(jointLists[index][(int)Astra.JointType.Head].gameObject, jointLists[index][(int)Astra.JointType.Head].transform.position, jointLists[index][(int)Astra.JointType.Neck].transform.position, Color.yellow);
        DrawLine(jointLists[index][(int)Astra.JointType.ShoulderSpine].gameObject, jointLists[index][(int)Astra.JointType.ShoulderSpine].transform.position, jointLists[index][(int)Astra.JointType.MidSpine].transform.position, Color.yellow);
        DrawLine(jointLists[index][(int)Astra.JointType.BaseSpine].gameObject, jointLists[index][(int)Astra.JointType.BaseSpine].transform.position, jointLists[index][(int)Astra.JointType.MidSpine].transform.position, Color.yellow);
    }

    void DrawLine(GameObject lineObj, Vector3 worldPos1, Vector3 worldPos2, Color color)
    {
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        worldPos1.z = 0;
        worldPos2.z = 0;
        line.SetPosition(0, worldPos1);
        line.SetPosition(1, worldPos2);
        line.SetWidth(0.7f, 0.7f);

        if (color != Color.black)
        {
            line.SetColors(color, color);
        }
    }

    void Test()
    {

        string data = "xxxx";
        byte[] toByte = System.Text.Encoding.UTF8.GetBytes(data);

        Texture2D tex = new Texture2D(512, 512);
        tex.LoadRawTextureData(toByte);
        tex.Apply();
    }
}

using UnityEngine;
using System.Collections;
using System;
using Astra;

public class StreamViewModel
{
    public Bindable<bool> depthStream = new Bindable<bool>();
    public Bindable<bool> colorStream = new Bindable<bool>();
    public Bindable<bool> bodyStream = new Bindable<bool>();
    public Bindable<bool> colorizedBodyStream = new Bindable<bool>();
    public Bindable<bool> maskedColorStream = new Bindable<bool>();
    public Bindable<bool> depthRecord = new Bindable<bool>();

    private static StreamViewModel _instance;
    public static StreamViewModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StreamViewModel();
            }
            return _instance;
        }
    }

    private StreamViewModel()
    {
        depthStream.onValueChanged += OnDepthStreamChanged;
        colorStream.onValueChanged += OnColorStreamChanged;
        bodyStream.onValueChanged += OnBodyStreamChanged;
        colorizedBodyStream.onValueChanged += OnColorizedBodyStreamChanged;
        maskedColorStream.onValueChanged += OnMaskedColorStreamChanged;
        depthRecord.onValueChanged += OnDepthRecordChanged;
    }

    private void OnDepthStreamChanged(bool value)
    {
        AstraManager.Instance.IsDepthOn = value;
    }

    private void OnColorStreamChanged(bool value)
    {
        AstraManager.Instance.IsColorOn = value;
    }

    private void OnBodyStreamChanged(bool value)
    {
        AstraManager.Instance.IsBodyOn = value;
    }

    private void OnColorizedBodyStreamChanged(bool value)
    {
        AstraManager.Instance.IsColorizedBodyOn = value;
    }

    private void OnMaskedColorStreamChanged(bool value)
    {
        AstraManager.Instance.IsMaskedColorOn = value;
    }

    private void OnDepthRecordChanged(bool value)
    {
        if (value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var path = GetExternalStorageDirectory();
            AstraManager.Instance.DepthStream.StartRecord(path + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".oni");
#else
            AstraManager.Instance.DepthStream.StartRecord(DateTime.Now.ToString("yyyyMMddHHmmss") + ".oni");
#endif
        }
        else
        {
            AstraManager.Instance.DepthStream.StopRecord();
        }
    }

    private String GetExternalStorageDirectory()
    {
        string path = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                using (AndroidJavaClass ajc = new AndroidJavaClass("android.os.Environment"))
                {
                    path = ajc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error fetching native Android internal storage dir: " + e.Message);
            }
        }
        return path;
    }
}

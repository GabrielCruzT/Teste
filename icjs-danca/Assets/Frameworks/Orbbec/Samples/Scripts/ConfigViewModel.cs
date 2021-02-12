using UnityEngine;
using System.Collections;

public class ConfigViewModel
{
    public struct ImageMode
    {
        public int width;
        public int height;

        public ImageMode(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    public Bindable<ImageMode> depthMode = new Bindable<ImageMode>();
    public Bindable<ImageMode> colorMode = new Bindable<ImageMode>();
    public Bindable<bool> depthMirror = new Bindable<bool>();
    public Bindable<bool> colorMirror = new Bindable<bool>();
    public Bindable<Astra.BodyTrackingFeatures> skeletonFeatures = new Bindable<Astra.BodyTrackingFeatures>();
    public Bindable<Astra.SkeletonProfile> skeletonProfile = new Bindable<Astra.SkeletonProfile>();
    public Bindable<Astra.SkeletonOptimization> skeletonOptimization = new Bindable<Astra.SkeletonOptimization>();

    private static ConfigViewModel _instance;
    public static ConfigViewModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConfigViewModel();
            }
            return _instance;
        }
    }

    private ConfigViewModel()
    {
        depthMode.onValueChanged += OnDepthModeChanged;
        colorMode.onValueChanged += OnColorModeChanged;
        depthMirror.onValueChanged += OnDepthMirrorChanged;
        colorMirror.onValueChanged += OnColorMirrorChanged;
        skeletonFeatures.onValueChanged += OnSkeletonFeaturesChanged;
        skeletonProfile.onValueChanged += OnSkeletonProfileChanged;
        skeletonOptimization.onValueChanged += OnSkeletonOptimizationChanged;
    }

    private void OnDepthModeChanged(ImageMode imageMode)
    {
        Astra.ImageMode[] modes = AstraManager.Instance.AvailableDepthModes;
        foreach (var mode in modes)
        {
            if (mode.Width == imageMode.width && mode.Height == imageMode.height)
            {
                AstraManager.Instance.DepthMode = mode;
                break;
            }
        }
    }

    private void OnColorModeChanged(ImageMode imageMode)
    {
        Astra.ImageMode[] modes = AstraManager.Instance.AvailableColorModes;
        foreach (var mode in modes)
        {
            if (mode.Width == imageMode.width && mode.Height == imageMode.height)
            {
                AstraManager.Instance.ColorMode = mode;
                break;
            }
        }
    }

    private void OnDepthMirrorChanged(bool isMirror)
    {
        AstraManager.Instance.DepthStream.IsMirroring = isMirror;
    }

    private void OnColorMirrorChanged(bool isMirror)
    {
        AstraManager.Instance.ColorStream.IsMirroring = isMirror;
    }

    private void OnSkeletonFeaturesChanged(Astra.BodyTrackingFeatures features)
    {
        AstraManager.Instance.BodyStream.SetDefaultBodyFeatures(features);
    }

    private void OnSkeletonProfileChanged(Astra.SkeletonProfile profile)
    {
        AstraManager.Instance.BodyStream.SetSkeletonProfile(profile);
    }

    private void OnSkeletonOptimizationChanged(Astra.SkeletonOptimization optimization)
    {
        AstraManager.Instance.BodyStream.SetSkeletonOptimization(optimization);
    }
}

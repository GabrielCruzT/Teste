using UnityEngine;
using System.Collections;

public static class ApplicationInfo
{

    /// <summary>
    /// 设备型号.
    /// </summary>
    /// <value>The device model.</value>
    public static string deviceModel
    {
        get
        {
            return SystemInfo.deviceModel;
        }
    }

    /// <summary>
    /// 设备标识符（唯一）.
    /// </summary>
    /// <value>The device identifier.</value>
    public static string deviceId
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    /// <summary>
    /// GPU型号.
    /// </summary>
    /// <value>The gpu model.</value>
    public static string gpuModel
    {
        get
        {
            return SystemInfo.graphicsDeviceName;
        }
    }

    /// <summary>
    /// 获取GPU显存（单位M）
    /// </summary>
    /// <value>The size of the gpu memory.</value>
    public static int gpuMemorySize
    {
        get
        {
            return SystemInfo.graphicsMemorySize;
        }
    }

    /// <summary>
    /// 操作系统型号.
    /// </summary>
    /// <value>The operating system.</value>
    public static string operatingSystem
    {
        get
        {
            return SystemInfo.operatingSystem;
        }
    }

    /// <summary>
    /// CPU型号.
    /// </summary>
    /// <value>The cpu model.</value>
    public static string cpuModel
    {
        get
        {
            return SystemInfo.processorType;
        }
    }

    /// <summary>
    /// CPU核数.
    /// </summary>
    /// <value>The cpu count.</value>
    public static int cpuCount
    {
        get
        {
            return SystemInfo.processorCount;
        }
    }

    /// <summary>
    /// CPU频率（单位MHz）.
    /// </summary>
    /// <value>The cpu frequency.</value>
    public static int cpuFrequency
    {
        get
        {
            return SystemInfo.processorFrequency;
        }
    }

    /// <summary>
    /// 系统内存.
    /// </summary>
    /// <value>The size of the system memory.</value>
    public static int systemMemorySize
    {
        get
        {
            return SystemInfo.systemMemorySize;
        }
    }


    /// <summary>
    /// 网络类型.
    /// </summary>
    /// <value>The network reachability.</value>
    public static NetworkReachability networkReachability
    {
        get
        {
            return Application.internetReachability;
        }
    }

    private const string NETWORK_LAN = "LAN/WLAN";
    private const string NETWORK_MOBILE = "2G/3G/4G";
    private const string NETWORK_NONE = "None";

    /// <summary>
    /// 网络类型（string类型）.
    /// </summary>
    /// <value>The state of the network.</value>
    public static string networkState
    {
        get
        {
            NetworkReachability nra = Application.internetReachability;
            if (nra == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return NETWORK_LAN;
            }
            else if (nra == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                return NETWORK_MOBILE;
            }
            else
            {
                return NETWORK_NONE;
            }
        }
    }

    /// <summary>
    /// Unity版本号.
    /// </summary>
    /// <value>The unity version.</value>
    public static string unityVersion
    {
        get
        {
            return Application.unityVersion;
        }
    }

    /// <summary>
    /// 应用版本号.
    /// </summary>
    /// <value>The name of the version.</value>
    public static string appVersion
    {
        get
        {
            return Application.version;
        }
    }

    /// <summary>
    /// 应用包名.
    /// </summary>
    /// <value>The name of the package.</value>
    public static string packageName
    {
        get
        {
            return Application.identifier;
        }
    }

    /// <summary>
    /// 应用产品名.
    /// </summary>
    /// <value>The name of the product.</value>
    public static string productName
    {
        get
        {
            return Application.productName;
        }
    }

}


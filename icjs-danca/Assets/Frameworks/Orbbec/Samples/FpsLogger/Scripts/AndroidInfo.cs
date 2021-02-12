#if UNITY_ANDROID && !UNITY_EDITOR
#define RUNTIME_ANDROID
#endif

using UnityEngine;
using System;
using System.Collections;

namespace OrbbecEx
{
    /// <summary>
    /// 获取一些Android信息
    /// </summary>
    public static class AndroidInfo
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        const string ANDROID_INFO_JAVA_CLASS = "com.orbbec.orbbecex.android.AndroidInfo";
        const string getActivityMetaValue = "getActivityMetaValue";
        const string getApplicationMetaValue = "getApplicationMetaValue";
        const string getPackageName = "getPackageName";
        const string getApplicationName = "getApplicationName";
        const string getDeviceMac = "getDeviceMac";
        const string getLayoutId = "getLayoutId";
        const string getStringId = "getStringId";
        const string getDrawableId = "getDrawableId";
        const string getStyleId = "getStyleId";
        const string getId = "getId";
        const string getColorId = "getColorId";
        const string getStringValue = "getStringValue";
        const string getAppMemory = "getAppMemory";
        const string getAvailMemory = "getAvailMemory";
        const string getCpuRate = "getCpuRate";
        const string getVersionName = "getVersionName";
        const string getVersionCode = "getVersionCode";
        const string getActivityName = "getActivityName";

        const string string2StringSign = "(Ljava/lang/String;)Ljava/lang/String;";
        const string void2StringSign = "()Ljava/lang/String;";
        const string string2IntSign = "(Ljava/lang/String;)I";
        const string void2IntSign = "()I";
        const string void2VoidSign = "()V";

        private static AndroidCall _androidCall;

        static AndroidInfo ()
        {
            _androidCall = new AndroidCall ();
            _androidCall.RegisterClass (ANDROID_INFO_JAVA_CLASS);
        }
        #endif

        public enum CacheMethod
        {
            getActivityMetaValue,
            getApplicationMetaValue,
            getLayoutId,
            getStringId,
            getDrawableId,
            getStyleId,
            getId,
            getColorId,
            getStringValue,
            getAppMemory,
            getAvailMemory,
            getCpuRate,
        }

        public static void AddCacheMethod (CacheMethod method)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            switch (method)
            {
                case CacheMethod.getActivityMetaValue:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getActivityMetaValue, string2StringSign);
                    break;
                case CacheMethod.getApplicationMetaValue:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getApplicationMetaValue, string2StringSign);
                    break;
                case CacheMethod.getLayoutId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getLayoutId, string2IntSign);
                    break;
                case CacheMethod.getStringId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStringId, string2IntSign);
                    break;
                case CacheMethod.getDrawableId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getDrawableId, string2IntSign);
                    break;
                case CacheMethod.getStyleId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStyleId, string2IntSign);
                    break;
                case CacheMethod.getId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getId, string2IntSign);
                    break;
                case CacheMethod.getColorId:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getColorId, string2IntSign);
                    break;
                case CacheMethod.getStringValue:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStringValue, string2StringSign);
                    break;
                case CacheMethod.getAppMemory:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getAppMemory, void2IntSign);
                    break;
                case CacheMethod.getAvailMemory:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getAvailMemory, void2IntSign);
                    break;
                case CacheMethod.getCpuRate:
                    _androidCall.RegisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getCpuRate, void2IntSign);
                    break;
            }
            #else
            Debug.LogError("AndroidInfo.AddCacheMethod : Please use AndroidInfo in Android platform!");
            #endif
        }

        public static void RemoveCacheMethod (CacheMethod method)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            switch (method)
            {
                case CacheMethod.getActivityMetaValue:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getActivityMetaValue, string2StringSign);
                    break;
                case CacheMethod.getApplicationMetaValue:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getApplicationMetaValue, string2StringSign);
                    break;
                case CacheMethod.getLayoutId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getLayoutId, string2IntSign);
                    break;
                case CacheMethod.getStringId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStringId, string2IntSign);
                    break;
                case CacheMethod.getDrawableId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getDrawableId, string2IntSign);
                    break;
                case CacheMethod.getStyleId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStyleId, string2IntSign);
                    break;
                case CacheMethod.getId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getId, string2IntSign);
                    break;
                case CacheMethod.getColorId:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getColorId, string2IntSign);
                    break;
                case CacheMethod.getStringValue:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getStringValue, string2StringSign);
                    break;
                case CacheMethod.getAppMemory:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getAppMemory, void2IntSign);
                    break;
                case CacheMethod.getAvailMemory:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getAvailMemory, void2IntSign);
                    break;
                case CacheMethod.getCpuRate:
                    _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS, getCpuRate, void2IntSign);
                    break;
            }
            #else
            Debug.LogError("AndroidInfo.RemoveCacheMethod : Please use AndroidInfo in Android platform!");
            #endif
        }

        public static void RemoveAllCachedMethods ()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            _androidCall.UnregisterStaticMethod (ANDROID_INFO_JAVA_CLASS);
            #else
            Debug.LogError("AndroidInfo.RemoveAllCachedMethods : Please use AndroidInfo in Android platform!");
            #endif
        }


        /// <summary>
        /// 获取Activity的Meta值
        /// </summary>
        /// <returns>The activity meta value.</returns>
        /// <param name="metaKey">Meta key.</param>
        public static string GetActivityMetaValue (string metaKey)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getActivityMetaValue, string2StringSign, metaKey);
            #else
            Debug.LogError("AndroidInfo.GetActivityMetaValue : Please use AndroidInfo in Android platform!");
            return string.Empty;
            #endif
        }

        /// <summary>
        /// 获取Application的Meta值
        /// </summary>
        /// <returns>The application meta value.</returns>
        /// <param name="metaKey">Meta key.</param>
        public static string GetApplicationMetaValue (string metaKey)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getApplicationMetaValue, string2StringSign, metaKey);
            #else
            Debug.LogError("AndroidInfo.GetApplicationMetaValue : Please use AndroidInfo in Android platform!");
            return string.Empty;
            #endif
        }

        private static string _packageName = string.Empty;

        /// <summary>
        /// App包名.
        /// </summary>
        /// <value>The name of the package.</value>
        public static string packageName
        {
            get
            {
                if (_packageName == string.Empty)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _packageName = _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getPackageName, void2StringSign);
                    #else
                    Debug.LogError("AndroidInfo.packageName : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _packageName;
            }
        }


        private static string _applicationName = string.Empty;

        /// <summary>
        /// Application名.
        /// </summary>
        /// <value>The name of the application.</value>
        public static string applicationName
        {
            get
            {
                if (_applicationName == string.Empty)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _applicationName = _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getApplicationName, void2StringSign);
                    #else
                    Debug.LogError("AndroidInfo.applicationName : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _applicationName;
            }
        }

        private static string _activityName = string.Empty;

        /// <summary>
        /// Activity名.
        /// </summary>
        /// <value>The name of the application.</value>
        public static string activityName
        {
            get
            {
                if (_activityName == string.Empty)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _activityName = _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getActivityName, void2StringSign);
                    #else
                    Debug.LogError("AndroidInfo.activityName : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _activityName;
            }
        }

        private static string _deviceMac = string.Empty;

        /// <summary>
        /// 设备Mac地址.
        /// </summary>
        /// <value>The device mac.</value>
        public static string deviceMac
        {
            get
            {
                if (_deviceMac == string.Empty)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _deviceMac = _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getDeviceMac, void2StringSign);
                    #else
                    Debug.LogError ("AndroidInfo.deviceMac : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _deviceMac;
            }
        }


        /// <summary>
        /// 获取layouts.xml中的字段 id.
        /// </summary>
        /// <returns>The layout identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetLayoutId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getLayoutId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetLayoutId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }

        /// <summary>
        /// 获取strings.xml中的字段 id.
        /// </summary>
        /// <returns>The string identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetStringId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getStringId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetStringId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }



        /// <summary>
        /// 获取Drawable目录中的资源 id.
        /// </summary>
        /// <returns>The drawable identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetDrawableId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getDrawableId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetDrawableId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }

        /// <summary>
        /// 获取styles.xml中的字段 id.
        /// </summary>
        /// <returns>The style identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetStyleId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getStyleId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetStyleId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }

        /// <summary>
        /// 获取id.
        /// </summary>
        /// <returns>The identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }

        /// <summary>
        /// 获取colors.xml中的字段 id.
        /// </summary>
        /// <returns>The color identifier.</returns>
        /// <param name="param">Parameter.</param>
        public static int GetColorId (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getColorId, string2IntSign, param);
            #else
            Debug.LogError("AndroidInfo.GetColorId : Please use AndroidInfo in Android platform!");
            return 0;
            #endif
        }

        /// <summary>
        /// 获取strings.xml中的字段 值.
        /// </summary>
        /// <returns>The string value.</returns>
        /// <param name="param">Parameter.</param>
        public static string GetStringValue (string param)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getStringValue, string2StringSign, param);
            #else
            Debug.LogError("AndroidInfo.GetStringValue : Please use AndroidInfo in Android platform!");
            return null;
            #endif
        }


        /// <summary>
        /// 获取app当前所使用的内存
        /// </summary>
        /// <returns>The app memory.</returns>
        public static int appMemory
        {
            get
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getAppMemory, void2IntSign);
                #else
                Debug.LogError ("AndroidInfo.appMemory : Please use AndroidInfo in Android platform!");
                return 0;
                #endif
            }
        }

        /// <summary>
        /// 获取系统还可用内存
        /// </summary>
        /// <returns>The app memory.</returns>
        public static int availMemory
        {
            get
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getAvailMemory, void2IntSign);
                #else
                Debug.LogError ("AndroidInfo.availMemory : Please use AndroidInfo in Android platform!");
                return 0;
                #endif
            }
        }

        /// <summary>
        /// 获取Cpu使用率
        /// </summary>
        /// <returns>The app memory.</returns>
        public static int cpuRate
        {
            get
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                return _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getCpuRate, void2IntSign);
                #else
                Debug.LogError ("AndroidInfo.cpuRate : Please use AndroidInfo in Android platform!");
                return 0;
                #endif
            }
        }


        private static string _versionName = string.Empty;

        /// <summary>
        /// 版本号.
        /// </summary>
        /// <value>The name of the version.</value>
        public static string versionName
        {
            get
            {
                if (_versionName == string.Empty)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _versionName = _androidCall.CallStatic<string>(ANDROID_INFO_JAVA_CLASS, getVersionName, void2StringSign);
                    #else
                    Debug.LogError("AndroidInfo.versionName : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _versionName;
            }
        }

        private static int _versionCode = -1;

        /// <summary>
        /// 版本号.
        /// </summary>
        /// <value>The name of the version.</value>
        public static int versionCode
        {
            get
            {
                if (_versionCode == -1)
                {
                    #if UNITY_ANDROID && !UNITY_EDITOR
                    _versionCode = _androidCall.CallStatic<int>(ANDROID_INFO_JAVA_CLASS, getVersionCode, void2IntSign);
                    #else
                    Debug.LogError("AndroidInfo.versionCode : Please use AndroidInfo in Android platform!");
                    #endif
                }
                return _versionCode;
            }
        }


    }
}
#if UNITY_ANDROID && !UNITY_EDITOR
#define UNITY_USE_ANDROIDCALL
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OrbbecEx
{
    /*
     * 签名：
     * int                      : I
     * bool                     : Z
     * byte                     : B
     * short                    : S
     * long                     : J
     * float                    : F
     * double                   : D
     * char                     : C
     * object                   : Ljava/lang/Object;
     * string                   : Ljava/lang/String;
     * AndroidJavaRunnable      : Ljava/lang/Runnable;
     * AndroidJavaClass         : Ljava/lang/Class;
     * Array[int]               : [I
     * Array[bool]              : [Z
     * void                     : V
     * AndroidJavaObject实例    : L + 实例的Java类名 + ;   如 Lcom.unity3d.player.UnityPlayerActivity;
     * 
     * 格式 : (参数签名)返回值签名
     * 如：
     * void(bool,int)       : (ZI)V 
     * string()             : ()Ljava/lang/String; 
     * bool(int,string)     : (ILjava/lang/String;)Z
     */

    /// <summary>
    /// 更快的无gc的调用Android的静态方法.
    /// </summary>
    public class AndroidCall
    {
        #if UNITY_USE_ANDROIDCALL

        internal sealed class AndroidJavaException : Exception
        {
            private string mJavaStackTrace;

            public override string StackTrace
            {
                get
                {
                    return this.mJavaStackTrace + base.StackTrace;
                }
            }

            internal AndroidJavaException(string message, string javaStackTrace) : base(message)
            {
                this.mJavaStackTrace = javaStackTrace;
            }
        }

        internal class AndroidCallStaticClassObject
        {
            public AndroidJavaClass javaClass;
            public Dictionary<string, Dictionary<string, IntPtr>> methodDict;
        }

        static object[] _nullObjects;
        static jvalue[] _nullJValues;
        const string _SIGN_VOID = "()V";

        Dictionary<string, AndroidCallStaticClassObject> _classDict;

        static AndroidCall ()
        {
            _nullObjects = new object[0];
            _nullJValues = AndroidJNIHelper.CreateJNIArgArray(_nullObjects);
        }

        #endif

        /// <summary>
        /// 构造一个AndroidCall实例
        /// </summary>
        public AndroidCall ()
        {
            #if UNITY_USE_ANDROIDCALL
            _classDict = new Dictionary<string, AndroidCallStaticClassObject> ();
            #endif
        }

        #if UNITY_USE_ANDROIDCALL
        private IntPtr GetRegisteredStaticMethod(string className, string methodName, string signature)
        {
            
            AndroidCallStaticClassObject classObj = null;
            Dictionary<string, IntPtr> signatureDict = null;

            if (_classDict.ContainsKey (className))
            {
                classObj = _classDict [className];
                if (classObj.methodDict != null && classObj.methodDict.ContainsKey (methodName))
                {
                    signatureDict = classObj.methodDict [methodName];
                    if (signatureDict.ContainsKey (signature))
                    {
                        return signatureDict [signature];
                    }
                }
            }
            return IntPtr.Zero;
        }
        #endif

        /// <summary>
        /// 获取注册的Android类.
        /// </summary>
        /// <returns>The registered class.</returns>
        /// <param name="className">Class name.</param>
        public AndroidJavaClass GetRegisteredClass(string className)
        {
            #if UNITY_USE_ANDROIDCALL
            if (_classDict.ContainsKey (className))
            {
                return _classDict [className].javaClass;
            }
            #endif
            return null;
        }

        /// <summary>
        /// 注册Android类.
        /// </summary>
        /// <param name="className">Class name.</param>
        public void RegisterClass(string className)
        {
            #if UNITY_USE_ANDROIDCALL
            if (!_classDict.ContainsKey (className))
            {
                AndroidJavaClass jclass = new AndroidJavaClass (className);
                AndroidCallStaticClassObject classObj = new AndroidCallStaticClassObject ();
                classObj.javaClass = jclass;
                _classDict.Add (className, classObj);
            }
            #endif
        }

        /// <summary>
        /// 注销Android类.
        /// </summary>
        /// <param name="className">Class name.</param>
        public void UnregisterClass(string className)
        {
            #if UNITY_USE_ANDROIDCALL
            if (_classDict.ContainsKey (className))
            {
                _classDict.Remove (className);
            }
            #endif
        }

        /// <summary>
        /// 注册Android类的方法.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="signature">Signature.</param>
        public void RegisterStaticMethod(string className, string methodName, string signature)
        {
            #if UNITY_USE_ANDROIDCALL
            AndroidCallStaticClassObject classObj = null;
            Dictionary<string, IntPtr> signatureDict = null;

            if (_classDict.ContainsKey (className))
            {
                classObj = _classDict [className];
                if (classObj.methodDict != null && classObj.methodDict.ContainsKey (methodName))
                {
                    signatureDict = classObj.methodDict [methodName];
                    if (signatureDict.ContainsKey (signature))
                    {
                        return;
                    }
                }
            }

            AndroidJavaClass jclass = null;

            if (classObj != null)
            {
                jclass = classObj.javaClass;
            }
            else
            {
                jclass = new AndroidJavaClass (className);
            }

            IntPtr methodID = AndroidJNIHelper.GetMethodID (jclass.GetRawClass(), methodName, signature, true);
            if (methodID == IntPtr.Zero)
            {
                return;
            }

            if (classObj == null)
            {
                classObj = new AndroidCallStaticClassObject ();
                classObj.javaClass = jclass;
                _classDict.Add (className, classObj);
            }

            if (signatureDict == null)
            {
                if (classObj.methodDict == null)
                {
                    classObj.methodDict = new Dictionary<string, Dictionary<string, IntPtr>> ();
                }
                signatureDict = new Dictionary<string, IntPtr> ();
                classObj.methodDict.Add (methodName, signatureDict);
            }

            signatureDict.Add (signature, methodID);
            #endif
        }

        /// <summary>
        /// 注销Android类的方法.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="signature">Signature.</param>
        public void UnregisterStaticMethod(string className, string methodName, string signature)
        {
            #if UNITY_USE_ANDROIDCALL
            AndroidCallStaticClassObject classObj = null;
            Dictionary<string, IntPtr> signatureDict = null;

            if (_classDict.ContainsKey (className))
            {
                classObj = _classDict [className];
                if (classObj.methodDict != null && classObj.methodDict.ContainsKey (methodName))
                {
                    signatureDict = classObj.methodDict [methodName];
                    if (signatureDict.ContainsKey (signature))
                    {
                        signatureDict.Remove (signature);
                    }
                }
            }
            #endif
        }

        /// <summary>
        /// 注销Android类的方法.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        public void UnregisterStaticMethod(string className, string methodName)
        {
            #if UNITY_USE_ANDROIDCALL
            AndroidCallStaticClassObject classObj = null;

            if (_classDict.ContainsKey (className))
            {
                classObj = _classDict [className];
                if (classObj.methodDict != null && classObj.methodDict.ContainsKey (methodName))
                {
                    classObj.methodDict.Remove (methodName);
                }
            }
            #endif
        }

        /// <summary>
        /// 注销Android类的所有方法，但不注销此Android类。
        /// </summary>
        /// <param name="className">Class name.</param>
        public void UnregisterStaticMethod(string className)
        {
            #if UNITY_USE_ANDROIDCALL
            AndroidCallStaticClassObject classObj = null;

            if (_classDict.ContainsKey (className))
            {
                classObj = _classDict [className];
                if (classObj.methodDict != null)
                {
                    classObj.methodDict.Clear ();
                }
            }
            #endif
        }

        /// <summary>
        /// 注销所有Android类.
        /// </summary>
        public void UnregisterAll()
        {
            #if UNITY_USE_ANDROIDCALL
            _classDict.Clear ();
            #endif
        }

        /// <summary>
        /// 调用无参数无返回的Android静态方法.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        public void CallStatic (string className, string methodName)
        {
            #if UNITY_USE_ANDROIDCALL
            IntPtr methodID = GetRegisteredStaticMethod(className, methodName, _SIGN_VOID);
            AndroidJavaClass jClass = GetRegisteredClass (className);
            if (jClass == null)
            {
                jClass = new AndroidJavaClass (className);
            }
            if (methodID == IntPtr.Zero)
            {
                methodID = AndroidJNIHelper.GetMethodID (jClass.GetRawClass(), methodName, _SIGN_VOID, true);
            }
                    
            try
            {
                AndroidJNISafeCallStaticVoidMethod (jClass.GetRawClass(), methodID, _nullJValues);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray (_nullObjects, _nullJValues);
            }
            #else
            Debug.LogError("AndroidCall.CallStatic : Please use AndroidCall in Android platform!");
            #endif
        }

        /// <summary>
        /// 调用带参数无返回的Android静态方法.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="signature">Signature.</param>
        /// <param name="args">Arguments.</param>
        public void CallStatic (string className, string methodName, string signature, params object[] args)
        {
            #if UNITY_USE_ANDROIDCALL
            jvalue[] jarray = AndroidJNIHelper.CreateJNIArgArray (args);
            IntPtr methodID = GetRegisteredStaticMethod(className, methodName, signature);
            AndroidJavaClass jClass = GetRegisteredClass (className);
            if (jClass == null)
            {
                jClass = new AndroidJavaClass (className);
            }
            if (methodID == IntPtr.Zero)
            {
                methodID = AndroidJNIHelper.GetMethodID (jClass.GetRawClass(), methodName, signature, true);
            }

            try
            {
                AndroidJNISafeCallStaticVoidMethod (jClass.GetRawClass(), methodID, jarray);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray (args, jarray);
            }
            #else
            Debug.LogError("AndroidCall.CallStatic : Please use AndroidCall in Android platform!");
            #endif
        }


        //-----------------------------------------------------------------------------------------------

        /// <summary>
        /// 调用无参数带返回值的Android静态方法.
        /// </summary>
        /// <returns>The static.</returns>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="signature">Signature.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T CallStatic<T>(string className, string methodName, string signature)
        {
            #if UNITY_USE_ANDROIDCALL
            object[] args = _nullObjects;
            jvalue[] jarray = _nullJValues;
            return _CallStatic<T> (className, methodName, signature, args, jarray);
            #else
            Debug.LogError("AndroidCall.CallStatic : Please use AndroidCall in Android platform!");
            return default(T);
            #endif
        }

        /// <summary>
        /// 调用带参数带返回值的Android静态方法.
        /// </summary>
        /// <returns>The static.</returns>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="signature">Signature.</param>
        /// <param name="args">Arguments.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T CallStatic<T>(string className, string methodName, string signature, params object[] args)
        {
            #if UNITY_USE_ANDROIDCALL
            jvalue[] jarray = AndroidJNIHelper.CreateJNIArgArray (args);
            return _CallStatic<T> (className, methodName, signature, args, jarray);
            #else
            Debug.LogError("AndroidCall.CallStatic : Please use AndroidCall in Android platform!");
            return default(T);
            #endif
        }

        #if UNITY_USE_ANDROIDCALL
        private T _CallStatic<T>(string className, string methodName, string signature, object[] args, jvalue[] jarray)
        {
            IntPtr methodID = GetRegisteredStaticMethod (className, methodName, signature);
            AndroidJavaClass jClass = GetRegisteredClass (className);
            if (jClass == null)
            {
                jClass = new AndroidJavaClass (className);
            }
            IntPtr jRawClass = jClass.GetRawClass ();
            if (methodID == IntPtr.Zero)
            {
                methodID = AndroidJNIHelper.GetMethodID (jRawClass, methodName, signature, true);
            }

            T result;
            try
            {
                if (typeof(T).IsPrimitive)
                {
                    if (typeof(T) == typeof(int))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticIntMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticBooleanMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(byte))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticByteMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(short))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticShortMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticLongMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticFloatMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticDoubleMethod(jRawClass, methodID, jarray));
                    }
                    else if (typeof(T) == typeof(char))
                    {
                        result = (T)((object)AndroidJNISafeCallStaticCharMethod(jRawClass, methodID, jarray));
                    }
                    else
                    {
                        result = default(T);
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    result = (T)((object)AndroidJNISafeCallStaticStringMethod(jRawClass, methodID, jarray));
                }
                else if (typeof(T) == typeof(AndroidJavaClass))
                {
                    result = default(T);
                    throw new Exception("JNI: Unknown return type '" + typeof(T) + "'");
//                    IntPtr intPtr = AndroidJNISafeCallStaticObjectMethod(jRawClass, methodID, jarray);
//                    result = ((!(intPtr == IntPtr.Zero)) ? ((T)((object)AndroidJavaObjectAndroidJavaClassDeleteLocalRef(intPtr))) : default(T));
                }
                else if (typeof(T) == typeof(AndroidJavaObject))
                {
                    result = default(T);
                    throw new Exception("JNI: Unknown return type '" + typeof(T) + "'");
//                    IntPtr intPtr2 = AndroidJNISafeCallStaticObjectMethod(jRawClass, methodID, jarray);
//                    result = ((!(intPtr2 == IntPtr.Zero)) ? ((T)((object)AndroidJavaObjectAndroidJavaObjectDeleteLocalRef(intPtr2))) : default(T));
                }
                else
                {
//                    if (!AndroidReflection.IsAssignableFrom(typeof(Array), typeof(T)))
                    if (!typeof(Array).IsAssignableFrom(typeof(T)))
                    {
                        throw new Exception("JNI: Unknown return type '" + typeof(T) + "'");
                    }
                    IntPtr intPtr3 = AndroidJNISafeCallStaticObjectMethod(jRawClass, methodID, jarray);
                    result = ((!(intPtr3 == IntPtr.Zero)) ? ((T)((object)AndroidJNIHelper.ConvertFromJNIArray<T>(intPtr3))) : default(T));
                }
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jarray);
            }
            return result;
        }
        #endif

        //-----------------------------------------------------------------------------------------------
        #if UNITY_USE_ANDROIDCALL
        private void AndroidJNISafeCallStaticVoidMethod (IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            try
            {
                AndroidJNI.CallStaticVoidMethod (clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException ();
            }
        }

        private IntPtr AndroidJNISafeCallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            IntPtr result;
            try
            {
                result = AndroidJNI.CallStaticObjectMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private string AndroidJNISafeCallStaticStringMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            string result;
            try
            {
                result = AndroidJNI.CallStaticStringMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private char AndroidJNISafeCallStaticCharMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            char result;
            try
            {
                result = AndroidJNI.CallStaticCharMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private double AndroidJNISafeCallStaticDoubleMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            double result;
            try
            {
                result = AndroidJNI.CallStaticDoubleMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private float AndroidJNISafeCallStaticFloatMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            float result;
            try
            {
                result = AndroidJNI.CallStaticFloatMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private long AndroidJNISafeCallStaticLongMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            long result;
            try
            {
                result = AndroidJNI.CallStaticLongMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private short AndroidJNISafeCallStaticShortMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            short result;
            try
            {
                result = AndroidJNI.CallStaticShortMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private byte AndroidJNISafeCallStaticByteMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            byte result;
            try
            {
                result = AndroidJNI.CallStaticByteMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private bool AndroidJNISafeCallStaticBooleanMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            bool result;
            try
            {
                result = AndroidJNI.CallStaticBooleanMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }

        private int AndroidJNISafeCallStaticIntMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
        {
            int result;
            try
            {
                result = AndroidJNI.CallStaticIntMethod(clazz, methodID, args);
            }
            finally
            {
                AndroidJNISafeCheckException();
            }
            return result;
        }


        private void AndroidJNISafeCheckException ()
        {
            IntPtr intPtr = AndroidJNI.ExceptionOccurred ();
            if (intPtr != IntPtr.Zero)
            {
                AndroidJNI.ExceptionClear ();
                IntPtr intPtr2 = AndroidJNI.FindClass ("java/lang/Throwable");
                IntPtr intPtr3 = AndroidJNI.FindClass ("android/util/Log");
                try
                {
                    IntPtr methodID = AndroidJNI.GetMethodID (intPtr2, "toString", "()Ljava/lang/String;");
                    IntPtr staticMethodID = AndroidJNI.GetStaticMethodID (intPtr3, "getStackTraceString", "(Ljava/lang/Throwable;)Ljava/lang/String;");
                    string message = AndroidJNI.CallStringMethod (intPtr, methodID, new jvalue[0]);
                    jvalue[] array = new jvalue[1];
                    array [0].l = intPtr;
                    string javaStackTrace = AndroidJNI.CallStaticStringMethod (intPtr3, staticMethodID, array);
                    throw new AndroidJavaException (message, javaStackTrace);
                }
                finally
                {
                    AndroidJNISafeDeleteLocalRef (intPtr);
                    AndroidJNISafeDeleteLocalRef (intPtr2);
                    AndroidJNISafeDeleteLocalRef (intPtr3);
                }
            }
        }

        private void AndroidJNISafeDeleteLocalRef (IntPtr localref)
        {
            if (localref != IntPtr.Zero)
            {
                AndroidJNI.DeleteLocalRef (localref);
            }
        }

        #endif

    }
}
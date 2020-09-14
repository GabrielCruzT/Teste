using UnityEngine;
using System;
using System.Collections;

namespace Astra.Simple
{
    public class AstraSimpleSDK
    {

        private static StreamManager _streamManager = null;

        /// <summary>
        /// Gets the stream manager.
        /// </summary>
        /// <value>The stream manager.</value>
        public static StreamManager streamManager
        {
            get 
            {
                if (_streamManager == null)
                {
                    GameObject go = new GameObject ("StreamManager");
                    GameObject.DontDestroyOnLoad (go);
                    _streamManager = go.AddComponent<StreamManager> ();
                }
                return _streamManager;
            }
        }

        //private static DeviceManager _deviceManager = null;

        ///// <summary>
        ///// Gets the device manager.
        ///// </summary>
        ///// <value>The device manager.</value>
        //public static DeviceManager deviceManager
        //{
        //    get 
        //    {
        //        if (_deviceManager == null)
        //            _deviceManager = new DeviceManager ();

        //        return _deviceManager;
        //    }
        //}


    }

}
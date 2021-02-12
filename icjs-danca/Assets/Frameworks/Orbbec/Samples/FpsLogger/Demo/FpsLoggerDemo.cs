using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Text;

namespace OrbbecEx
{
    public class FpsLoggerDemo : MonoBehaviour
    {

        void Start ()
        {
            FpsLogger.Init ();
//            FpsLogger.Init (20,1000,1000,false,200,100,1000,1000,FpsLoggerInfoMode.None);

//            FpsLogger.OpenLogger (FpsLoggerInfoMode.Detail, false);
//            Debug.Log(Application.unityVersion);
//            Debug.Log(Application.version);
//            Debug.Log (Application.bundleIdentifier);
//            Debug.Log (Application.productName);
        }
	
        int frame;

        void Update()
        {
            frame++;

            if (frame == 1)
            {
                FpsLogger.BeginSample ("Sample 1");
            }
            else if (frame == 150)
            {
                FpsLogger.BeginSample ("Sample 1.1");
            }
            else if (frame == 500)
            {
                FpsLogger.EndSample ();
            }
            else if (frame == 1000)
            {
                FpsLogger.BeginSample ("Sample 1.2");
            }
            else if (frame == 1100)
            {
                FpsLogger.BeginSample ("Sample 1.2.1");
            }
            else if (frame == 1500)
            {
                FpsLogger.EndSample ();
            }
            else if (frame == 1600)
            {
                FpsLogger.EndSample ();
            }
            else if (frame == 1800)
            {
                FpsLogger.EndSample ();
            }
            else if (frame >= 2000)
            {
                frame = 0;
            }

            if (Input.GetKeyDown (KeyCode.Escape))
            {
                #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
                #else
                Application.Quit ();
                #endif
            }
        }
    }
}


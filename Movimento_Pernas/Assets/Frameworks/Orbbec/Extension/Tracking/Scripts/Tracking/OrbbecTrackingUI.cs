using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Astra.Tracking
{
    public class OrbbecTrackingUI : MonoBehaviour
    {
        float outNearPointX = 400.0f;
        float outNearPointZ = 1000.0f;
        float outFarPointX = 1200.0f;
        float outFarPointZ = 3000.0f;
        float inNearPointX = 200.0f;
        float inNearPointZ = 1500.0f;
        float inFarPointX = 600.0f;
        float inFarPointZ = 2500.0f;

        float floorWidth = 1220f;
        float floorHeight = 520f;

        [SerializeField] RectTransform userIconRoot = null;
        [SerializeField] RawImage maskImage = null;
//        [SerializeField] Text posText = null;
//        [SerializeField] Text trackingTimeText = null;

        //临时使用的removeList
        List<int> tmpIntList = new List<int>();

        //存放userIcon图标的Dict
        Dictionary<int,OrbbecTrackingUserIcon> iconDict = new Dictionary<int, OrbbecTrackingUserIcon>();

        //存放icon的对象池
        List<OrbbecTrackingUserIcon> iconPool = new List<OrbbecTrackingUserIcon>(6);

        public void SetData(float outNearPointX, float outNearPointZ, float outFarPointX, float outFarPointZ, float inNearPointX, float inNearPointZ, float inFarPointX, float inFarPointZ)
        {
            this.outNearPointX = outNearPointX;
            this.outNearPointZ = outNearPointZ;
            this.outFarPointX = outFarPointX;
            this.outFarPointZ = outFarPointZ;

            this.inNearPointX = inNearPointX;
            this.inNearPointZ = inNearPointZ;
            this.inFarPointX = inFarPointX;
            this.inFarPointZ = inFarPointZ;
        }

        public void SetMaskMap(Texture2D t2d)
        {
            maskImage.texture = t2d;
            maskImage.gameObject.SetActive(true);
        }

//        public void ShowMaskMap()
//        {
//            maskImage.gameObject.SetActive(true);
//        }

        //临时的，之后会废弃掉
//        StringBuilder strb = new StringBuilder(100);

        public void UpdateView(Dictionary<int, OrbbecTrackingUser> trackingDict, float needTrackingTime)
        {
            
            tmpIntList.Clear();

            var trackingUserIconEnumerator = iconDict.GetEnumerator();  
            while (trackingUserIconEnumerator.MoveNext())  
//            foreach (KeyValuePair<int,OrbbecTrackingUserIcon> kvp in iconDict)
            {
                if (!trackingDict.ContainsKey(trackingUserIconEnumerator.Current.Key))
                {
                    tmpIntList.Add(trackingUserIconEnumerator.Current.Key);
                }
            }

            int cnt;
            cnt = tmpIntList.Count;
            for (int i = 0; i < cnt; i++)
            {
                DespawnIcon(iconDict[tmpIntList[i]]);
                iconDict.Remove(tmpIntList[i]);
            }

//            strb.Length = 0;

            //设置节点位置
            var trackingUserEnumerator = trackingDict.GetEnumerator();  
            while (trackingUserEnumerator.MoveNext())  
//            foreach (KeyValuePair<int,OrbbecTrackingUser> kv in trackingDict)
            {
                OrbbecTrackingUserIcon icon = null;

                if (iconDict.ContainsKey(trackingUserEnumerator.Current.Key))
                {
                    icon = iconDict[trackingUserEnumerator.Current.Key];
                }
                else
                {
                    icon = SpawnIcon();
                    icon.rectTransform.SetParent(userIconRoot, false);
                    icon.id = trackingUserEnumerator.Current.Key;
                    iconDict.Add(trackingUserEnumerator.Current.Key, icon);
                }

                icon.isInInBox = trackingUserEnumerator.Current.Value.isInInBox;
                icon.progress = trackingUserEnumerator.Current.Value.trackingTime / needTrackingTime;
                icon.rectTransform.anchoredPosition = GetIconPosition(trackingUserEnumerator.Current.Value.comPoint);

                //无骨架点，隐藏起来
                if (trackingUserEnumerator.Current.Value.comPoint != Vector3.zero)
                {
                    ShowIcon(icon);
                }
                else
                {
                    HideIcon(icon);
                }

//                Debug.Log(string.Format("{0} ({1:f2},  {2:f2},  {3:f2})\n", trackingUserEnumerator.Current.Key, trackingUserEnumerator.Current.Value.comPoint.x, trackingUserEnumerator.Current.Value.comPoint.y, trackingUserEnumerator.Current.Value.comPoint.z));
//                strb.AppendFormat("{0} ({1:f2},  {2:f2},  {3:f2})\n", trackingUserEnumerator.Current.Key, trackingUserEnumerator.Current.Value.comPoint2.x, trackingUserEnumerator.Current.Value.comPoint2.y, trackingUserEnumerator.Current.Value.comPoint2.z);
//                strb.AppendFormat("{0} ({1:f2},  {2:f2},  {3:f2})\n", trackingUserEnumerator.Current.Key, trackingUserEnumerator.Current.Value.comPoint2.x, trackingUserEnumerator.Current.Value.comPoint2.y, trackingUserEnumerator.Current.Value.comPoint2.z);
//                str = str + trackingUserEnumerator.Current.Key + "<color=#00ffffff>" + string.Format(" ({0:f2},  {1:f2},  {2:f2})", trackingUserEnumerator.Current.Value.comPoint3.x, trackingUserEnumerator.Current.Value.comPoint3.y, trackingUserEnumerator.Current.Value.comPoint3.z) + "</color>\n";
            }

//            posText.text = strb.ToString();
//
//            strb.Length = 0;
//            strb.Append("标定时间: ");
//            strb.Append(needTrackingTime);
//            strb.Append("s");
//            trackingTimeText.text = strb.ToString();

        }

        Vector2 GetIconPosition(Vector3 comPoint)
        {
            float y = (comPoint.z - outNearPointZ) / (outFarPointZ - outNearPointZ) * floorHeight;
            y = -Mathf.Clamp(y, 0f, floorHeight);
            float mx = GetOutBorderXByZ(Mathf.Clamp(comPoint.z, outNearPointZ, outFarPointZ));
            float zx = Mathf.Clamp(comPoint.x, -mx, mx);
            float x = zx / outFarPointX * floorWidth * 0.5f;
            return new Vector2(x, y);
        }

        float GetOutBorderXByZ(float z)
        {
            return (z - outNearPointZ) * (outFarPointX - outNearPointX) / (outFarPointZ - outNearPointZ) + outNearPointX;
        }

        float GetInBorderXByZ(float z)
        {
            return (z - inNearPointZ) * (inFarPointX - inNearPointX) / (inFarPointZ - inNearPointZ) + inNearPointX;
        }

        OrbbecTrackingUserIcon SpawnIcon()
        {
            OrbbecTrackingUserIcon icon = null;
            if (iconPool.Count > 0)
            {
                icon = iconPool[iconPool.Count - 1];
                iconPool.RemoveAt(iconPool.Count - 1);
            }
            else
            {
                UnityEngine.Object PrefabObj = Resources.Load("Prefabs/OrbbecTrackingUserIcon");
                GameObject go = UnityEngine.Object.Instantiate(PrefabObj) as GameObject;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.name = "UserIcon";
                icon = go.GetComponent<OrbbecTrackingUserIcon>();
            }

            ShowIcon(icon);
            return icon;
        }

        void DespawnIcon(OrbbecTrackingUserIcon icon)
        {
            HideIcon(icon);
            iconPool.Add(icon);
        }

        void ShowIcon(OrbbecTrackingUserIcon icon)
        {
            icon.gameObject.SetActive(true);
        }

        void HideIcon(OrbbecTrackingUserIcon icon)
        {
            icon.gameObject.SetActive(false);
        }
    }
}
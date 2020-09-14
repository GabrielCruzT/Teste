using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Astra;
using Astra.Simple;

namespace Astra.Tracking
{
    public class OrbbecTrackingManager : MonoBehaviour
    {
        /// <summary>
        /// <para>是否使用标定 [Default true]</para>
        /// <para>比如在纯遥控器操作界面，不需标定不需骨架，则可设置为false节省点CPU。关闭后将无法进行标定，也无法正确获取到骨架信息</para>
        /// </summary>
        public bool isUseTracking = true;

        /// <summary>
        /// <para>是否显示标定UI [Default true]</para>
        /// <para>可在纯遥控器操作界面设置此参数为false，丢标将不会显示标定界面，但还是可以进行标定，如已标定，可进行骨架操作。</para>
        /// </summary>
        public bool isShowTrackingUI = true;

        /// <summary>
        /// <para>标定时间</para>
        /// <para>需要进入蓝色标定框区域内多久才算标定成功。</para>
        /// </summary>
        public float needTrackingTime = 1f;

        /// <summary>
        /// <para>瞬间标定时间</para>
        /// <para>该时间内丢标会瞬间标定，不会出标定界面</para>
        /// </summary>
        public float isImmediatelyTrackingTime = 0.8f; 

        /// <summary>
        /// 是否在标定成功后自动关闭骨架识别. 不能关，关了标定就挂了
        /// </summary>
//        public bool autoCloseTrackingSkeletonWhenTracked = false;

        /// <summary>
        /// 是否在标定成功后自动关闭label图和label数组
        /// </summary>
        public bool autoCloseUserLabelWhenTracked = false;

        // 标定UI界面
        [SerializeField] OrbbecTrackingUI orbbecTrackingUI = null;

        // 策划定的外边框点 靠近摄像头x
        float outNearPointX = 400.0f;
        // 策划定的外边框点 靠近摄像头z
        float outNearPointZ = 1000.0f;
        // 策划定的外边框点 远离摄像头x
        float outFarPointX = 1200.0f;
        // 策划定的外边框点 远离摄像头z
        float outFarPointZ = 3000.0f;

        // 策划定的内边框点 靠近摄像头x
        float inNearPointX = 200.0f;
        // 策划定的内边框点 靠近摄像头z
        float inNearPointZ = 1500.0f;
        // 策划定的内边框点 远离摄像头x
        float inFarPointX = 600.0f;
        // 策划定的内边框点 远离摄像头z
        float inFarPointZ = 2500.0f;

        // 是否初次标定
        bool _firstTracking = true;

        // 是否已显示label图
//        bool isLabelMapShown = false;

        // 标定成功的用户id列表
        List<int> trackedList = new List<int>(10);

        // 标定中的用户信息列表
        Dictionary<int, OrbbecTrackingUser> trackingDict = new Dictionary<int, OrbbecTrackingUser>(10);

        // 临时的一个int数组，标定过程中将反复使用
        List<int> tmpIntList = new List<int>(10);

        // 临时的一个OrbbecTrackingUser数组，用来排序标定权重
        List<OrbbecTrackingUser> sortTrackingUserList = new List<OrbbecTrackingUser>(10);

        //比较方法
        System.Comparison<OrbbecTrackingUser> compareTrackingUser;

        // 玩家1骨架数据
        int _user1Id;

        // 玩家2骨架数据
        int _user2Id;

        // 游戏模式，单人还是双人
        public enum PlayerMode
        {
            single,
            two,
        }

        // 游戏模式，单人还是双人
        int _needPlayerNumber = 1;

        // 单人模式还是双人模式
        PlayerMode _playerMode = PlayerMode.single;

        /// <summary>
        /// <para>游戏模式</para>
        /// <para>单人模式/双人模式</para>
        /// </summary>
        /// <value>The player mode.</value>
        public PlayerMode playerMode
        {
            get
            {
                return _playerMode;
            }
            set
            {
                if (_playerMode == value)
                    return;

                _playerMode = value;

                if (_playerMode == PlayerMode.two)
                {
                    _needPlayerNumber = 2;
                }
                else
                {
                    _needPlayerNumber = 1;
                }
            }
        }

        // 正在标定中
        private bool _isTracking = false;

        /// <summary>
        /// 是否正在标定中
        /// </summary>
        /// <value><c>true</c> if is tracking; otherwise, <c>false</c>.</value>
        public bool isTracking
        {
            get
            {
                return _isTracking;
            }
        }

        // 从OrbbecManager获取的OrbbecUser用户列表
        Dictionary<int, Body> _userTable = new Dictionary<int, Body>();

        /// <summary>
        /// 从OrbbecManager获取的OrbbecUser用户列表
        /// </summary>
        /// <value>The user table.</value>
        public Dictionary<int, Body> userTable
        {
            get
            {
                return _userTable;
            }
        }

        // OrbbecTrackingManager的单例
        private static OrbbecTrackingManager _instance;

        /// <summary>
        /// OrbbecTrackingManager的单例
        /// </summary>
        /// <value>The instance.</value>
        public static OrbbecTrackingManager instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// OrbbecManager是否已经ok
        /// </summary>
        /// <value><c>true</c> if is inited; otherwise, <c>false</c>.</value>
        public bool isInited
        {
            get
            {
                return AstraSimpleSDK.streamManager.isInitialized;
            }
        }

        Body _user1;
        Body _user2;

        /// <summary>
        /// 玩家1的OrbbecUser数据
        /// </summary>
        /// <value>The OrbbecUser user1.</value>
        public Body user1
        {
            get
            {
                if (trackedList.Count >= _needPlayerNumber)
                {
                    return userTable[_user1Id];
                }
                else
                {
                    return _user1;
                }
            }
        }

        /// <summary>
        /// 玩家2的OrbbecUser数据
        /// </summary>
        /// <value>The OrbbecUser user2.</value>
        public Body user2
        {
            get
            {
                if (trackedList.Count >= _needPlayerNumber)
                {
                    return userTable[_user2Id];
                }
                else
                {
                    return _user2;
                }
            }
        }

        /// <summary>
        /// <para>是否标定有效</para>
        /// <para>只有此值为true时，拿取的user1,user2的数据才是有效信息</para>
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        public bool isActive
        {
            get
            {
                if (trackedList.Count >= _needPlayerNumber)
                {
                    return true;
                }
                else
                {
                    if (isImmediatelyTracked)
                    {
                        if (playerMode == PlayerMode.single)
                        {
                            return _user1 != null;
                        }
                        else if (playerMode == PlayerMode.two)
                        {
                            return _user1 != null && _user2 != null;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// <para>OrbbecTrackingManager初始化</para>
        /// <para>在使用此类单例实例前需要调用此方法</para>
        /// </summary>
        public static void Init()
        {
            Debug.Log("OrbbecTrackingManager Init");
            if (_instance == null)
            {
                UnityEngine.Object PrefabObj = Resources.Load("Prefabs/OrbbecTrackingManager");
                GameObject obj = UnityEngine.Object.Instantiate(PrefabObj) as GameObject;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.name = "OrbbecTrackingManager";
                DontDestroyOnLoad(obj);
                _instance = obj.GetComponent<OrbbecTrackingManager>();
            }
        }

        /// <summary>
        /// 是否存在实例
        /// </summary>
        /// <returns><c>true</c> if has instance; otherwise, <c>false</c>.</returns>
        public static bool HasInstance()
        {
            return _instance != null;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public static void Destroy()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        void OnDestroy()
        {
            //if (AstraSimpleSDK.streamManager.isInitialized)
            //{
            //    AstraSimpleSDK.streamManager.SetStreamDataReadyCallBack(null);
            //}
        }

        void Awake()
        {
            //设置比较方法
            compareTrackingUser = new System.Comparison<OrbbecTrackingUser>(CompareTrackingUser);

            // 初始化时给orbbecTrackingUI也设置对应的顶点信息
            if (orbbecTrackingUI != null)
            {
                orbbecTrackingUI.SetData(outNearPointX, outNearPointZ, outFarPointX, outFarPointZ, inNearPointX, inNearPointZ, inFarPointX, inFarPointZ);
            }

            //SetUpdate();

        }

        //void SetUpdate()
        //{
        //    StartCoroutine(_SetUpdate());
        //}
        //IEnumerator _SetUpdate()
        //{
        //    while (true)
        //    {
        //        if (AstraSimpleSDK.streamManager.isInitialized)
        //        {
        //            AstraSimpleSDK.streamManager.SetStreamDataReadyCallBack(UpdateData);
        //            break;
        //        }
        //        yield return new WaitForSeconds(0.5f);
        //    }
        //}

        private void LateUpdate()
        {
            UpdateData();
        }

        float realtimeDeltaTime = 0.0f;
        float realtimeSinceStartup = 0.0f;
        float realtimeSinceTracking = 0.0f;
        bool isImmediatelyTracked = false;

        public void UpdateData()
        {
            if (!isInited)
                return;

            // Debug.Log("OrbbecTrackingManager -> isInited");

            _userTable.Clear();
            StreamData streamData = AstraSimpleSDK.streamManager.GetStreamData();
            if (streamData.BodyData != null && streamData.BodyData.frameIndex != 0)
            {
                Body[] _bodies = AstraSimpleSDK.streamManager.GetStreamData().BodyData.bodies;
                int index = 0;

                for (int i = 0; i < _bodies.Length; i++)
                {
                    index = i;
                    if (_bodies[i].Id != 0)
                    {
                        _userTable.Add(_bodies[index].Id, _bodies[index]);
                    }
                }

            }

            // isUseTracking为false时，不进行检测
            if (!isUseTracking)
            {
                CloseTrackingUI();
                return;
            }

            // Debug.Log("OrbbecTrackingManager -> GetAllOrbbecUsers().Count == " + OrbbecManager.Instance.GetAllOrbbecUsers().Count);

            // 第一次进入，设置ui的labelMap
            if (_firstTracking && orbbecTrackingUI != null)
            {
                //_firstTracking = false;
                if (AstraSimpleSDK.streamManager.GetStreamData().BodyData != null && AstraSimpleSDK.streamManager.GetStreamData().BodyData.frameIndex != 0)
                {
                    orbbecTrackingUI.SetMaskMap(AstraSimpleSDK.streamManager.GetStreamData().ColorizedBodyData.texture);

                }

                //                    GetUserLabelMap());
            }

//            Debug.Log (userTable.Count);

//            Debug.Log (OrbbecManager.Instance.AllOrbbecUsers.Count);

            // Debug.Log("OrbbecTrackingManager -> UpdateTrackedUser");
            // 检测标定的用户是否丢失
            UpdateTrackedUser();

            // Debug.Log("OrbbecTrackingManager -> trackedList.Count == " + trackedList.Count);
            // 当已标定用户数不够时，检测标定
            if (trackedList.Count < _needPlayerNumber)
            {
                //记录时间
                isImmediatelyTracked = false;
                if (realtimeSinceStartup <= 0.0f)
                {
                    realtimeSinceStartup = Time.realtimeSinceStartup;
                }
                realtimeDeltaTime = Time.realtimeSinceStartup - realtimeSinceStartup;
                realtimeSinceStartup = Time.realtimeSinceStartup;
                realtimeSinceTracking += realtimeDeltaTime;
                if (realtimeSinceTracking <= isImmediatelyTrackingTime)
                {
                    isImmediatelyTracked = true;
                }

                // Debug.Log("OrbbecTrackingManager -> UpdateTrackingUser");
                UpdateTrackingUser();

                if (!_isTracking)
                {
                    // 判断瞬间标定
                    _isTracking = true;
                    //AstraSimpleSDK.streamManager.OpenStream(StreamType.Body | StreamType.ColorizedBody);
                    //OrbbecManager.Instance.Param.UsingSkeleton = true;
                    //                    IsTrackingSkeleton = true;
                    //OrbbecManager.Instance.Param.UsingUserLabel = true;


                    //                    IsUseUserLabel = true;
                    //                    CheckTracking(true);
                }
                //                else
                //                {
                // 判断持续标定
                // Debug.Log("OrbbecTrackingManager -> CheckTracking");
                CheckTracking();
//                }
            }

            // Debug.Log("OrbbecTrackingManager -> trackingDict.Count == " + trackingDict.Count);
            if (trackingDict.Count > 0)
            {
//                if (!isLabelMapShown)
//                {
//                    orbbecTrackingUI.ShowMaskMap();
//                    isLabelMapShown = true;
//                }
            }

            if (trackedList.Count >= _needPlayerNumber)
            {
                // 已完成标定，判定user1，user2
                if (_playerMode == PlayerMode.single)
                {
                    _user1Id = trackedList[0];
                    _user1 = userTable[_user1Id];
                }
                else
                {
                    int id1 = trackedList[0];
                    int id2 = trackedList[1];
                    if (userTable[id1].CenterOfMass.X <= userTable[id2].CenterOfMass.X)
                    {
                        _user1Id = id1;
                        _user2Id = id2;
                        _user1 = userTable[_user1Id];
                        _user2 = userTable[_user2Id];
                    }
                    else
                    {
                        _user1Id = id2;
                        _user2Id = id1;
                        _user1 = userTable[_user1Id];
                        _user2 = userTable[_user2Id];
                    }
                }
                // 已完成标定，关闭标定界面
                CloseTrackingUI();
            }
            else
            {
                if (isImmediatelyTracked)
                {
                    CloseTrackingUI();
                }
                else
                {
                    // Debug.Log("OrbbecTrackingManager -> RefreshTrackingView " );
                    // 未完成标定，刷新标定界面
                    RefreshTrackingView();
                }
            }
        }

        /// <summary>
        /// 更新已标定用户信息
        /// </summary>
        void UpdateTrackedUser()
        {
            int i;
            int cnt;

            tmpIntList.Clear();

            // 将trackedList中已经丢失的用户id加入临时列表
            cnt = trackedList.Count;
            for (i = 0; i < cnt; i++)
            {
                Body curBody = null;
                userTable.TryGetValue(trackedList[i], out curBody);
                Vector3 tmpV3 = Vector3.zero;
                if (curBody != null)
                {
                    Vector3D v3D = curBody.Joints[(int)JointType.MidSpine].WorldPosition;
                    tmpV3 = new Vector3(v3D.X, v3D.Y, v3D.Z);
                }

                if (!userTable.ContainsKey(trackedList[i]) || tmpV3 == Vector3.zero)
//                if (!userTable.ContainsKey(trackedList[i]) || userTable[trackedList[i]].Status != BodyStatus.Tracking)
                {
                    tmpIntList.Add(trackedList[i]);
                }
            }

            // 移除trackedList中已经丢失的用户id
            cnt = tmpIntList.Count;
            for (i = 0; i < cnt; i++)
            {
                trackedList.Remove(tmpIntList[i]);
            }
        }

        /// <summary>
        /// 更新参与标定用户信息
        /// </summary>
        void UpdateTrackingUser()
        {
            int i;
            int cnt;
            tmpIntList.Clear();

            // 将trackingDict中已经丢失的用户id加入临时列表

//            Debug.Log (userTable.Count);

            //foreach会造成gc，所以用这个替代
            var trackingUserEnumerator = trackingDict.GetEnumerator();  
            while (trackingUserEnumerator.MoveNext())
            {  
                if (!userTable.ContainsKey(trackingUserEnumerator.Current.Key))
                {
                    tmpIntList.Add(trackingUserEnumerator.Current.Key);
                }
            }  

            // 移除trackingDict中已经丢失的用户id
            cnt = tmpIntList.Count;
            for (i = 0; i < cnt; i++)
            {
                trackingDict.Remove(tmpIntList[i]);
            }

            Vector3 comPoint;
            bool isInInBox = false;
            OrbbecTrackingUser orbbecTrackingUser = null;

            var orbbecUserEnumerator = userTable.GetEnumerator();  
            while (orbbecUserEnumerator.MoveNext())  
//            foreach (KeyValuePair<int,OrbbecUser> kvp in userTable)
            {
                Vector3D v3D= orbbecUserEnumerator.Current.Value.Joints[(int)JointType.MidSpine].WorldPosition;
                comPoint = new Vector3(v3D.X, v3D.Y, v3D.Z);
//                comPoint = orbbecUserEnumerator.Current.Value.GetCOM();
//                if (orbbecUserEnumerator.Current.Value.Status != BodyStatus.Tracking)
//                {
//                    isInInBox = false;
//                }
//                else
//                {
                    isInInBox = CheckInInBox(comPoint);
//                }

                if (trackingDict.ContainsKey(orbbecUserEnumerator.Current.Key))
                {
                    orbbecTrackingUser = trackingDict[orbbecUserEnumerator.Current.Key];
                }
                else
                {
                    orbbecTrackingUser = new OrbbecTrackingUser();
                    orbbecTrackingUser.id = orbbecUserEnumerator.Current.Value.Id;
                    trackingDict.Add(orbbecTrackingUser.id, orbbecTrackingUser);
                }

                orbbecTrackingUser.comPoint = comPoint;
                orbbecTrackingUser.isInInBox = isInInBox;

//                Vector3 comPoint2 = orbbecUserEnumerator.Current.Value.GetCOM();
//                orbbecTrackingUser.comPoint2 = comPoint2;

//                Vector3 comPoint3 = orbbecUserEnumerator.Current.Value.GetJointWorld3D(JointType.RightHand);
//                orbbecTrackingUser.comPoint3 = comPoint3;
            }
        }

        /// <summary>
        /// OrbbecTrackingUser按标定权重排序
        /// </summary>
        /// <returns>Compare -1,0,1</returns>
        /// <param name="tuser1">Tuser1.</param>
        /// <param name="tuser2">Tuser2.</param>
        int CompareTrackingUser(OrbbecTrackingUser tuser1, OrbbecTrackingUser tuser2)
        {
            if (tuser1.isInInBox != tuser2.isInInBox)
            {
                if (tuser1.isInInBox)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                float int1 = Mathf.Abs(tuser1.comPoint.x) + tuser1.comPoint.z;
                float int2 = Mathf.Abs(tuser2.comPoint.x) + tuser2.comPoint.z;
                if (int1 < int2)
                {
                    return -1;
                }
                else if (int1 == int2)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 检测标定
        /// </summary>
        /// <param name="isImmediatelyTracked">If set to <c>true</c> is immediately tracked.</param>
        void CheckTracking()
        {
            int i;
            int cnt;

            //trackingDict中的用户数据按标定权重进行排序
            sortTrackingUserList.Clear();

            //添加数据
            //Dictionary.Values每次产生12b的gc
            //sortTrackingUserList.AddRange(trackingDict.Values);
            var trackingUserEnumerator = trackingDict.GetEnumerator();  
            while (trackingUserEnumerator.MoveNext())
            {
                sortTrackingUserList.Add(trackingUserEnumerator.Current.Value);
            }

            //按权重排序
            sortTrackingUserList.Sort(compareTrackingUser);

            cnt = sortTrackingUserList.Count;
            int needCnt = _needPlayerNumber - trackedList.Count;
            int curCnt = 0;
            for (i = 0; i < cnt; i++)
            {
                //在标定框中，判定权重靠前的user先标定
                if (sortTrackingUserList[i].isInInBox)
                {
                    if (trackedList.Contains(sortTrackingUserList[i].id))
                    {
                        sortTrackingUserList[i].trackingTime = needTrackingTime;
                    }
                    else
                    {
                        if (curCnt < needCnt)
                        {
                            if (isImmediatelyTracked)
                            {
                                sortTrackingUserList[i].trackingTime = needTrackingTime;
                                trackedList.Add(sortTrackingUserList[i].id);
                            }
                            else
                            {
                                sortTrackingUserList[i].trackingTime += realtimeDeltaTime;
                                if (sortTrackingUserList[i].trackingTime >= needTrackingTime)
                                {
                                    trackedList.Add(sortTrackingUserList[i].id);
                                }
                            }
                            curCnt++;
                        }
                        else
                        {
                            sortTrackingUserList[i].trackingTime = 0f;
                        }
                    }
                }

                //不在标定框中，标定时间清0
                else
                {
                    sortTrackingUserList[i].trackingTime = 0f;
                }
            }

            //当已标定数组元素量大于需要标定的用户数，则标定成功
            if (trackedList.Count >= _needPlayerNumber)
            {
                // 标定成功
                _isTracking = false;
                realtimeSinceStartup = 0.0f;
                realtimeSinceTracking = 0.0f;
                trackingDict.Clear();

//                if (autoCloseTrackingSkeletonWhenTracked)
//                {
//                    OrbbecManager.Instance.Param.IsTrackingSkeleton = false;
//                }

                if (autoCloseUserLabelWhenTracked)
                {
                    AstraSimpleSDK.streamManager.CloseStream(StreamType.ColorizedBody);
                    //OrbbecManager.Instance.Param.UsingUserLabel = false;
//                    IsUseUserLabel = false;
                }
                return;
            }
        }

        /// <summary>
        /// 检查是否在外边框盒子中
        /// </summary>
        /// <returns><c>true</c>, if in out box was checked, <c>false</c> otherwise.</returns>
        /// <param name="point">用户的骨架点</param>
        bool CheckInOutBox(Vector3 point)
        {
            if (point.z <= outFarPointZ && point.z >= outNearPointZ)
            {
                float borderX = GetOutBorderXByZ(point.z);
                if (point.x <= borderX && point.x >= -borderX)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否在内边框盒子中
        /// </summary>
        /// <returns><c>true</c>, if in in box was checked, <c>false</c> otherwise.</returns>
        /// <param name="point">用户的骨架点</param>
        bool CheckInInBox(Vector3 point)
        {
            if (point.z <= inFarPointZ && point.z >= inNearPointZ)
            {
                float borderX = GetInBorderXByZ(point.z);
                if (point.x <= borderX && point.x >= -borderX)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得外边框z轴上x的边界
        /// </summary>
        /// <returns>The out border X by z.</returns>
        /// <param name="z">The z coordinate.</param>
        float GetOutBorderXByZ(float z)
        {
            return (z - outNearPointZ) * (outFarPointX - outNearPointX) / (outFarPointZ - outNearPointZ) + outNearPointX;

        }

        /// <summary>
        /// 获得内边框z轴上x的边界
        /// </summary>
        /// <returns>The in border X by z.</returns>
        /// <param name="z">The z coordinate.</param>
        float GetInBorderXByZ(float z)
        {
            return (z - inNearPointZ) * (inFarPointX - inNearPointX) / (inFarPointZ - inNearPointZ) + inNearPointX;
        }

        /// <summary>
        /// 关闭标定UI界面
        /// </summary>
        void CloseTrackingUI()
        {
            if (orbbecTrackingUI != null && orbbecTrackingUI.gameObject.activeSelf)
            {
                orbbecTrackingUI.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 刷新标定UI界面
        /// </summary>
        void RefreshTrackingView()
        {
            if (isShowTrackingUI && orbbecTrackingUI != null)
            {
                orbbecTrackingUI.gameObject.SetActive(true);

//                if (Input.GetKeyDown(KeyCode.LeftArrow))
//                {
//                    if (needTrackingTime <= 1.0f)
//                    {
//                        needTrackingTime = 0.5f;
//                    }
//                    else
//                    {
//                        needTrackingTime -= 1.0f;
//                    }
//                }
//                else if (Input.GetKeyDown(KeyCode.RightArrow))
//                {
//                    if (needTrackingTime < 1.0f)
//                    {
//                        needTrackingTime = 1f;
//                    }
//                    else
//                    {
//                        needTrackingTime += 1.0f;
//                    }
//                }

                // 更新人物id
                orbbecTrackingUI.UpdateView(trackingDict, needTrackingTime);
            }
        }
    }
}
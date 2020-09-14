using UnityEngine;
using System.Collections;

namespace Astra.Tracking
{
    public class OrbbecTrackingUser
    {
        //body id
        public int id;

        //标定时间
        public float trackingTime;

        //当前点位置，胸点
        public Vector3 comPoint;

        //是否在标定区域
        public bool isInInBox;

//        //丢失举手次数
//        public int trackingMissCnt;
//
//        //用于log的com点
//        public Vector3 comPoint2;
//
//        //用于log的右手骨架坐标
//        public Vector3 comPoint3;
    }
}

using UnityEngine;
using System.Collections;

namespace Astra.Simple
{
    public class BodyStreamData 
    {
        /// <summary>
        /// 宽
        /// </summary>
        /// <value>The width.</value>
        public int width = 0;

        /// <summary>
        /// 高
        /// </summary>
        /// <value>The height.</value>
        public int height = 0;

        /// <summary>
        /// 帧序号
        /// </summary>
        /// <value>The index of the frame.</value>
        public long frameIndex = 0;

        /// <summary>
        /// 是否估算的
        /// </summary>
        /// <value><c>true</c> if is estimated; otherwise, <c>false</c>.</value>
        public bool isEstimated = false;

        /// <summary>
        /// body数组
        /// </summary>
        /// <value>The bodies.</value>
        public Astra.Body[] bodies = null;

        /// <summary>
        /// 用户标签数组
        /// </summary>
        /// <value>用户标签数组.</value>
        public byte[] bodyMaskData = null;

        /// <summary>
        /// 用户标签数组-宽
        /// </summary>
        /// <value>The width of the body mask.</value>
        public int bodyMaskWidth = 0;

        /// <summary>
        /// 用户标签数组-高
        /// </summary>
        /// <value>The height of the body mask.</value>
        public int bodyMaskHeight = 0;

        /// <summary>
        /// 地板数据
        /// </summary>
        /// <value>The floor mask data.</value>
        public byte[] floorMaskData = null;

        /// <summary>
        /// 地板数据-宽
        /// </summary>
        /// <value>The width of the floor mask.</value>
        public int floorMaskWidth = 0;

        /// <summary>
        /// 地板数据-高
        /// </summary>
        /// <value>The height of the floor mask.</value>
        public int floorMaskHeight = 0;

        /// <summary>
        /// 地板平面
        /// </summary>
        /// <value>The floor plane.</value>
        public Astra.Plane floorPlane = null;

        /// <summary>
        /// 是否检测到地面
        /// </summary>
        /// <value><c>true</c> if is floor detected; otherwise, <c>false</c>.</value>
        public bool isFloorDetected = false;


    }
}

using UnityEngine;
using System.Collections;

namespace Astra.Simple
{
    public class ImageStreamData<T>
    {
        /// <summary>
        /// 图像数据
        /// </summary>
        /// <value>The data.</value>
        public T[] data = null;

        /// <summary>
        /// 图像数据-高
        /// </summary>
        /// <value>The height.</value>
        public int height = 0;

        /// <summary>
        /// 图像数据-宽
        /// </summary>
        /// <value>The width.</value>
        public int width = 0;

        /// <summary>
        /// 图像数据格式
        /// </summary>
        /// <value>The pixel format.</value>
        public Astra.PixelFormat pixelFormat = Astra.PixelFormat.Unknown;

        /// <summary>
        /// 帧序号
        /// </summary>
        /// <value>The index of the frame.</value>
        public long frameIndex = 0;

        /// <summary>
        /// 图像纹理，只有StreamManager使用对应的UsingTexture为true时才有效
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D texture = null;

    }
}
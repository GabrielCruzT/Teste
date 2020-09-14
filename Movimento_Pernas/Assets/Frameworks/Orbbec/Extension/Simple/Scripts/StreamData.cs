using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Astra;

namespace Astra.Simple
{
    public class StreamData 
    {
        /// <summary>
        /// 获取深度流所返回的数据.(只读)
        /// </summary>
        /// <value>The depth data.</value>
        public DepthStreamData DepthData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取彩色流所返回的数据.(只读)
        /// </summary>
        /// <value>The color data.</value>
        public ColorStreamData ColorData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取IR流所返回的数据.(只读)
        /// </summary>
        /// <value>The IR data.</value>

        public InfraredStreamData InfraredData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取抠图流所返回的数据.(只读)
        /// </summary>
        /// <value>The masked color data.</value>
        public MaskedColorStreamData MaskedColorData
        {
            get;
            private set;
        }

        /// <summary>
        /// 深度背景抠图流返回的数据.(只读)
        /// </summary>
        /// <value>The colorized body data.</value>
        public ColorizedBodyStreamData ColorizedBodyData
        {
            get;
            private set;
        }

        /// <summary>
        /// Body流返回的数据.(只读)
        /// </summary>
        /// <value>The body data.</value>
        public BodyStreamData BodyData
        {
            get;
            private set;
        }


        private StreamManager _streamManager;
        private byte[] _depthTextureBuffer;

        public StreamData(StreamManager streamManager)
        {
            _streamManager = streamManager;
        }

        public void UpdateDepthStreamData(DepthFrame depthFrame, bool updateTexture)
        {
            if (_streamManager.IsStreamOpend(StreamType.Depth))
            {
                if (DepthData == null)
                {
                    DepthData = new DepthStreamData();
                    DepthData.texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
                }
                if (depthFrame == null)
                {
                    return;
                }
                // 拷贝深度流数据
                if (DepthData.data == null || DepthData.data.Length != depthFrame.Width * depthFrame.Height)
                {
                    DepthData.data = new short[depthFrame.Width * depthFrame.Height];
                }
                depthFrame.CopyData(ref DepthData.data);
                DepthData.frameIndex = depthFrame.FrameIndex;
                DepthData.width = depthFrame.Width;
                DepthData.height = depthFrame.Height;
                DepthData.pixelFormat = depthFrame.PixelFormat;

                // 深度纹理
                if (updateTexture)
                {
                    if (DepthData.texture == null)
                    {
                        DepthData.texture = new Texture2D(depthFrame.Width, depthFrame.Height, TextureFormat.RGB24, false);
                    }
                    else if (DepthData.texture.width != depthFrame.Width || DepthData.texture.height != depthFrame.Height)
                    {
                        DepthData.texture.Resize(depthFrame.Width, depthFrame.Height, TextureFormat.RGB24, false);
                    }

                    if (_depthTextureBuffer == null || _depthTextureBuffer.Length != depthFrame.Width * depthFrame.Height * 3)
                    {
                        _depthTextureBuffer = new byte[depthFrame.Width * depthFrame.Height * 3];
                    }
                    int length = DepthData.data.Length;
                    for (int i = 0; i < length; i++)
                    {
                        short depth = DepthData.data[i];
                        byte depthByte = (byte)0;
                        if (depth != 0)
                        {
                            depthByte = (byte)(255 - (255 * depth / 10000.0f));
                        }
                        _depthTextureBuffer[i * 3 + 0] = depthByte;
                        _depthTextureBuffer[i * 3 + 1] = depthByte;
                        _depthTextureBuffer[i * 3 + 2] = depthByte;
                    }

                    DepthData.texture.LoadRawTextureData(_depthTextureBuffer);
                    DepthData.texture.Apply();
                }
            }
            else
            {
                DepthData = null;
            }
        }

        public void UpdateColorStreamData(ColorFrame colorFrame, bool updateTexture)
        {
            if (_streamManager.IsStreamOpend(StreamType.Color))
            {
                if (ColorData == null)
                {
                    ColorData = new ColorStreamData();
                    ColorData.texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
                }
                if(colorFrame == null)
                {
                    return;
                }
                // 拷贝彩色流数据
                if (ColorData.data == null || ColorData.data.Length != colorFrame.ByteLength)
                {
                    ColorData.data = new byte[colorFrame.ByteLength];
                }
                colorFrame.CopyData(ref ColorData.data);
                ColorData.frameIndex = colorFrame.FrameIndex;
                ColorData.width = colorFrame.Width;
                ColorData.height = colorFrame.Height;
                ColorData.pixelFormat = colorFrame.PixelFormat;

                // 彩色纹理
                if (updateTexture)
                {
                    if (ColorData.texture == null)
                    {
                        ColorData.texture = new Texture2D(colorFrame.Width, colorFrame.Height, TextureFormat.RGB24, false);
                    }
                    else if (ColorData.texture.width != colorFrame.Width || ColorData.texture.height != colorFrame.Height)
                    {
                        ColorData.texture.Resize(colorFrame.Width, colorFrame.Height, TextureFormat.RGB24, false);
                    }

                    ColorData.texture.LoadRawTextureData(ColorData.data);
                    ColorData.texture.Apply(false);
                }
            }
            else
            {
                ColorData = null;
            }
        }

        private byte[] _infraredTextureBuffer;
        public void UpdateInfraredStreamData(InfraredFrame infraredFrame, bool updateTexture)
        {
            if (_streamManager.IsStreamOpend(StreamType.Infrared))
            {
                if (InfraredData == null)
                {
                    InfraredData = new InfraredStreamData();
                    InfraredData.texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
                }
                if (infraredFrame == null)
                {
                    return;
                }
                // 拷贝彩色流数据
                if (InfraredData.data == null || InfraredData.data.Length != infraredFrame.ByteLength)
                {
                    InfraredData.data = new short[infraredFrame.ByteLength];
                }
                infraredFrame.CopyData(ref InfraredData.data);
                InfraredData.frameIndex = infraredFrame.FrameIndex;
                InfraredData.width = infraredFrame.Width;
                InfraredData.height = infraredFrame.Height;
                InfraredData.pixelFormat = infraredFrame.PixelFormat;

                // 彩色纹理
                if (updateTexture)
                {
                    if (InfraredData.texture == null)
                    {
                        InfraredData.texture = new Texture2D(infraredFrame.Width, infraredFrame.Height, TextureFormat.RGB24, false);
                    }
                    else if (InfraredData.texture.width != infraredFrame.Width || InfraredData.texture.height != infraredFrame.Height)
                    {
                        InfraredData.texture.Resize(infraredFrame.Width, infraredFrame.Height, TextureFormat.RGB24, false);
                    }

                    if (_infraredTextureBuffer == null || _infraredTextureBuffer.Length != infraredFrame.Width * infraredFrame.Height * 3)
                    {
                        _infraredTextureBuffer = new byte[infraredFrame.Width * infraredFrame.Height * 3];
                    }

                    int length = InfraredData.data.Length;
                    for (int i = 0; i < length; i++)
                    {
                        short infrared = InfraredData.data[i];
                        byte infraredByte = (byte)0;
                        if (infrared != 0)
                        {
                            infraredByte = (byte)infrared; ;
                        }
                        _infraredTextureBuffer[i * 3 + 0] = infraredByte;
                        _infraredTextureBuffer[i * 3 + 1] = infraredByte;
                        _infraredTextureBuffer[i * 3 + 2] = infraredByte;
                    }
                    InfraredData.texture.LoadRawTextureData(_infraredTextureBuffer);
                    InfraredData.texture.Apply(false);
                }
            }
            else
            {
                ColorData = null;
            }
        }

        public void UpdateMaskedColorStreamData(MaskedColorFrame maskedColorFrame, bool updateTexture)
        {
            if (_streamManager.IsStreamOpend(StreamType.MaskedColor))
            {
                if (MaskedColorData == null)
                {
                    MaskedColorData = new MaskedColorStreamData();
                    MaskedColorData.texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                }
                if (maskedColorFrame == null)
                {
                    return;
                }
                // 拷贝抠图流数据
                if (MaskedColorData.data == null || MaskedColorData.data.Length != maskedColorFrame.ByteLength)
                {
                    MaskedColorData.data = new byte[maskedColorFrame.ByteLength];
                }
                maskedColorFrame.CopyData(ref MaskedColorData.data);
                MaskedColorData.frameIndex = maskedColorFrame.FrameIndex;
                MaskedColorData.width = maskedColorFrame.Width;
                MaskedColorData.height = maskedColorFrame.Height;
                MaskedColorData.pixelFormat = maskedColorFrame.PixelFormat;

                // 抠图纹理
                if (updateTexture)
                {
                    if (MaskedColorData.texture == null)
                    {
                        MaskedColorData.texture = new Texture2D(maskedColorFrame.Width, maskedColorFrame.Height, TextureFormat.RGBA32, false);
                    }
                    else if (MaskedColorData.texture.width != maskedColorFrame.Width || MaskedColorData.texture.height != maskedColorFrame.Height)
                    {
                        MaskedColorData.texture.Resize(maskedColorFrame.Width, maskedColorFrame.Height, TextureFormat.RGBA32, false);
                    }

                    MaskedColorData.texture.LoadRawTextureData(MaskedColorData.data);
                    MaskedColorData.texture.Apply(false);
                }
            }
            else
            {
                MaskedColorData = null;
            }
        }

        public void UpdateColorizedBodyStreamData(ColorizedBodyFrame colorizedBodyFrame, bool updateTexture)
        {
            if (_streamManager.IsStreamOpend(StreamType.ColorizedBody))
            {
                if (ColorizedBodyData == null)
                {
                    ColorizedBodyData = new ColorizedBodyStreamData();
                    ColorizedBodyData.texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                }
                if (colorizedBodyFrame == null)
                {
                    return;
                }
                // 拷贝label图流数据
                if (ColorizedBodyData.data == null || ColorizedBodyData.data.Length != colorizedBodyFrame.ByteLength)
                {
                    ColorizedBodyData.data = new byte[colorizedBodyFrame.ByteLength];
                }
                colorizedBodyFrame.CopyData(ref ColorizedBodyData.data);
                ColorizedBodyData.frameIndex = colorizedBodyFrame.FrameIndex;
                ColorizedBodyData.width = colorizedBodyFrame.Width;
                ColorizedBodyData.height = colorizedBodyFrame.Height;
                ColorizedBodyData.pixelFormat = colorizedBodyFrame.PixelFormat;

                // label图纹理
                if (updateTexture)
                {
                    if (ColorizedBodyData.texture == null)
                    {
                        ColorizedBodyData.texture = new Texture2D(colorizedBodyFrame.Width, colorizedBodyFrame.Height, TextureFormat.RGBA32, false);
                    }
                    else if (ColorizedBodyData.texture.width != colorizedBodyFrame.Width || ColorizedBodyData.texture.height != colorizedBodyFrame.Height)
                    {
                        ColorizedBodyData.texture.Resize(colorizedBodyFrame.Width, colorizedBodyFrame.Height, TextureFormat.RGBA32, false);
                    }

                    ColorizedBodyData.texture.LoadRawTextureData(ColorizedBodyData.data);
                    ColorizedBodyData.texture.Apply(false);
                }
            }
            else
            {
                ColorizedBodyData = null;
            }
        }

        public void UpdateBodyStreamData(BodyFrame bodyFrame)
        {
            if (_streamManager.IsStreamOpend(StreamType.Body))
            {
                if (BodyData == null)
                {
                    BodyData = new BodyStreamData();
                }
                if (bodyFrame == null)
                {
                    return;
                }
                // 拷贝 body流数据
                BodyData.frameIndex = bodyFrame.FrameIndex;
                BodyData.width = bodyFrame.Width;
                BodyData.height = bodyFrame.Height;
                BodyData.isEstimated = bodyFrame.IsEstimated;
                if (BodyData.bodies == null)
                {
                    BodyData.bodies = new Body[0];
                }
                bodyFrame.CopyBodyData(ref BodyData.bodies);
                if (BodyData.bodyMaskData == null || BodyData.bodyMaskData.Length != bodyFrame.BodyMask.Width * bodyFrame.BodyMask.Height)
                {
                    BodyData.bodyMaskData = new byte[bodyFrame.BodyMask.Width * bodyFrame.BodyMask.Height];
                }
                bodyFrame.BodyMask.CopyData(ref BodyData.bodyMaskData);
                BodyData.bodyMaskWidth = bodyFrame.BodyMask.Width;
                BodyData.bodyMaskHeight = bodyFrame.BodyMask.Height;
                if (BodyData.floorMaskData == null || BodyData.floorMaskData.Length != bodyFrame.FloorInfo.FloorMask.Width * bodyFrame.FloorInfo.FloorMask.Height)
                {
                    BodyData.floorMaskData = new byte[bodyFrame.FloorInfo.FloorMask.Width * bodyFrame.FloorInfo.FloorMask.Height];
                }
                bodyFrame.FloorInfo.FloorMask.CopyData(ref BodyData.floorMaskData);
                BodyData.floorMaskWidth = bodyFrame.FloorInfo.FloorMask.Width;
                BodyData.floorMaskHeight = bodyFrame.FloorInfo.FloorMask.Height;
                BodyData.floorPlane = bodyFrame.FloorInfo.FloorPlane;
                BodyData.isFloorDetected = bodyFrame.FloorInfo.IsFloorDetected;
            }
            else
            {
                BodyData = null;
            }
        }
    }
}

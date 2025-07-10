using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;
using System.Drawing.Imaging;
using System.Drawing;
using Color = UnityEngine.Color;
using System.IO;
public static class Texture2DTool 
{
    #region 图像处理类 裁剪 缩放 旋转 翻转 合并
    /// <summary>
    /// 裁剪Texture2D
    /// </summary>
    /// <param name="originalTexture"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="originalWidth"></param>
    /// <param name="originalHeight"></param>
    /// <returns></returns>
    public static Texture2D ScaleTextureCutOut(Texture2D originalTexture, int offsetX, int offsetY, float originalWidth, float originalHeight)
    {
        Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalWidth), Mathf.CeilToInt(originalHeight));
        int maxX = originalTexture.width - 1;
        int maxY = originalTexture.height - 1;
        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                float targetX = x + offsetX;
                float targetY = y + offsetY;
                int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                int x2 = Mathf.Min(maxX, x1 + 1);
                int y2 = Mathf.Min(maxY, y1 + 1);
                float u = targetX - x1;
                float v = targetY - y1;
                float w1 = (1 - u) * (1 - v);
                float w2 = u * (1 - v);
                float w3 = (1 - u) * v;
                float w4 = u * v;
                Color color1 = originalTexture.GetPixel(x1, y1);
                Color color2 = originalTexture.GetPixel(x2, y1);
                Color color3 = originalTexture.GetPixel(x1, y2);
                Color color4 = originalTexture.GetPixel(x2, y2);
                Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
                                        Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
                                        Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
                                        Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
                                        );
                newTexture.SetPixel(x, y, color);
            }
        }
        newTexture.anisoLevel = 2;
        newTexture.Apply();
        return newTexture;
    }
    /// <summary>
    /// 缩放Textur2D
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns></returns>
    public static Texture2D ScaleTexture(Texture2D source, float targetWidth, float targetHeight)
    {
        Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

        float incX = (1.0f / targetWidth);
        float incY = (1.0f / targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        return result;
    }
    //水平翻转
    public static Texture2D HorizontalFlipTexture(Texture2D texture)
    {
        //得到图片的宽高
        int width = texture.width;
        int height = texture.height;

        Texture2D flipTexture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            flipTexture.SetPixels(i, 0, 1, height, texture.GetPixels(width - i - 1, 0, 1, height));
        }
        flipTexture.Apply();

        return flipTexture;
    }
    // 垂直翻转
    public static Texture2D VerticalFlipTexture(Texture2D texture)
    {
        //得到图片的宽高
        int width = texture.width;
        int height = texture.height;

        Texture2D flipTexture = new Texture2D(width, height);
        for (int i = 0; i < height; i++)
        {
            flipTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
        }
        flipTexture.Apply();
        return flipTexture;
    }
    /// <summary>
    /// 图片逆时针旋转90度
    /// </summary>
    /// <param name="src">原图片二进制数据</param>
    /// <param name="srcW">原图片宽度</param>
    /// <param name="srcH">原图片高度</param>
    /// <param name="desTexture">输出目标图片</param>
    public static Texture2D RotationLeft90(Color32[] src, int srcW, int srcH)
    {
        Color32[] des = new Color32[src.Length];
        Texture2D desTexture = new Texture2D(srcH, srcW);
        //if (desTexture.width != srcH || desTexture.height != srcW)
        //{
        //    desTexture.Resize(srcH, srcW);
        //}

        for (int i = 0; i < srcW; i++)
        {
            for (int j = 0; j < srcH; j++)
            {
                des[i * srcH + j] = src[(srcH - 1 - j) * srcW + i];
            }
        }

        desTexture.SetPixels32(des);
        desTexture.Apply();
        return desTexture;
    }
    /// <summary>
    /// 图片顺时针旋转90度
    /// </summary>
    /// <param name="src">原图片二进制数据</param>
    /// <param name="srcW">原图片宽度</param>
    /// <param name="srcH">原图片高度</param>
    /// <param name="desTexture">输出目标图片</param>
    public static Texture2D RotationRight90(Color32[] src, int srcW, int srcH)
    {

        Color32[] des = new Color32[src.Length];
        Texture2D desTexture = new Texture2D(srcH, srcW);

        for (int i = 0; i < srcH; i++)
        {
            for (int j = 0; j < srcW; j++)
            {
                des[(srcW - j - 1) * srcH + i] = src[i * srcW + j];
            }
        }

        desTexture.SetPixels32(des);
        desTexture.Apply();
        return desTexture;
    }
    /// <summary>
    /// 两张图合并 两张贴图合并，可以实现水印等功能，该代码是我实现3行3列9张相同照片排列 
    /// </summary>
    /// <param name="_baseTexture2D"></param>
    /// <param name="_texture2D"></param>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_w"></param>
    /// <param name="_h"></param>
    /// <returns></returns>
    public static Texture2D MergeImage(Texture2D _baseTexture2D, Texture2D _texture2D, int _x, int _y, int _w, int _h)
    {
        //取图
        Color32[] color = _texture2D.GetPixels32(0);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                _baseTexture2D.SetPixels32(_x + i * (_texture2D.width + _w), _y + j * (_texture2D.height + _h), _texture2D.width, _texture2D.height, color); //宽度
            }
        }
        //应用
        _baseTexture2D.Apply();
        return _baseTexture2D;
    }
    #endregion
    #region 转格式
    public static Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        UnityEngine.Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    public static byte[] TextureToPngBytes(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        UnityEngine.Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        //这里可以转 JPG PNG EXR  Unity都封装了固定的Api
        byte[] bytes = readableText.EncodeToPNG();
        return bytes;
        //RenderTexture.active = previous;
        //RenderTexture.ReleaseTemporary(renderTex);
        //return readableText;
    }

    #endregion
    /// <summary>
    /// 修改图片水平和垂直像素量 Bitmap操作需要用到System.Drawing.dll，请自行下载！
    /// </summary>
    //public static Texture2D SetPictureResolution(string _path)
    //{
    //    Bitmap bm = new Bitmap(_path);
    //    bm.SetResolution(300, 300);
    //    string _idPath = Application.persistentDataPath + "/";
    //    string _name = "print.jpg";
    //    bm.Save(_idPath + _name, ImageFormat.Jpeg);
    //    Texture2D tex = loadTexture(_idPath, _name);
    //    File.WriteAllBytes(Application.persistentDataPath + "/aa.jpg", tex.EncodeToJPG());
    //    bm.Dispose();
    //    return tex;
    //}
}

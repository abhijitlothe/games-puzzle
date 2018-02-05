using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JigsawGame
{
    public class BinaryMaskGenerator
    {
        public static bool[][] GenerateBinaryMask(Texture2D maskImage, int threshold = 0)
        {
            if (maskImage == null)
                return null;

            bool[][] mask = new bool[maskImage.height][];
            for (uint i = 0; i < maskImage.height; ++i)
            {
                mask[i] = new bool[maskImage.width];
            }

            for (int row = 0; row < maskImage.height; ++row)
            {
                for (int col = 0; col < maskImage.width; ++col)
                {
                    Color pixel = maskImage.GetPixel(col, row);
                    mask[row][col] = true;
                    if(System.Math.Abs(pixel.r - Color.black.r) <= threshold 
                       && System.Math.Abs(pixel.g - Color.black.g) <= threshold 
                       && System.Math.Abs(pixel.b - Color.black.b) <= threshold)
                    {
                        mask[row][col] = false;
                    }
                }
            }
            return mask;
        }

        public static Texture2D GenerateTextureFromBinaryMask(bool[][] binaryMask)
        {
            if (binaryMask == null || binaryMask.Length == 0 || binaryMask[0] == null || binaryMask[0].Length == 0)
                return null;

            Texture2D texture = new Texture2D(binaryMask[0].Length, binaryMask.Length, TextureFormat.RGB24, false);
            for (int y = 0; y < texture.height; ++y)
            {
                for (int x = 0; x < texture.width; ++x)
                {
                    if(binaryMask[y][x])
                    {
                        texture.SetPixel(x, y, Color.white);   
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.black);
                    }
                }
            }
            texture.Apply(false, false);
            return texture;
        }
    }
}

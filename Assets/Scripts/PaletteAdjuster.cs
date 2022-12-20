using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PaletteAdjuster : MonoBehaviour
{

    public Texture2D paletteImage;
    Color[] palette;

    public float hueStrength;
    public float satStrength;
    public float valStrength;

    [Range(-1f, 1f)]
    public float exposure, saturation, hueShift;

    public ColorLookup lut;
    public Texture2D defaultLUT;
    public Texture2D save;
    public RawImage image;
    Volume v;

    void Start()
    {
        ManagePalette();
    }

    public void ManagePalette()
    {
        palette = paletteImage.GetPixels();

        v = GetComponent<Volume>();
        v.profile.TryGet(out lut);

        lut.texture.value = EditTexture(defaultLUT);

        if (image != null) image.texture = EditTexture(defaultLUT);
    }

    Texture2D EditTexture(Texture2D input)
    {
        for (int x = 0; x < input.width; x++)
        {
            for (int y = 0; y < input.height; y++)
            {
                save.SetPixel(x, y, ClosestColor(input.GetPixel(x, y)));
            }
        }

        save.Apply();

        return save;
    }

    Color ClosestColor(Color inColor)
    {
        float dist = Mathf.Infinity;
        Color c = Color.white;

        float h2 = 0;
        float s2 = 0;
        float v2 = 0;
        Color.RGBToHSV(inColor, out h2, out s2, out v2);

        h2 += hueShift;
        h2 %= 1f;
        s2 += saturation;
        v2 += exposure;

        foreach (Color color in palette)
        {
            float h1 = 0;
            float s1 = 0;
            float v1 = 0;
            Color.RGBToHSV(color, out h1, out s1, out v1);

            float dh = Mathf.Min(Mathf.Abs(h1 - h2), 1f - Mathf.Abs(h1 - h2)) * 2f * hueStrength;
            float ds = Mathf.Abs(s1 - s2) * satStrength;
            float dv = Mathf.Abs(v1 - v2) * valStrength;


            float cDist = Mathf.Sqrt(dh * dh + ds * ds + dv * dv);

            if (cDist < dist)
            {
                c = color;
                dist = cDist;
            }
        }

        c.a = 1;
        return c;
    }
}

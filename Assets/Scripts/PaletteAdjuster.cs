using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PaletteAdjuster : MonoBehaviour
{
    public static PaletteAdjuster Instance;

    public Texture2D paletteImage;
    Texture2D oldPallete;
    Color[] palette;

    public ColorLookup lut;
    public Texture2D defaultLUT;
    public Texture2D save;
    public RawImage image;
    Volume v;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ManagePalette();
    }

    private void Update()
    {
        if (oldPallete != paletteImage) ManagePalette();
    }

    public void ManagePalette()
    {
        palette = paletteImage.GetPixels();
        oldPallete = paletteImage;

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

        foreach (Color color in palette)
        {
            float cDist = Vector3.Distance(new Vector3(inColor.r, inColor.g, inColor.b), new Vector3(color.r, color.g, color.b));

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

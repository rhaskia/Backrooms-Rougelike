using Cyan;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    bool open;

    public GameObject[] menus;
    public UniversalRendererData URPrenderer;

    public GameObject UI;

    [Header("Graphics")]
    public Slider sphere;
    public Toggle lines;
    public Slider noise;
    public TMP_Dropdown pallete;
    public Slider chromatic;
    public Sprite[] palSprites;
    PaletteAdjuster paletteAdjuster;

    bool setup = false;

    public int vl;

    // Start is called before the first frame update
    void Start()
    {
        paletteAdjuster = FindObjectOfType<PaletteAdjuster>();

        noise.value = PlayerPrefs.GetFloat("ScreenNoise", 0.2f);
        sphere.value = PlayerPrefs.GetFloat("ScreenCurve", 1.8f);
        lines.isOn = PlayerPrefs.GetInt("ScreenLines", 0) == 1;
        chromatic.value = PlayerPrefs.GetFloat("ChromaticAbberation", 0);

        pallete.value = 4;

        setup = true;

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var pal in palSprites)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(pal.name, pal);
            options.Add(option);
        }

        pallete.AddOptions(options);

        vl = PlayerPrefs.GetInt("Palette", 15);

        pallete.value = vl;

        UpdatePallete();
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) open = !open;

        if (UI != null)
        {
            UI.SetActive(!open);
        }

        transform.GetChild(0).gameObject.SetActive(open);
    }

    public void SwitchMenu()
    {
        open = !open;
    }

    public void UpdatePallete()
    {
        if (!setup) return;

        paletteAdjuster.active = pallete.value != 0;

        if (pallete.value != 0) paletteAdjuster.paletteImage = palSprites[pallete.value - 1].texture;

        PlayerPrefs.SetInt("Palette", pallete.value);
        PlayerPrefs.Save();
    }

    public void UpdateMaterial()
    {
        if (!setup) return;

        //Set up
        var blit = URPrenderer.rendererFeatures.OfType<Blit>().FirstOrDefault();
        if (blit == null) return;

        //Editing Material
        Material mat = blit.settings.blitMaterial;

        mat.SetFloat("_ScanlinesStrength", lines.isOn ? 1f : 0f);
        mat.SetFloat("_SphereStrength", sphere.value);
        mat.SetFloat("_Noise", noise.value);

        blit.settings.blitMaterial = mat;

        FindObjectOfType<PaletteAdjuster>().chrom.intensity.value = chromatic.value;

        URPrenderer.SetDirty();

        PlayerPrefs.SetFloat("ScreenNoise", noise.value);
        PlayerPrefs.SetFloat("ScreenCurve", sphere.value);
        PlayerPrefs.SetInt("ScreenLines", lines.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("ChromaticAbberation", chromatic.value);
        PlayerPrefs.Save();
    }

    public void OpenMenu(int n)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == n);
        }
    }
}

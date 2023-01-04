using Cyan;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
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

    public void UpdateMaterial()
    {
        //Set up
        var blit = URPrenderer.rendererFeatures.OfType<Blit>().FirstOrDefault();
        if (blit == null) return;

        //Editing Material
        Material mat = blit.settings.blitMaterial;

        mat.SetFloat("_ScanlinesStrength", lines.isOn ? 1f : 0f);
        mat.SetFloat("_SphereStrength", sphere.value);
        mat.SetFloat("_Noise", noise.value);

        blit.settings.blitMaterial = mat;

        URPrenderer.SetDirty();
    }

    public void OpenMenu(int n)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == n);
        }
    }
}

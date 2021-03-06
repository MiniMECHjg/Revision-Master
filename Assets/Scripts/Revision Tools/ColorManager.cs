using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        change_colours();
    }

    public void change_colours()
    {
        Color32 primaryColor = SaveManager.Instance.primary_colour();
        Color32 accentColor = SaveManager.Instance.accent_colour();
        Color32 textColor = SaveManager.Instance.text_colour();

        GameObject[] primarys = GameObject.FindGameObjectsWithTag("Primary");
        GameObject[] accents = GameObject.FindGameObjectsWithTag("Accent");
        GameObject[] primaryshadows = GameObject.FindGameObjectsWithTag("PrimaryShadow");
        GameObject[] accentshadows = GameObject.FindGameObjectsWithTag("AccentShadow");
        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        GameObject[] images = GameObject.FindGameObjectsWithTag("Image");

        foreach(GameObject panel in primarys)
        {
            Image img =  panel.GetComponent<Image>();
            img.color = SaveManager.Instance.primary_colour();
            
        }

        foreach(GameObject panel in accents)
        {
            Image img =  panel.GetComponent<Image>();
            img.color = SaveManager.Instance.accent_colour();
        }

        foreach(GameObject panel in primaryshadows)
        {
            Image img =  panel.GetComponent<Image>();
            Color32 color = SaveManager.Instance.primary_colour();
            img.color = color;
            color[3] = 75;
            panel.GetComponent<Shadow>().effectColor = color;
            
        }

        foreach(GameObject panel in accentshadows)
        {
            Image img =  panel.GetComponent<Image>();
            Color32 color = SaveManager.Instance.accent_colour();
            img.color = color;
            color[3] = 75;
            panel.GetComponent<Shadow>().effectColor = color;
        }

        foreach(GameObject panel in texts)
        {
            Text img = panel.GetComponent<Text>();
            img.color = SaveManager.Instance.text_colour();
        }

        foreach(GameObject panel in images)
        {
            Image img = panel.GetComponent<Image>();
            img.color = SaveManager.Instance.text_colour();
        }
    }

    public void set_colour_primary(Color32 i)
    {
        SaveManager.Instance.change_primary(i);
        change_colours();
    }

    public void set_colour_accents(Color32 i)
    {
        SaveManager.Instance.change_accent(i);
        change_colours();
    }

    public void set_colour_text(Color32 i)
    {
        SaveManager.Instance.change_text(i);
        change_colours();
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class colourSliders : MonoBehaviour
{

    public GameObject[] sliders;
    //[r, g, b]
    public Text[] values;
    
    public Animator colourWindow;
    public int currentColour = 0;
    //current colour tells what color you are chaging e.g. primary accent or text
    public Image colourSquare;

    public Animator importWindow;

    private string txtFileType;

    public void Start()
    {
        txtFileType = NativeFilePicker.ConvertExtensionToFileType( "txt" ); // Returns "application/txt" on Android and "com.adobe.txt" on iOS
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_back_to_lastPage();
        }
    }

    public void draw_square()
    {
        int r = Int32.Parse(values[0].text);
        int g = Int32.Parse(values[1].text);
        int b = Int32.Parse(values[2].text);
        colourSquare.color = new Color(r/255f,g/255f,b/255f,1);
    }

    public void slider_start()
    {
        if(currentColour == 0)
        {
            for (int count = 0; count < 3; count++)
            {
                int colourNum = SaveManager.Instance.primary_colour()[count];
                sliders[count].GetComponent<Slider>().value = colourNum;
                values[count].text = colourNum.ToString();
            }
        }else if (currentColour == 1)
        {
            for (int count = 0; count < 3; count++)
            {
                int colourNum = SaveManager.Instance.accent_colour()[count];
                sliders[count].GetComponent<Slider>().value = colourNum;
                values[count].text = colourNum.ToString();
            }
        }else if (currentColour == 2)
        {
            for (int count = 0; count < 3; count++)
            {
                int colourNum = SaveManager.Instance.text_colour()[count];
                sliders[count].GetComponent<Slider>().value = colourNum;
                values[count].text = colourNum.ToString();
            }
        }
        draw_square();
    }

    public void change_slider(int i)
    {
        values[i].text = (sliders[i].GetComponent<Slider>().value).ToString();
        draw_square();
        //when you move a slider
    }

    public void open_window(int i)
    {
        currentColour = i;
        slider_start();
        colourWindow.SetTrigger("open");
    }

    public void close_window()
    {
        colourWindow.SetTrigger("close");
    }

    public void open_import_window()
    {
        importWindow.SetTrigger("open");
    }

    public void close_import_window()
    {
        importWindow.SetTrigger("close");
    }

    public void export_file()
    {
        string text = "Theme\n";

        //write the file by adding to text. Find each value. Primary/n accent/n text
        Color32 colourPrimary = SaveManager.Instance.primary_colour();
        Color32 colourAccent = SaveManager.Instance.accent_colour();
        Color32 colourText = SaveManager.Instance.text_colour();

        text += colourPrimary[0].ToString() + "," + colourPrimary[1].ToString() + "," + colourPrimary[2].ToString() + "\n";
        text += colourAccent[0].ToString() + "," + colourAccent[1].ToString() + "," + colourAccent[2].ToString() + "\n";
        text += colourText[0].ToString() + "," + colourText[1].ToString() + "," + colourText[2].ToString();

    
        string filePath = Path.Combine( Application.temporaryCachePath, "Theme.txt" );
        File.WriteAllText( filePath, text);

        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( filePath, ( success ) => Debug.Log( "File exported: " + success ) );

        Debug.Log( "Permission result: " + permission );
        close_import_window();
    }

    public void import_theme(string[] Lines)
    {
        if(Lines[0] == "Theme")
        {
            List<List<int>> colourList = new List<List<int>>();

            Debug.Log(Lines.Length - 1);

            for(int x = 1; x != Lines.Length; x++)
            {
                char[] seperators = {',', '¬', '`'};
                Debug.Log(Lines[x]);
                string[] colours = Lines[x].Split(seperators);

                //Logic for changing the colours
                Debug.Log(colours[0]);

                colourList.Add(new List<int>());
                colourList[x - 1].Add(Int32.Parse(colours[0].ToString()));
                colourList[x - 1].Add(Int32.Parse(colours[1].ToString()));
                colourList[x - 1].Add(Int32.Parse(colours[2].ToString()));


            }

            Color32 primaryColour = new Color(colourList[0][0]/255f, colourList[0][1]/255f, colourList[0][2]/255f, 1);
            Color32 accentColour = new Color(colourList[1][0]/255f, colourList[1][1]/255f, colourList[1][2]/255f, 1);
            Color32 textColour = new Color(colourList[2][0]/255f, colourList[2][1]/255f, colourList[2][2]/255f, 1);

            SaveManager.Instance.change_primary(primaryColour);
            SaveManager.Instance.change_accent(accentColour);
            SaveManager.Instance.change_text(textColour);
            FindObjectOfType<ColorManager>().change_colours();
        }else
        {
            Debug.Log("This will not work");
        }
        close_import_window();
    }

    public void import_file()
    {
        if( NativeFilePicker.IsFilePickerBusy() )
			return;

		NativeFilePicker.Permission permission = NativeFilePicker.PickFile( ( path ) =>
		{
			if( path == null )
				Debug.Log( "Operation cancelled" );
			else
				Debug.Log( "Picked file: " + path );

            string[] Lines = File.ReadAllLines(path);
            import_theme(Lines);

		}, new string[] { txtFileType } );

		Debug.Log( "Permission result: " + permission );
    }

    public void enter_colour()
    {
        int r = Int32.Parse(values[0].text);
        int g = Int32.Parse(values[1].text);
        int b = Int32.Parse(values[2].text);
        Color32 newColour = new Color(r/255f, g/255f, b/255f, 1);
        if (currentColour == 0)
        {
            SaveManager.Instance.change_primary(newColour);
        }else if(currentColour == 1)
        {
            SaveManager.Instance.change_accent(newColour);
        }else if(currentColour == 2)
        {
            SaveManager.Instance.change_text(newColour);
        }
        FindObjectOfType<ColorManager>().change_colours();
        colourWindow.SetTrigger("close");
    }

    
}

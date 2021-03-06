using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class HomeManager : MonoBehaviour
{

    public GameObject overallContainerGO;
    public GameObject container;
    public GameObject subjectPrefab;
    public GameObject addSubjectPrefab;
    public float scaleFactor;
    public Animator edit_window;
    public int currentPressed;

    public Animator importWindow;
    private string txtFileType;

    public int[] oldResolution = new int[2] {Screen.width, Screen.height};

    // Start is called before the first frame update
    void Start()
    {

        txtFileType = NativeFilePicker.ConvertExtensionToFileType( "txt" ); // Returns "application/txt" on Android and "com.adobe.txt" on iOS

        float scaleRatioHeight = FindObjectOfType<scaler>().heightRatio();
        float scaleRatioWidth = FindObjectOfType<scaler>().widthRatio();

        if (scaleRatioHeight <= scaleRatioWidth)
        {
            scaleFactor = scaleRatioHeight;
        }
        else
        {
            scaleFactor = scaleRatioWidth;
        }

        List<string> subjects = SaveManager.Instance.subjects();

        for(int i = 0; i != subjects.Count; i++)
        {
            GameObject subject_btn = Instantiate(subjectPrefab) as GameObject;
            subject_btn.transform.SetParent(container.transform, false);
            GameObject child = subject_btn.transform.GetChild(0).GetChild(0).gameObject;
            int index = i;
            child.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.subject_title(index);
            subject_btn.GetComponent<Button>().onClick.AddListener(() => press_subject(index));
            child = subject_btn.transform.GetChild(1).gameObject;
            child.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
        }
        FindObjectOfType<ColorManager>().change_colours();
        FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
    }

    public void press_subject(int i) 
    {
        SaveManager.Instance.set_current_subject(i);
    }

    public void open_edit_window(int i)
    {
        currentPressed = i;
        edit_window.SetTrigger("open");
    }

    public void close_edit_window()
    {
        edit_window.SetTrigger("close");
    }

    public void open_import_window()
    {
        importWindow.SetTrigger("open");
    }

    public void close_import_window()
    {
        importWindow.SetTrigger("close");
    }

    public void import_subject(string[] Lines)
    {
        try
        {
            if(Lines[0] == "Subject" && (Lines[2] == "0" || Lines[2] == "1"))
            {
                SaveManager.Instance.add_subject(Lines[1], Int32.Parse(Lines[2]));
                SaveManager.Instance.set_current_subject(SaveManager.Instance.subjects().Count - 1);
                SaveManager.Instance.set_current_topic(SaveManager.Instance.num_of_topics()-1);
                for(int x = 3; x != Lines.Length; x++)
                {
                    if(Lines[2] == "1")
                    {
                        if(Lines[x].Substring(0,1) == "`")
                        {
                            char[] seperators = {'¬'};

                            string[] question = Lines[x].Split(seperators);

                            question[0] = question[0].Substring(1);

                            if(question[0].Substring(0) == "0")
                            {
                                SaveManager.Instance.add_definition(question[1], question[2]);
                            } else if (question[0].Substring(0) == "1")
                            {
                                SaveManager.Instance.add_flash_card(question[1], question[2]);
                            } else if (question[0].Substring(0) == "2")
                            {
                                SaveManager.Instance.add_choice2(question[1], question[2], question[3]);
                            } else if (question[0].Substring(0) == "3")
                            {
                                SaveManager.Instance.add_choice5(question[1], question[2], question[3], question[4], question[5]);
                            }
                        }
                        else
                        {
                            SaveManager.Instance.add_topic(Lines[x]);
                            SaveManager.Instance.set_current_topic(SaveManager.Instance.num_of_topics() - 2);
                        }
                    }else if (Lines[2] == "0")
                    {
                        char[] seperators = {'¬'};

                        string[] question = Lines[x].Split(seperators);


                        if(question[0] == "0")
                        {
                            SaveManager.Instance.add_definition(question[1], question[2]);
                        } else if (question[0] == "1")
                        {
                            SaveManager.Instance.add_flash_card(question[1], question[2]);
                        } else if (question[0] == "2")
                        {
                            SaveManager.Instance.add_choice2(question[1], question[2], question[3]);
                        } else if (question[0] == "3")
                        {
                            SaveManager.Instance.add_choice5(question[1], question[2], question[3], question[4], question[5]);
                        }
                    }
                }
                FindObjectOfType<Scenes>().load_current_page();
            }else
            {
                Debug.Log("This will not work");
            }
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }


        //This needs to be re-done
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

            string text = File.ReadAllText(path);
            string[] seperators = new string[] {"|$\n"};
            string[] Lines = text.Split(seperators, StringSplitOptions.None);
            Debug.Log(Lines[1]);
            import_subject(Lines);

		}, new string[] { txtFileType } );

		Debug.Log( "Permission result: " + permission );
    }

    public void delete()
    {
        List<string> subjects = SaveManager.Instance.subjects();
        subjects.RemoveAt(currentPressed);
        SaveManager.Instance.update_subjects(subjects);
        List<List<List<List<string>>>> baseQuestionArray = SaveManager.Instance.base_question_list();
        baseQuestionArray.RemoveAt(currentPressed);
        SaveManager.Instance.set_base_question_list(baseQuestionArray);
        SaveManager.Instance.delete_topic_value(currentPressed);
        SaveManager.Instance.delete_all_topics(currentPressed);
        
        close_edit_window();
        FindObjectOfType<Scenes>().load_current_page();
    }

    public void edit_subject()
    {
        SaveManager.Instance.set_current_subject(currentPressed);
        SaveManager.Instance.set_build_mode(true);
        FindObjectOfType<Scenes>().new_subject();
        SaveManager.Instance.set_question_index(currentPressed);
    }

    void Update()
    {
        int x = Screen.width;
        int y = Screen.height;
        if(x != oldResolution[0] || y != oldResolution[1])
        {
            FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
            oldResolution[0] = x;
            oldResolution[1] = y;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_home();
        }
    }

}

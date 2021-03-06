using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class subHome : MonoBehaviour
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

    public Animator addTopicWindow;
    public GameObject topicTitle;
    public GameObject addTopicBtnText;

    public bool editMode = false;

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

        List<string> subjects = SaveManager.Instance.topic_titles();

        Debug.Log(subjects.Count);

        for(int i = 0; i != subjects.Count; i++)
        {
            GameObject subject_btn = Instantiate(subjectPrefab) as GameObject;
            subject_btn.transform.SetParent(container.transform, false);
            GameObject child = subject_btn.transform.GetChild(0).GetChild(0).gameObject;
            int index = i;
            child.GetComponent<UnityEngine.UI.Text>().text = subjects[index];
            subject_btn.GetComponent<Button>().onClick.AddListener(() => press_sub_topic(index));
            child = subject_btn.transform.GetChild(1).gameObject;
            child.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
        }
        FindObjectOfType<ColorManager>().change_colours();
        FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
    }

    public void press_sub_topic(int i) 
    {
        SaveManager.Instance.set_current_topic(i);
        FindObjectOfType<Scenes>().go_to_subject();
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

    public void open_topic_window()
    {
        addTopicWindow.SetTrigger("open");
        if(editMode == false)
        {
            topicTitle.GetComponent<InputField>().text = "";
            addTopicBtnText.GetComponent<Text>().text = "Add Sub-Topic";
        }else
        {
            topicTitle.GetComponent<InputField>().text = SaveManager.Instance.topic_titles()[currentPressed];
            addTopicBtnText.GetComponent<Text>().text = "Edit Sub-Topic";
        }
    }

    public void close_topic_window()
    {
        editMode = false;
        addTopicWindow.SetTrigger("close");
    }

    public void add_topic()
    {
        if (editMode == false)
        {
            SaveManager.Instance.add_topic(topicTitle.GetComponent<InputField>().text);
        }else
        {
            SaveManager.Instance.edit_topic_name(topicTitle.GetComponent<InputField>().text, currentPressed);
        }
        close_topic_window();
        FindObjectOfType<Scenes>().load_current_page();
        
    }

    
    public void export_topics()
    {
        string text = "Subject|$\n";
        text += SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()] + "|$\n";
        SaveManager.Instance.set_current_topic(0);
        string subject_type = SaveManager.Instance.topic_value().ToString();
        text += subject_type;
        if(subject_type == "0")
        {
            for(int x=0; x != SaveManager.Instance.current_questions_for_subject(); x++)
            {
                text += "|$\n";
                List<string> question = SaveManager.Instance.question(x);
                text += question[0] + "¬" + question[1] + "¬" + question[2];
                if (question[0] == "2")
                {
                text += "¬" +  question[3];
                } else if (question[0] == "3")
                {
                    text += "¬" + question[3] + "¬" + question[4] + "¬" + question[5];
                }
            }
        }else
        {
            for(int x=0; x!= SaveManager.Instance.topic_titles().Count; x++)
            {
                Debug.Log(SaveManager.Instance.topic_titles().Count);
                Debug.Log(x);
                text += "|$\n";
                text += SaveManager.Instance.topic_titles()[x];
                SaveManager.Instance.set_current_topic(x);
                for(int index = 0; index != SaveManager.Instance.current_questions_for_subject() + 1; index++)
                {
                    text += "|$\n";
                    Debug.Log(SaveManager.Instance.current_questions_for_subject());
                    List<string> question = SaveManager.Instance.question(index);
                    text += "`" + question[0] + "¬" + question[1] + "¬" + question[2];
                    if (question[0] == "2")
                    {
                    text += "¬" +  question[3];
                    } else if (question[0] == "3")
                    {
                        text += "¬" + question[3] + "¬" + question[4] + "¬" + question[5];
                    }
                }
            }
        }

        string filePath = Path.Combine( Application.temporaryCachePath, "Subject_" + SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()] +".txt" );
        File.WriteAllText( filePath, text);

        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( filePath, ( success ) => Debug.Log( "File exported: " + success ) );

        Debug.Log( "Permission result: " + permission );
    }
    

    public void import_topics(string[] Lines)
    {
        try
        {
            if(Lines[0] == "Subject" && (Lines[2] == "0" || Lines[2] == "1"))
            {
                if(Lines[2] == "0")
                {
                    SaveManager.Instance.add_topic(Lines[1]);
                    SaveManager.Instance.set_current_topic(SaveManager.Instance.num_of_topics() - 2);
                }

                for(int x = 3; x != Lines.Length; x++)
                {
                    if(Lines[2] == "1")
                    {
                        if(Lines[x].Substring(0,1) == "`")
                        {
                            char[] seperators = {'¬'};

                            string[] question = Lines[x].Split(seperators);

                            question[0] = question[0].Substring(1);

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
            import_topics(Lines);

		}, new string[] { txtFileType } );

		Debug.Log( "Permission result: " + permission );
    }

    public void delete()
    {
        //topic titles and questions
        //topicTitles[currentSubject].RemoveAt(currentPressed);
        //questions[currentSubject].RemoveAt(currentPressed);

        SaveManager.Instance.remove_topic_title(currentPressed);
        SaveManager.Instance.remove_topic_for_questions(currentPressed);

        close_edit_window();
        FindObjectOfType<Scenes>().load_current_page();
    }

    public void edit_subject()
    {
        close_edit_window();
        topicTitle.GetComponent<InputField>().text = "";
        editMode = true;
        open_topic_window();
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

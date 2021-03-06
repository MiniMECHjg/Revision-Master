using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class allQuestions : MonoBehaviour
{

    public GameObject title;
    public GameObject overallContainerGO;
    public GameObject container;
    public GameObject definitionPrefab;
    public GameObject choice2Prefab;
    public GameObject choice4Prefab;
    public GameObject flashCardPrefab;
    public Animator editWindow;
    public int currentPressed;
    public List<string> questionTypes;

    public Animator importWindow;

    private string txtFileType;

    public int[] oldResolution = new int[2] {Screen.width, Screen.height};

    // Start is called before the first frame update
    void Start()
    {
        txtFileType = NativeFilePicker.ConvertExtensionToFileType( "txt" ); // Returns "application/txt" on Android and "com.adobe.txt" on iOS

        title.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.subject_title(SaveManager.Instance.current_subject()) + " Questions";

        for (int i = 0; i <= SaveManager.Instance.current_questions_for_subject(); i++)
        {
            int index = i;
            List<string> currentQuestion = SaveManager.Instance.question(i);
            if(currentQuestion[0] == "0")
            {
                GameObject def_btn = Instantiate(definitionPrefab) as GameObject;
                def_btn.transform.SetParent(container.transform, false);
                def_btn.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
                GameObject child = def_btn.transform.GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[1];
                child.GetComponent<UnityEngine.UI.Text>().SetAllDirty();
                child = def_btn.transform.GetChild(1).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[2];
            } else if (currentQuestion[0] == "2")
            {
                GameObject choice_btn = Instantiate(choice2Prefab) as GameObject;
                choice_btn.transform.SetParent(container.transform, false);
                GameObject child = choice_btn.transform.GetChild(0).GetChild(0).gameObject;
                choice_btn.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[1];
                child = choice_btn.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[2];
                child = choice_btn.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[3];
            }else if (currentQuestion[0] == "3")
            {
                GameObject choice_btn = Instantiate(choice4Prefab) as GameObject;
                choice_btn.transform.SetParent(container.transform, false);
                GameObject child = choice_btn.transform.GetChild(0).GetChild(0).gameObject;
                choice_btn.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[1];
                child = choice_btn.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[2];
                child = choice_btn.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[3];
                child = choice_btn.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[4];
                child = choice_btn.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[5];
            }else if (currentQuestion[0] == "1")
            {
                GameObject def_btn = Instantiate(flashCardPrefab) as GameObject;
                def_btn.transform.SetParent(container.transform, false);
                def_btn.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
                GameObject child = def_btn.transform.GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[1];
                child = def_btn.transform.GetChild(1).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = currentQuestion[2];
            }

            questionTypes.Add(currentQuestion[0]);
            
        }

        FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
        FindObjectOfType<ColorManager>().change_colours();
    }

    public void open_edit_window(int i)
    {
        currentPressed = i;
        editWindow.SetTrigger("open");
    }

    public void close_edit_window()
    {
        editWindow.SetTrigger("close");
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
        string text = "Subject\n";
        text += SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()];
        for(int x=0; x != SaveManager.Instance.current_questions_for_subject() + 1; x++)
        {
            text += "\n";
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

        string filePath = Path.Combine( Application.temporaryCachePath, "Subject_" + SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()] +".txt" );
        File.WriteAllText( filePath, text);

        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( filePath, ( success ) => Debug.Log( "File exported: " + success ) );

        Debug.Log( "Permission result: " + permission );
    }

    public void import_subject_questions(string[] Lines)
    {
        if(Lines[0] == "Subject")
        {
            for(int x = 2; x != Lines.Length; x++)
            {
                char[] seperators = {'¬', '`'};

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
            FindObjectOfType<Scenes>().load_current_page();
        }else
        {
            Debug.Log("This will not work");
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

            string[] Lines = File.ReadAllLines(path);
            import_subject_questions(Lines);

		}, new string[] { txtFileType } );

		Debug.Log( "Permission result: " + permission );
    }

    public void delete_question()
    {
        List<List<List<List<string>>>> baseQuestionArray = SaveManager.Instance.base_question_list();
        baseQuestionArray[SaveManager.Instance.current_subject()].RemoveAt(currentPressed);
        SaveManager.Instance.set_base_question_list(baseQuestionArray);

        close_edit_window();
        FindObjectOfType<Scenes>().load_current_page();
    }

    public void edit_question()
    {
        SaveManager.Instance.set_current_question_to_edit(currentPressed);
        SaveManager.Instance.set_build_mode(true);
        string questionType = questionTypes[currentPressed];
        if(questionType == "0")
        {
            FindObjectOfType<Scenes>().add_question_def();
        } else if (questionType == "1")
        {
            FindObjectOfType<Scenes>().add_flash();
        }else if (questionType == "2" )
        {
            FindObjectOfType<Scenes>().add_multi2();
        }else if (questionType == "3")
        {
            FindObjectOfType<Scenes>().add_multi4();
        }
        //FindObjectOfType<Scenes>().new_subject();
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
            FindObjectOfType<Scenes>().go_to_subject();
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSubjectName : MonoBehaviour
{
    public GameObject subjectInputField;
    public InputField subjectInputField_input;
    private string txt;
    public bool editMode = false;
    public Text buttonText;

    public Dropdown topicChoice;

    public void Start()
    {
        if(SaveManager.Instance.build_mode() == true)
        {
            SaveManager.Instance.set_build_mode(false);
            editMode = true;
            subjectInputField_input.text = SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()];
            buttonText.text = "Edit Subject Name";
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_home();
        }
    }

    public void submitSubjectName() 
    {
        txt = subjectInputField.GetComponent<InputField>().text;
        int value = topicChoice.value;

        if(editMode == true)
        {
            SaveManager.Instance.subjects()[SaveManager.Instance.current_subject()] = txt;
            SaveManager.Instance.Save();
        }else 
        {
            //Debug.Log(value);
            SaveManager.Instance.add_subject(txt, value);
        }
        
        FindObjectOfType<Scenes>().go_home();
    }

}

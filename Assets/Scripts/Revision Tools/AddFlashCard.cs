using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFlashCard : MonoBehaviour
{
    public GameObject titleInputField;
    public GameObject textInputField;

    public Text buttonText;

    public bool editMode = false;

    public void Start()
    {
        if(SaveManager.Instance.build_mode() == true)
        {
            SaveManager.Instance.set_build_mode(false);
            editMode = true;
            titleInputField.GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1];
            textInputField.GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2];
            buttonText.text = "Edit This Flash Card";
        }
    }

    public void submit_flash()
    {
        string text = textInputField.GetComponent<InputField>().text;
        
        string title = titleInputField.GetComponent<InputField>().text;

        if (editMode != true)
        {
            SaveManager.Instance.add_flash_card(title, text);
        }else
        {
            SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1] = title;
            SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2] = text;
            SaveManager.Instance.Save();
        }
        
        FindObjectOfType<Scenes>().go_back_to_lastPage();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_to_subject();
        }
    }
}

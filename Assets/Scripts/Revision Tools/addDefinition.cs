using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addDefinition : MonoBehaviour
{

    public GameObject wordInputField;
    public GameObject definitionInputField;

    public Text buttonText;

    public bool editMode = false;

    public void Start()
    {
        if(SaveManager.Instance.build_mode() == true)
        {
            SaveManager.Instance.set_build_mode(false);
            editMode = true;
            wordInputField.GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1];
            definitionInputField.GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2];
            buttonText.text = "Edit This Definition";
        }
    }

    public void submit_def()
    {
        string word = wordInputField.GetComponent<InputField>().text;
        
        string definition = definitionInputField.GetComponent<InputField>().text;

        if(editMode == false)
        {
            SaveManager.Instance.add_definition(word, definition);
        } else
        {
            SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1] = word;
            SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2] = definition;
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

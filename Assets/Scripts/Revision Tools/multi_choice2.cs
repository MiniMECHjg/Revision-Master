using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class multi_choice2 : MonoBehaviour
{

    public List<GameObject> inputFields;
    public Text buttonText;

    public bool editMode = false; 

    public void Start()
    {
        if(SaveManager.Instance.build_mode() == true)
        {
            SaveManager.Instance.set_build_mode(false);
            editMode = true;
            inputFields[0].GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1];
            inputFields[1].GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2];
            inputFields[2].GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][3];
            buttonText.text = "Edit This Multi";
            if (inputFields.Count == 5)
            {
                inputFields[3].GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][4];
                inputFields[4].GetComponent<InputField>().text = SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][5];
            }
            
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_to_subject();
        }
    }

    public void submit()
    {
        //Choice 1 will be the correct one
        if (inputFields.Count == 3)
        {
            string question = inputFields[0].GetComponent<InputField>().text;
            string choice1 = inputFields[1].GetComponent<InputField>().text;
            string choice2 = inputFields[2].GetComponent<InputField>().text;
            if(editMode == false)
            {
                SaveManager.Instance.add_choice2(question, choice1, choice2);
            }else
            {
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1] = question;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2] = choice1;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][3] = choice2;
                SaveManager.Instance.Save();
            }
            
        } else if (inputFields.Count == 5)
        {
            string question = inputFields[0].GetComponent<InputField>().text;
            string choice1 = inputFields[1].GetComponent<InputField>().text;
            string choice2 = inputFields[2].GetComponent<InputField>().text;
            string choice3 = inputFields[3].GetComponent<InputField>().text;
            string choice4 = inputFields[4].GetComponent<InputField>().text;
            
            if(editMode == false)
            {
                SaveManager.Instance.add_choice5(question, choice1, choice2, choice3, choice4);;
            }else
            {
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][1] = question;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][2] = choice1;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][3] = choice2;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][4] = choice3;
                SaveManager.Instance.base_question_list()[SaveManager.Instance.current_subject()][SaveManager.Instance.current_topic()][SaveManager.Instance.current_question_to_edit()][5] = choice4;
                SaveManager.Instance.Save();
            }
        }
        FindObjectOfType<Scenes>().go_back_to_lastPage();
    }

    
}

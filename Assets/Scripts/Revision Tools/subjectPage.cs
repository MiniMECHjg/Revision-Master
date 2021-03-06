using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subjectPage : MonoBehaviour
{

    public GameObject title;
    public List<int> all_questions;
    public List<int> set_questions;

    public Animator playPanel;

    public List<Toggle> questionToggles;
    public List<Toggle> numToDoToggles;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance.topic_value() == 0)
        {
            title.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.subject_title(SaveManager.Instance.current_subject());
        }
        else 
        {
            title.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.subject_title(SaveManager.Instance.current_subject()) + " - " + SaveManager.Instance.topic_titles()[SaveManager.Instance.current_topic()];
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().back_from_subject();
        }
    }

    public void play()
    {
        for(int i = 0; i <= SaveManager.Instance.current_questions_for_subject(); i++)
        {
            List<string> question = SaveManager.Instance.question(i);
            if((question[0] == "0" && questionToggles[0].isOn == true) || (question[0] == "1" && questionToggles[1].isOn == true) || ((question[0] == "2" || question[0] == "3") && questionToggles[2].isOn == true))
            {
                all_questions.Add(i);
                // 0, 1, 2, 3 ...
            }
        }

        bool play = true;

        if (numToDoToggles[0].isOn)
        {
            SaveManager.Instance.set_play_value(0);
        } else if(numToDoToggles[1].isOn)
        {
            SaveManager.Instance.set_play_value(1);
        } else if(numToDoToggles[2].isOn)
        {
            SaveManager.Instance.set_play_value(2);
        }else if(numToDoToggles[3].isOn)
        {
            SaveManager.Instance.set_play_value(3);
        }else
        {
            play = false;
        }

        if (play == true)
        {

            int numToDo = 0;
            if(SaveManager.Instance.play_value() != 3)
            {
                numToDo = (SaveManager.Instance.play_value()+1)*5;
            }else
            {
                numToDo = all_questions.Count;
            }
            //Debug.Log(numToDo);
            //Debug.Log(all_questions.Count);
            if (all_questions.Count != 0)
            {
                //This should put in the number from all_questions at random indexs and remove it after.

                if(all_questions.Count <= numToDo)
                {
                    int total_questions = all_questions.Count;
                    for(int index = 0; index < total_questions; index++)
                    {
                        int choice = UnityEngine.Random.Range(0, all_questions.Count);
                        set_questions.Add(all_questions[choice]);
                        all_questions.RemoveAt(choice);
                    }
                }else 
                {
                    for(int x = 0; x < numToDo; x++)
                    {
                        int choice = UnityEngine.Random.Range(0, all_questions.Count);
                        set_questions.Add(all_questions[choice]);
                        all_questions.RemoveAt(choice);
                    }
                }

                //Debug.Log(set_questions.Count);
                SaveManager.Instance.set_questions_to_do(set_questions);
                SaveManager.Instance.set_question_index(0);
                SaveManager.Instance.set_total_current_questions_index(set_questions.Count);
                FindObjectOfType<Scenes>().load_first_question();

                /*
                foreach(int i in set_questions)
                {
                    Debug.Log(set_questions[i]);
                }
                */
            }
        }
    }

    public void open_play_panel()
    {
        playPanel.SetTrigger("open");
    }

    public void close_play_panel()
    {
        playPanel.SetTrigger("close");
    }

}

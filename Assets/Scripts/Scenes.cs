using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 0.5f;

    public void load_current_page()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void settings() 
    {
        SaveManager.Instance.set_last_page(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(LoadLevel(3));
    }

    public void back_from_subject()
    {
        if(SaveManager.Instance.topic_value() == 1)
        {
            go_to_topics();
        } else
        {
            go_home();
        }
    }

    public void go_back_to_lastPage()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.last_page()));
    }

    public void go_main_home() 
    {
        StartCoroutine(LoadLevel(0));
    }

    public void go_home() 
    {
        StartCoroutine(LoadLevel(1));
        SaveManager.Instance.increment_button_count();
    }

    public void new_subject()
    {
        StartCoroutine(LoadLevel(2));
    }

    public void go_to_subject()
    {
        StartCoroutine(LoadLevel(4));
        SaveManager.Instance.increment_button_count();
    }

    public void add_question_choice()
    {
        SaveManager.Instance.set_last_page(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(LoadLevel(5));
    }

    public void add_question_def()
    {
        StartCoroutine(LoadLevel(6));
    }

    public void see_all_questions()
    {
        StartCoroutine(LoadLevel(7));
    }

    //loads first question but also the one you are on
    public void load_first_question() 
    {
        if(SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "0")
        {
            StartCoroutine(LoadLevel(8));
        }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "2")
        {
            StartCoroutine(LoadLevel(14));
        }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "3")
        {
            StartCoroutine(LoadLevel(15));
        }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "1")
        {
            StartCoroutine(LoadLevel(17));
        }
    }

    public void next_question()
    {
        SaveManager.Instance.set_question_index(SaveManager.Instance.current_question_index() + 1);
        if(SaveManager.Instance.current_question_index() != SaveManager.Instance.total_current_questions_index())
        {
            if(SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "0")
            {
                StartCoroutine(LoadLevel(8));
            }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "2")
            {
                StartCoroutine(LoadLevel(14));
            }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "3")
            {
                StartCoroutine(LoadLevel(15));
            }else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "1")
            {
                StartCoroutine(LoadLevel(17));
            }
        }else
        {
            go_to_subject();
        }
    }

    public void add_flash()
    {
        StartCoroutine(LoadLevel(16));
    }

    public void correct()
    {
        SaveManager.Instance.add_correct();
        FindObjectOfType<audio_manager>().correct();
        StartCoroutine(LoadLevel(9));
    }

    public void wrong()
    {
        SaveManager.Instance.add_wrong();
        FindObjectOfType<audio_manager>().wrong();
        StartCoroutine(LoadLevel(10));
    }

    public void flash_happy()
    {
        SaveManager.Instance.add_correct();
        FindObjectOfType<audio_manager>().correct();
        next_question();
    }

    public void flash_unhappy()
    {
        SaveManager.Instance.add_wrong();
        FindObjectOfType<audio_manager>().wrong();
        next_question();
    }

    public void pick_multi_choice()
    {
       StartCoroutine(LoadLevel(11)); 
    }

    public void add_multi2()
    {
        StartCoroutine(LoadLevel(12));
    }

    public void add_multi4()
    {
        StartCoroutine(LoadLevel(13));
    }



    /*
    The following are the movement between Helpful tools
    */

    public void to_do_home()
    {
        StartCoroutine(LoadLevel(18));
        SaveManager.Instance.increment_button_count();
    }

    public void load_to_do_list()
    {
        StartCoroutine(LoadLevel(19));
    }

    public void statistics_home()
    {
        StartCoroutine(LoadLevel(20));
        SaveManager.Instance.increment_button_count();
    }

    public void go_to_topics()
    {
        StartCoroutine(LoadLevel(21));
    }


    //This is how we go to each scene to add an animation
    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
    }

}

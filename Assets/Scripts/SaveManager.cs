using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveScript state;

    public static SaveManager instance;

    public int button_count = 0;

    string gameId = "3956205";
    bool testMode = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //ResetSave();
        Instance = this;
        Load();

        set_build_mode(false);

        Advertisement.Initialize (gameId, testMode);

        DateTime todaysDate = DateTime.Now.Date;
        int[] todaysDateArray = new int[3];
        todaysDateArray[0] = todaysDate.Day;
        todaysDateArray[1] = todaysDate.Month;
        todaysDateArray[2] = todaysDate.Year;

        if(todaysDateArray[0] == last_date()[0] && todaysDateArray[1] == last_date()[1] && todaysDateArray[2] == last_date()[2])
        {
            //Do nothing
        } else
        {
            set_new_date(todaysDateArray);
            reset_per_day_stats();
        }
    }

    public void set_play_value(int i)
    {
        state.playValue = i;
        Save();
    }

    public int play_value()
    {
        return state.playValue;
    }

    public void increment_button_count()
    {
        button_count += 1;
        if (button_count >= 10)
        {
            if(Advertisement.IsReady("video"))
            {
                Advertisement.Show("video");
            }
            button_count = 0;
        }
    }

    public int[] all_correct()
    {
        return state.allCorrect;
    }

    public int[] all_wrong()
    {
        return state.allWrong;
    }

    public void reset_per_day_stats()
    {
        for(int x = 0; x!=state.correct.Length; x++)
        {
            state.correct[x] = 0;
            state.wrong[x] = 0;
        }

        Save();
    }

    public void set_new_date(int[] newArray)
    {
        state.lastDate = newArray;
        Save();
    }

    public int[] last_date()
    {
        return state.lastDate;
    }

    public int[] correct_array()
    {
        return state.correct;
    }

    public int[] wrong_array()
    {
        return state.wrong;
    }

    public void add_correct()
    {
        string questionType = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0];
        int questionTypeIndex = Int32.Parse(questionType);
        state.correct[0] += 1;
        state.allCorrect[0] += 1;
        state.correct[questionTypeIndex + 1] += 1;
        state.allCorrect[questionTypeIndex + 1] += 1;
        Save();
    }

    public void add_wrong()
    {
        string questionType = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0];
        int questionTypeIndex = Int32.Parse(questionType);
        state.wrong[0] += 1;
        state.allWrong[0] += 1;
        state.wrong[questionTypeIndex + 1] += 1;
        state.allWrong[questionTypeIndex + 1] += 1;
        Save();
    }

    public List<int> recent_list()
    {
        return state.recentLists;
    }

    public void set_recent_lists(List<int> newList)
    {
        state.recentLists = newList;
        Save();
    }
    
    public int current_question_to_edit()
    {
        return state.currentQuestionToEdit;
    }

    public void set_current_question_to_edit(int i)
    {
        state.currentQuestionToEdit = i;
        Save();
    }

    public void add_new_title(string title)
    {
        state.titlesToDo.Add(title);
        Save();
    }

    public List<string> title_list()
    {
        return state.titlesToDo;
    }

    public string title_to_do_index(int index)
    {
        return state.titlesToDo[index];
    }

    public void remove_to_do_list(int index)
    {
        state.toDos.RemoveAt(index);
        state.titlesToDo.RemoveAt(index);
        Save();
    }

    public void add_task(string text, string priority)
    {
        state.toDos[current_to_do_index()].Add(new List<string>());
        int index = (state.toDos[current_to_do_index()].Count) - 1;
        state.toDos[current_to_do_index()][index].Add(text);
        state.toDos[current_to_do_index()][index].Add(priority);
        Save();
    }

    public int current_to_do_index()
    {
        return state.currentToDoIndex;
    }

    public void set_current_to_do_index(int i)
    {
        state.currentToDoIndex = i;
        Save();
    }

    public void add_to_do_list(string title)
    {
        state.toDos.Add(new List<List<string>>());
        Save();
        add_new_title(title);
    }

    public List<List<List<string>>> all_to_do_list()
    {
        return state.toDos;
    }

    public void set_build_mode(bool i)
    {
        state.buildMode = i;
    }

    public bool build_mode()
    {
        return state.buildMode;
    }

    public int last_page()
    {
        return state.last_page;
    }

    public void set_last_page(int index)
    {
        state.last_page = index;
        Save();
    }

    public int current_question_index() 
    {
        return state.currentQuestionIndex;
    }

    public void set_question_index(int i)
    {
        state.currentQuestionIndex = i;
        Save();
    }

    public int total_current_questions_index() 
    {
        return state.totalCurrentQuestionsIndex;
    }

    public void set_total_current_questions_index(int i) 
    {
        state.totalCurrentQuestionsIndex = i;
        Save();
    }

    public List<int> questions_to_do()
    {
        return state.questionsToDo;
    }

    public void set_questions_to_do(List<int> questions)
    {
        state.questionsToDo = questions;
        Save();
    }

    public List<string> question(int i)
    {
        return state.questions[current_subject()][current_topic()][i];
    }

    public List<List<List<List<string>>>> base_question_list()
    {
        return state.questions;
    }

    public void set_base_question_list(List<List<List<List<string>>>> newArray)
    {
        state.questions = newArray;
    }

    public void add_definition(string word, string def) 
    {
        state.questions[current_subject()][current_topic()].Add(new List<string>());
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add("0");
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(word);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(def);
        Save();
    }

    public void add_flash_card(string title, string word)
    {
        //Debug.Log(current_questions_for_subject());
        state.questions[current_subject()][current_topic()].Add(new List<string>());
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add("1");
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(title);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(word);
        Save();
    }

    public void add_choice2(string question, string choice1, string choice2)
    {
        state.questions[current_subject()][current_topic()].Add(new List<string>());
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add("2");
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(question);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice1);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice2);
        Save();
    }

    public void add_choice5(string question, string choice1, string choice2, string choice3, string choice4)
    {
        state.questions[current_subject()][current_topic()].Add(new List<string>());
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add("3");
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(question);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice1);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice2);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice3);
        state.questions[current_subject()][current_topic()][current_questions_for_subject()].Add(choice4);
        Save();
    }

    public int current_questions_for_subject() 
    {
        //Debug.Log(current_subject());
        //Debug.Log(current_topic());
        //Debug.Log(state.questions[current_subject()]);
        //Debug.Log(state.questions[current_subject()][current_topic()]);
        return (state.questions[current_subject()][current_topic()].Count) - 1;
    }

    public void set_current_subject(int i)
    {
        state.currentSubject = i;
        if(state.topics[state.currentSubject] == 0);
        {
            state.currentTopic = 0;
            FindObjectOfType<Scenes>().go_to_subject();
        }
        if (state.topics[state.currentSubject] == 1)
        {
            FindObjectOfType<Scenes>().go_to_topics();
        }
        Save();
    }

    public int current_subject()
    {
        return state.currentSubject;
    }

    public void update_subjects(List<string> subjectsList)
    {
        state.subjects = subjectsList;
        Save();
    }

    public int current_topic()
    {
        return state.currentTopic;
    }


    public void set_current_topic(int value)
    {
        state.currentTopic = value;
    }

    public List<string> subjects() 
    {
        return state.subjects;
    }

    public List<string> topic_titles()
    {
        return state.topicTitles[current_subject()];
    }

    public int topic_value()
    {
        return state.topics[state.currentSubject];
    }

    public void delete_topic_value(int i)
    {
        state.topics.RemoveAt(i);
        Save();
    }

    public void remove_topic_title(int i)
    {
        state.topicTitles[state.currentSubject].RemoveAt(i);
        Save();
    }

    public void edit_topic_name(string name, int i)
    {
        state.topicTitles[state.currentSubject][i] = name;
        Save();
    }

    public void remove_topic_for_questions(int i)
    {
        state.questions[state.currentSubject].RemoveAt(i);
        Save();
    }

    public void delete_all_topics(int i)
    {
        state.topicTitles.RemoveAt(i);
        Save();
    }   

    public string subject_title(int i) 
    {
        return state.subjects[i];
    }

    public void add_subject(string name, int value) 
    {
        state.subjects.Add(name);
        state.topicTitles.Add(new List<string>());
        state.questions.Add(new List<List<List<string>>>());
        state.questions[state.questions.Count-1].Add(new List<List<string>>());
        if(value == 0)
        {
            state.topics.Add(0);
        } else
        {
            state.topics.Add(1);
        }
        Save();
    }

    public int num_of_topics()
    {
        int numOfTopics = state.questions[state.currentSubject].Count;
        return numOfTopics;
    }

    public void add_topic(string name)
    {
        state.topicTitles[state.currentSubject].Add(name);
        state.questions[state.currentSubject].Add(new List<List<string>>());
        Save();
    }

    public void change_primary(Color32 i) 
    {
        state.colorp = i;
        Save();
    }

    public Color32 primary_colour()
    {
        return state.colorp;
    }

    public void change_accent(Color32 i)
    {
        state.colora = i;
        Save();
    }

    public Color32 accent_colour()
    {
        return state.colora;
    }

    public void change_text(Color32 i)
    {
        state.colort = i;
        Save();
    }

    public Color32 text_colour()
    {
        return state.colort;
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveScript>(state));
        Load();
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveScript>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveScript();
            Save();
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}

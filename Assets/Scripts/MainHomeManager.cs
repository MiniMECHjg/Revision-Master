using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainHomeManager : MonoBehaviour
{

    public GameObject LHSToggle;
    public GameObject RHSToggle;
    public Animator LHSMenu;
    public Animator RHSMenu;
    public bool LHSout = false;
    public bool RHSout = false;

    public GameObject overallContainerGO;

    public GameObject subjectsContainer;
    public GameObject subjectPrefab;
    public GameObject subjectText;

    public GameObject listContainer;
    public GameObject listPrefab;
    public GameObject listText;

    public Image correctImage;
    public Text correctText;

    public Image allTimeCorrectImage;
    public Text allTimeCorrectText;

    public int[] oldResolution = new int[2] {Screen.width, Screen.height};

    // Start is called before the first frame update
    void Start()
    {
        List<string> subjects = SaveManager.Instance.subjects();
        double rows = (subjects.Count)/3;
        int rowCount = (int)Math.Ceiling(rows);

        if(rowCount > 3)
        {
            rowCount -= 2;
            RectTransform rectTransform = subjectsContainer.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (-120f * rowCount));

        } else if (subjects.Count == 0)
        {
            subjectText.SetActive(true);
        }

        for(int i = 0; i != subjects.Count; i++)
        {
            GameObject subject_btn = Instantiate(subjectPrefab) as GameObject;
            subject_btn.transform.SetParent(subjectsContainer.transform, false);
            int index = i;
            GameObject child = subject_btn.transform.GetChild(0).gameObject;
            child.GetComponent<UnityEngine.UI.Text>().text = subjects[index];
            subject_btn.GetComponent<Button>().onClick.AddListener(() => press_subject(index));
        }

        List<int> recentUsedLists = SaveManager.Instance.recent_list();
        if (recentUsedLists.Count != 0)
        {
            listText.SetActive(false);
            for(int i = 0; i != recentUsedLists.Count; i++)
            {
                GameObject subject_btn = Instantiate(listPrefab) as GameObject;
                subject_btn.transform.SetParent(listContainer.transform, false);
                int index = i;
                GameObject child = subject_btn.transform.GetChild(0).GetChild(0).gameObject;
                child.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.title_list()[recentUsedLists[index]];
                subject_btn.GetComponent<Button>().onClick.AddListener(() => press_list(recentUsedLists[index]));
            }
        }

        set_pie_charts();

        FindObjectOfType<scaler>().reset_container(overallContainerGO, subjectsContainer);
    }

    public void set_pie_charts()
    {
        int[] correct = SaveManager.Instance.correct_array();
        int[] wrong = SaveManager.Instance.wrong_array();
        float percentageCorrect;
        int percentage; 

        if(correct[0] + wrong[0] == 0)
        {
            correctImage.fillAmount = 1;
            percentageCorrect = 1;
            correctImage.color = new Color32(60, 25, 25, 255);
            correctText.text = "No work has been done"; 
        } else
        {
            percentageCorrect = (float)correct[0]/(float)(correct[0]+wrong[0]);
            correctImage.fillAmount = percentageCorrect;

            percentage = Convert.ToInt32(percentageCorrect*100);
            correctText.text = "Correct: " + percentage.ToString() +"%";
        }
  
        int[] allTimeCorrect = SaveManager.Instance.all_correct();
        int[] allTimeWrong = SaveManager.Instance.all_wrong();
        if(allTimeCorrect[0] + allTimeWrong[0] == 0)
        {
            allTimeCorrectImage.fillAmount = 1;
            percentageCorrect = 1;
            allTimeCorrectImage.color = new Color32(60, 25, 25, 255);
            allTimeCorrectText.text = "No work has been done";
        }else
        {
            percentageCorrect = (float)allTimeCorrect[0]/(float)(allTimeCorrect[0]+allTimeWrong[0]);
            allTimeCorrectImage.fillAmount = percentageCorrect;

            percentage = Convert.ToInt32(percentageCorrect*100);
            allTimeCorrectText.text = "Correct: " + percentage.ToString() + "%";
        }

    }

    public void toggleLHS()
    {
        if(LHSout == false)
        {
            LHSMenu.SetTrigger("Out");
            LHSout = true;
        } else 
        {
            LHSMenu.SetTrigger("In");
            LHSout = false;
        }
    }

    public void toggleRHS()
    {
        if(RHSout == false)
        {
            RHSMenu.SetTrigger("Out");
            RHSout = true;
        } else 
        {
            RHSMenu.SetTrigger("In");
            RHSout = false;
        }
    }

    public void press_subject(int index)
    {
        //Debug.Log(index);
        SaveManager.Instance.set_current_subject(index);
        if(SaveManager.Instance.topic_value() == 0)
        {
            FindObjectOfType<Scenes>().go_to_subject();
        }else
        {
            FindObjectOfType<Scenes>().go_to_topics();
        }
        
    }

    public void press_list(int index)
    {
        //Debug.Log(index);
        SaveManager.Instance.set_current_to_do_index(index);
        List<int> recentList = SaveManager.Instance.recent_list();
        //Open List section
        if(recentList[0] != index) 
        { 
            recentList.Insert(0, index);
            if(recentList.Count > 2)
            {
                recentList.RemoveAt(2);
            }
        }
 
        SaveManager.Instance.set_recent_lists(recentList);

        FindObjectOfType<Scenes>().load_to_do_list();
    }

    void Update()
    {
        int x = Screen.width;
        int y = Screen.height;
        if(x != oldResolution[0] || y != oldResolution[1])
        {
            FindObjectOfType<scaler>().reset_container(overallContainerGO, subjectsContainer);
            oldResolution[0] = x;
            oldResolution[1] = y;
        }
    }
}

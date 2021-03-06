using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class statsHome : MonoBehaviour
{

    public GameObject overallContainerGO;
    public GameObject containerOfStats;
    public int[] oldResolution = new int[2] {Screen.width, Screen.height};


    //For pie charts that are about all that has been done correct
    public Image correctImage;
    public Text correctText;

    public Image allTimeCorrectImage;
    public Text allTimeCorrectText;

    public Image correctDef;
    public Text defCorrectText;

    public Image correctMulti;
    public Text multiCorrectText;

    public Image correctFlash;
    public Text flashCorrectText;

    public Text analysis;

    // Start is called before the first frame update
    void Start()
    {
        set_correct_pie_charts();
        set_questions_done_pie_charts();
        FindObjectOfType<scaler>().reset_container(overallContainerGO, containerOfStats);
    }

    public void set_correct_pie_charts()
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

    public void set_questions_done_pie_charts()
    {
        int[] correct = SaveManager.Instance.all_correct();
        int[] wrong = SaveManager.Instance.all_wrong();
        float percentageCorrect;
        int percentage;

        if(correct[1] + wrong[1] == 0)
        {
            correctDef.fillAmount = 1;
            percentageCorrect = 1;
            correctDef.color = new Color32(60, 25, 25, 255);
            defCorrectText.text = "No defs have been done"; 
        } else
        {
            percentageCorrect = (float)correct[1]/(float)(correct[1]+wrong[1]);
            correctDef.fillAmount = percentageCorrect;

            percentage = Convert.ToInt32(percentageCorrect*100);
            defCorrectText.text = "Defs Correct: " + percentage.ToString() +"%";
        }

        if(correct[2] + wrong[2] + correct[3] + wrong[3] == 0)
        {
            correctMulti.fillAmount = 1;
            percentageCorrect = 1;
            correctMulti.color = new Color32(60, 25, 25, 255);
            multiCorrectText.text = "No multis have been done"; 
        } else
        {
            percentageCorrect = (float)(correct[2]+correct[3])/(float)((correct[2] + correct[3] + wrong[2] + wrong[3]));
            correctMulti.fillAmount = percentageCorrect;

            percentage = Convert.ToInt32(percentageCorrect*100);
            multiCorrectText.text = "Multis Correct: " + percentage.ToString() +"%";
        }

        if(correct[4] + wrong[4] == 0)
        {
            correctFlash.fillAmount = 1;
            percentageCorrect = 1;
            correctFlash.color = new Color32(60, 25, 25, 255);
            flashCorrectText.text = "No flashes have been done"; 
        } else
        {
            percentageCorrect = (float)correct[4]/(float)(correct[4]+wrong[4]);
            correctFlash.fillAmount = percentageCorrect;

            percentage = Convert.ToInt32(percentageCorrect*100);
            flashCorrectText.text = "Flashes Correct: " + percentage.ToString() +"%";
        }

    }


    // Update is called once per frame
    void Update()
    {
        int x = Screen.width;
        int y = Screen.height;
        if(x != oldResolution[0] || y != oldResolution[1])
        {
            FindObjectOfType<scaler>().reset_container(overallContainerGO, containerOfStats);
            oldResolution[0] = x;
            oldResolution[1] = y;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_home();
        }
    }
}

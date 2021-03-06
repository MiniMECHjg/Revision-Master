using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submit_multi : MonoBehaviour
{

    public List<GameObject> texts;
    public List<GameObject> toggles;

    public int randomNum;

    // Start is called before the first frame update
    void Start()
    {
        texts[0].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[1];
        if(toggles.Count == 2)
        {
            System.Random rnd = new System.Random();
            randomNum = rnd.Next(1,3);
            //no change
            if (randomNum == 1) {
                texts[1].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];
                texts[2].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[3];
            } else 
            {
                texts[1].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[3];
                texts[2].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];
            }
        }else
        {
            System.Random rnd = new System.Random();
            randomNum = rnd.Next(1,5);
            //Debug.Log(randomNum);
            List<int> places = new List<int>();
            for (int x = 1; x != 5; x++)
            {
                places.Add(x);
                //Debug.Log(x);
            }
            places.Remove(randomNum);

            texts[randomNum].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];

            int newRandom = 0;
            for(int x = 3; x!=6; x++)
            {
                rnd = new System.Random();
                newRandom = rnd.Next(0, places.Count);
                texts[places[newRandom]].GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[x];
                places.RemoveAt(newRandom);
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
        if(toggles.Count == 2)
        {
            if(randomNum == 1)
            {
                if(toggles[0].GetComponent<Toggle>().isOn)
                {
                    FindObjectOfType<Scenes>().correct();
                } else{
                    FindObjectOfType<Scenes>().wrong();
                }
            }else
            {
                if(toggles[1].GetComponent<Toggle>().isOn)
                {
                    FindObjectOfType<Scenes>().correct();
                } else{
                    FindObjectOfType<Scenes>().wrong();
                }
            }
        }else
        {
            if(toggles[randomNum-1].GetComponent<Toggle>().isOn)
            {
                FindObjectOfType<Scenes>().correct();
            }
            else{
                FindObjectOfType<Scenes>().wrong();
            }
        }
    }
}

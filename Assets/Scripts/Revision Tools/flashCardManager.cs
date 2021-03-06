using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashCardManager : MonoBehaviour
{
    public int turnt = 1;
    public Animator flashCard;
    public GameObject title;
    public GameObject text;

    public void Start()
    {
        title.GetComponent<Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[1];
        text.GetComponent<Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_to_subject();
        }
    }

    public void rotate_card()
    {
        if (turnt % 2 == 0) 
        {
            flashCard.SetTrigger("ToTitle");
        } else
        {
            flashCard.SetTrigger("ToText");
        }
        turnt++;
    }
}

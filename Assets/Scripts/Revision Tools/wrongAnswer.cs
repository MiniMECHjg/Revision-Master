using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrongAnswer : MonoBehaviour
{

    public GameObject word;
    public GameObject definition;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "0")
        {
            word.GetComponent<UnityEngine.UI.Text>().text = "Wrong! The correct definition of " + SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[1] + " is:";
        }
        else if (SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "2" || SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[0] == "3")
        {
            word.GetComponent<UnityEngine.UI.Text>().text = "Wrong! The correct answer to " + SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[1] + " is:";
        }
        definition.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];
    }
}

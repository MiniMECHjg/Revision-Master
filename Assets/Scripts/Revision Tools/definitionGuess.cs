using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class definitionGuess : MonoBehaviour
{

    public GameObject word; //Title
    public GameObject definitionInputField; //InputField

    // Start is called before the first frame update
    void Start()
    {
        word.GetComponent<UnityEngine.UI.Text>().text = "What is the definition of: " + SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[1];
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
        GameObject txtGameObject = definitionInputField.transform.GetChild(2).gameObject;
        string definitionGuessed = txtGameObject.GetComponent<UnityEngine.UI.Text>().text;
        definitionGuessed = definitionGuessed.ToLower();
        definitionGuessed = definitionGuessed.Replace(" ", "");
        definitionGuessed = definitionGuessed.Replace("\n", "").Replace("\r", "");

        string actualDefinition = SaveManager.Instance.question(SaveManager.Instance.questions_to_do()[SaveManager.Instance.current_question_index()])[2];
        actualDefinition = actualDefinition.ToLower();
        actualDefinition = actualDefinition.Replace(" ","");
        actualDefinition = actualDefinition.Replace("\n", "").Replace("\r", "");

        if (actualDefinition == definitionGuessed)
        {
            FindObjectOfType<Scenes>().correct();
        } else
        {
            FindObjectOfType<Scenes>().wrong();
        }
    }
}

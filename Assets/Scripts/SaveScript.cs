using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript
{
    public List<int> topics = new List<int>();
    //Hold 1 and 0. If 1 they have topics If 0 that one does not have topics and will default currentTopic to 0
    public List<List<string>> topicTitles = new List<List<string>>();
    // Hold the name of the topics for each subject. A subject that does not want topics will just not have any names as it will be skipped anyway.
    public List<string> subjects = new List<string>();
    public List<List<List<List<string>>>> questions = new List<List<List<List<string>>>>();
    //[Subject][Sub-topic][question][question information]
    // in the form [int type, str text, str answer]
    // if the type is multichoice then have answers it cannot be as well as an ibnt holding the values in total

    public int currentSubject = 0;
    public int currentTopic = 0;
    //Default to 0 if there is no topics in the question
    public int currentQuestionIndex = 0;
    public int totalCurrentQuestionsIndex = 0;
    public List<int> questionsToDo;

    public int last_page = 0;

    public Color32 colorp = new Color32(255,255,255,255);
    public Color32 colora = new Color32(220,220,220, 255);
    public Color32 colort = new Color32(0, 0, 0, 255);

    public bool buildMode = false;
    public int currentQuestionToEdit = 0;

    public int currentToDoIndex = 0;
    public List<string> titlesToDo = new List<string>();
    public List<List<List<string>>> toDos = new List<List<List<string>>>();
    //[[[text, priority], [text, priority]]]

    public List<int> recentLists;

    public int[] lastDate = new int[3] {0, 0, 0};
    //[day, month, year] - This is checked to see if any have changed if so everything is reset
    public int[] correct = new int[5] {0, 0, 0, 0, 0};
    public int[] wrong = new int[5] {0, 0, 0, 0, 0};
    //[number of right/wrong, count fot 0, count for 1, count for 2, count for 3]
    //0, 1, 2, 3 represent the question type

    public int[] allCorrect = new int[5] {0, 0, 0, 0, 0};
    public int[] allWrong = new int[5] {0, 0, 0, 0, 0};

    public int playValue = 0;
}
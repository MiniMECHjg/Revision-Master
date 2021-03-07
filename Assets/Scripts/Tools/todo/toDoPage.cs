using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toDoPage : MonoBehaviour
{

    public GameObject toDoPrefab;
    public GameObject overallContainerGO;
    public GameObject container;

    public Sprite highPriorityImage;
    public Sprite mediumPriorityImage;
    public Sprite noPriorityImage;

    public GameObject task;
    public GameObject taskPriority;
    public GameObject taskField;

    public Animator addTaskWindow;

    public List<GameObject> taskList;
    public List<Toggle> allToggles;
    public List<int> toDestroy;

    public int[] oldResolution = new int[2] {Screen.width, Screen.height};

    // Start is called before the first frame update
    void Start()
    {

        List<List<List<string>>> toDos = SaveManager.Instance.all_to_do_list();
        int numOfToDos = toDos[SaveManager.Instance.current_to_do_index()].Count;

        if (numOfToDos > 7)
        {
            int numOverContainer = numOfToDos - 7;
            RectTransform rectTransform = container.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (-175f * numOverContainer));
        }

        //shows all the taks to do
        for(int x = 0; x != numOfToDos; x++)
        {
            GameObject toDo = Instantiate(toDoPrefab) as GameObject;
            toDo.transform.SetParent(container.transform, false);
            GameObject child = toDo.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
            int index = x;
            child.GetComponent<UnityEngine.UI.Text>().text = toDos[SaveManager.Instance.current_to_do_index()][index][0];
            child = toDo.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            if(toDos[SaveManager.Instance.current_to_do_index()][index][1] == "0")
            {
                child.GetComponent<Image>().sprite = noPriorityImage;
            }else if(toDos[SaveManager.Instance.current_to_do_index()][index][1] == "1")
            {
                child.GetComponent<Image>().sprite = mediumPriorityImage;
            }else if(toDos[SaveManager.Instance.current_to_do_index()][index][1] == "2")
            {
                child.GetComponent<Image>().sprite = highPriorityImage;
            }

            child = toDo.transform.GetChild(0).GetChild(0).gameObject;
            Toggle toggle = child.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate {press_toggle(index);});
            allToggles.Add(toggle);
            taskList.Add(toDo);
        }

        FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            go_back();
        }
        int x = Screen.width;
        int y = Screen.height;
        if(x != oldResolution[0] || y != oldResolution[1])
        {
            FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
            oldResolution[0] = x;
            oldResolution[1] = y;
        }
    }

    public void press_toggle(int index)
    {
        if (allToggles[index].isOn == true)
        {
            toDestroy.Add(index);
            (taskList[index].transform.GetChild(1).gameObject).GetComponent<Animator>().SetTrigger("finished");
        }else
        {
            toDestroy.Remove(index);
            (taskList[index].transform.GetChild(1).gameObject).GetComponent<Animator>().SetTrigger("notFinished");
        }
        
    }

    public void sort_list()
    {
        toDestroy.Sort();
        toDestroy.Reverse();
    }

    public void delete_all_completed()
    {
        sort_list();
        foreach (int index in toDestroy)
        {
            List<List<List<string>>> toDos = SaveManager.Instance.all_to_do_list();
            toDos[SaveManager.Instance.current_to_do_index()].RemoveAt(index);
        }
    }

    //all movement to different scenes are done differently as they
    //need to save any changes to any to-dos which are to be gotten rid of
    public void go_to_settings()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().settings();
    }

    public void go_back()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().to_do_home();
    }

    public void go_home()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().go_main_home();
    }

    public void go_revision_tools()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().go_home();
    }

    public void statistics()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().go_main_home();
    }

    public void open_task_window()
    {
        task.GetComponent<InputField>().text = "";

        taskPriority.GetComponent<Dropdown>().value = 0;

        addTaskWindow.SetTrigger("open");
    }

    public void close_task_window()
    {
        addTaskWindow.SetTrigger("close");
    }

    public void reload_page()
    {
        delete_all_completed();
        FindObjectOfType<Scenes>().load_current_page();
    }

    public void add_to_do()
    {
        string taskText = taskField.GetComponent<InputField>().text;

        string priority = (taskPriority.GetComponent<Dropdown>().value).ToString();

        SaveManager.Instance.add_task(taskText, priority);
        close_task_window();
        reload_page();
    }


}

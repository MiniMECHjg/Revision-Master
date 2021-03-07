using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class todoHomeManager : MonoBehaviour
{

    public GameObject overallContainerGO;
    public GameObject container;
    public GameObject toDoPrefab;
    public Animator editWindow;
    public Animator addListWindow;
    public GameObject addListWindowInputField;
    public int currentPressed = 0;

    public int[] oldResolution = new int[2] {Screen.width, Screen.height};

   void Start()
   {
       //sets the different buttons that will be needed to go to each list
       for(int x = 0; x < SaveManager.Instance.title_list().Count; x++)
       {
            GameObject toDoBtn = Instantiate(toDoPrefab) as GameObject;
            toDoBtn.transform.SetParent(container.transform, false);
            GameObject child = toDoBtn.transform.GetChild(0).GetChild(0).gameObject;
            int index = x;
            child.GetComponent<UnityEngine.UI.Text>().text = SaveManager.Instance.title_to_do_index(index);
            toDoBtn.GetComponent<Button>().onClick.AddListener(() => press_list(index));
            child = toDoBtn.transform.GetChild(1).gameObject;
            child.GetComponent<Button>().onClick.AddListener(() => open_edit_window(index));
       }

       FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
       
   }

   public void press_list(int index)
   {
       SaveManager.Instance.set_current_to_do_index(index);
       //Open List section
       List<int> recentList = SaveManager.Instance.recent_list();
       if(recentList.Count > 0)
       {
           if(recentList[0] != index) 
           {
               recentList.Insert(0, index);
               if(recentList.Count > 2)
               {
                   recentList.RemoveAt(2);
               }
           }
       }else
       {
           recentList.Add(index);
       }

        SaveManager.Instance.set_recent_lists(recentList);

        FindObjectOfType<Scenes>().load_to_do_list();
   }

   public void open_edit_window(int i)
    {
        currentPressed = i;
        editWindow.SetTrigger("open");
    }

    public void close_edit_window()
    {
        editWindow.SetTrigger("close");
    }

    public void open_add_list_window()
    {
        addListWindowInputField.GetComponent<InputField>().text = "";
        addListWindow.SetTrigger("open");
    }

    public void close_add_list_window()
    {
        addListWindow.SetTrigger("close");
    }

    public void add_list()
    {
        string title = addListWindowInputField.GetComponent<InputField>().text;
        
        SaveManager.Instance.add_to_do_list(title);

        addListWindow.SetTrigger("close");
        FindObjectOfType<Scenes>().load_current_page();
    }

    public void delete_list()
    {
        SaveManager.Instance.remove_to_do_list(currentPressed);
        List<int> recentList = SaveManager.Instance.recent_list();
        if (recentList.Count != 0)
        {
            if (recentList.Count == 2)
            {
                if (recentList[0] == currentPressed || recentList[1] == currentPressed)
                {
                    recentList.Remove(currentPressed);
                    SaveManager.Instance.set_recent_lists(recentList);
                }
            }else if (recentList.Count == 1)
            {
                if (recentList[0] == currentPressed)
                {
                    recentList.Remove(currentPressed);
                    SaveManager.Instance.set_recent_lists(recentList);
                }
            }
        }
       
        
        addListWindow.SetTrigger("close");
        FindObjectOfType<Scenes>().load_current_page();
    }

    void Update()
    {
        int x = Screen.width;
        int y = Screen.height;
        if(x != oldResolution[0] || y != oldResolution[1])
        {
            FindObjectOfType<scaler>().reset_container(overallContainerGO, container);
            oldResolution[0] = x;
            oldResolution[1] = y;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_main_home();
        }
    }
}

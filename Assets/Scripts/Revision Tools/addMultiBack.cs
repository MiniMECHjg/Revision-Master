using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addMultiBack : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<Scenes>().go_to_subject();
        }
    }
}

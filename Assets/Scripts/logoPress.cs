using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logoPress : MonoBehaviour
{
    public Animator logoAnim;
    public Text numText;

    public void press_button()
    {
        //plays a simple animation if a logo is clicked
        numText.text = Random.Range(0, 10).ToString();
        logoAnim.SetTrigger("logoPressed");
    }

}

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
        numText.text = Random.Range(0, 10).ToString();
        logoAnim.SetTrigger("logoPressed");
    }

}

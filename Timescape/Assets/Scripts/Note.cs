using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public string title;
    [TextArea(4,8)]
    public string text;
    public GameObject CIcon;
    private bool canRead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "Player" )
        {
            CIcon.SetActive(true);
            canRead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CIcon.SetActive(false);
            canRead = false;
        }
    }

    private void Update()
    {
        if(canRead)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                UIManager.Instance.OpenScroll(title, text);
            }
        }
    }
}

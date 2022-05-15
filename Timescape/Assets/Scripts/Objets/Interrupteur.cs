using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour
{
    public GameObject objectToEnable;
    public GameObject objectToDisable;
    private bool canInteract = false;
    public Color firstColor = Color.red;
    public Color secondColor = Color.green;
    private SpriteRenderer sr;
    bool isActivated = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = firstColor;
    }

    void Update()
    {
        if(canInteract)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!isActivated)
                {
                    Debug.Log("ActivateButton");
                    sr.color = secondColor;
                    if(objectToEnable != null)
                    {
                        objectToEnable.SetActive(true);
                    }
                    if(objectToDisable != null)
                    {
                        objectToDisable.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("DesactivateButton");
                    sr.color = firstColor;
                    if(objectToEnable != null)
                    {
                        objectToEnable.SetActive(false);
                    }
                    if (objectToDisable != null)
                    {
                        objectToDisable.SetActive(true);
                    }
                }
                isActivated = !isActivated;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canInteract = false;
        }
    }
}

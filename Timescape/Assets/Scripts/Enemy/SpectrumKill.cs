using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.TriggerLose();
        }
    }
}
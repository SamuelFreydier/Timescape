using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position;
    }
}

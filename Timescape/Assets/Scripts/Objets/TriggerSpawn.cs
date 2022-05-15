using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn : MonoBehaviour
{
    public GameObject mobPrefab;
    public Transform spawnPoint;
    public GameObject wall;
    private Note note;
    private bool triggered = false;

    private GameObject mob;

    void Start()
    {
        note = GetComponent<Note>();
    }

    void Update()
    {
        if(note.canRead && !triggered)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                triggered = true;
                mob = Instantiate(mobPrefab, spawnPoint.position, Quaternion.identity);
                wall.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        if(mob != null)
        {
            Destroy(mob);
        }
    }
}

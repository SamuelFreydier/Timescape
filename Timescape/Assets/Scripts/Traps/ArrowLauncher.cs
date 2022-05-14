using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int direction = -1; // -1 pour Left et 1 pour Right
    public float period = 2f; // Période de lancement de projectile
    private float currentTime;

    private void Start()
    {
        currentTime = period;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        Debug.Log(currentTime);
        if( currentTime < 0f )
        {
            GameObject arrow = Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
            arrow.GetComponent<Arrow>().direction = direction;
            currentTime = period;
        }
    }

    private void Launch()
    {
        Debug.Log("LAUNCH");
        
    }
}

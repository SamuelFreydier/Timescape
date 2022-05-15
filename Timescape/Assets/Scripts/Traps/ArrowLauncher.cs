using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int direction = -1; // -1 pour Left et 1 pour Right
    public float period = 2f; // Période de lancement de projectile
    private float currentTime;
    public float decalage = 0f;
    private float timeDecal = 0f;
    private AudioSource arrowLaunch;

    private void Start()
    {
        currentTime = period;
        arrowLaunch = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timeDecal < decalage)
        {
            timeDecal += Time.deltaTime;
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f)
            {
                GameObject arrow = Instantiate(arrowPrefab, new Vector3(gameObject.transform.position.x + direction, gameObject.transform.position.y), Quaternion.identity);
                arrow.GetComponent<Arrow>().direction = direction;
                currentTime = period;
                arrowLaunch.Play();
            }
        }
    }
}

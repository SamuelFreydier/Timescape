using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLeap : MonoBehaviour
{
    private GameObject residue;
    public float spellCooldown = 5f;
    private float timeRemaining = 5f;
    bool canCast = true;

    private void Start()
    {
        UIManager.Instance.UpdateCooldown(1f);
    }

    void Update()
    {
        if(!canCast)
        {
            timeRemaining += Time.deltaTime;
            if (timeRemaining >= spellCooldown)
            {
                canCast = true;
            }
            UIManager.Instance.UpdateCooldown(timeRemaining / spellCooldown);
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                if(residue == null)
                {
                    SpawnResidue();
                    AudioManager.Instance.InstantPlay("TPLoop", 1f);
                }
                else
                {
                    gameObject.transform.position = residue.transform.position;
                    Destroy(residue);
                    canCast = false;
                    timeRemaining = 0f;
                    AudioManager.Instance.InstantStop("TPLoop");
                    AudioManager.Instance.InstantPlay("Teleportation", 1f);
                }
            }
        }
    }

    private void SpawnResidue()
    {
        residue = Instantiate(new GameObject("residue"), gameObject.transform.position, Quaternion.identity);
        residue.transform.localScale = gameObject.transform.localScale;
        residue.AddComponent<SpriteRenderer>();
        SpriteRenderer sr = residue.GetComponent<SpriteRenderer>();
        sr.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        sr.color = new Color(51/255f, 74/255f, 230/255f, 128/255f);
    }

    private void OnDestroy()
    {
        if(residue != null)
        {
            Destroy(residue);
        }
        AudioManager.Instance.InstantStop("TPLoop");
    }
}

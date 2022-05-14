using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parchemin : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scrollTitle;
    public TMPro.TextMeshProUGUI scrollText;
    public Button closeBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(CloseScroll);
    }

    public void UpdateScroll(string title, string text)
    {
        scrollTitle.text = title;
        scrollText.text = text;
    }

    private void CloseScroll()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public float countdown = 5;

    private void Update()
    {
        countdown -= Time.deltaTime;
        text.text = countdown.ToString("F1");
        if(countdown < 0)
        {
            Destroy(gameObject);
        }

    }
}

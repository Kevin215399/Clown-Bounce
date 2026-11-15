using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BounceBackText : MonoBehaviour
{
    [SerializeField] private RectTransform text;
    [SerializeField] private float position0;
    [SerializeField] private float position1;
    [SerializeField] private float position2;
    [SerializeField] private float multiplier = 5;
    [SerializeField] private GameObject trails;

    public bool animate = false;
    private byte stage = 0;

    public void Animate()
    {
        stage = 0;
        animate = true;
        Debug.Log("bounce text");
    }
    private void Update()
    {
        if (animate)
        {
            if (stage == 0)
            {
                float error = position1 - text.anchoredPosition.x;
                trails.SetActive(true);
                text.anchoredPosition += new Vector2(error * Time.deltaTime * multiplier, 0);
                if (Mathf.Abs(error) < 5)
                {
                    stage++;
                }
            }
            else if (stage == 1)
            {
                float error = position2 - text.anchoredPosition.x;
                text.anchoredPosition += new Vector2(error * Time.deltaTime * multiplier, 0);
                if (Mathf.Abs(error) < 5)
                {
                    stage++;
                }
            }
            else
            {
                animate = false;
            }
        }
        else
        {
            trails.SetActive(false);
            text.anchoredPosition = new Vector2(position0, text.anchoredPosition.y);
        }
    }
}

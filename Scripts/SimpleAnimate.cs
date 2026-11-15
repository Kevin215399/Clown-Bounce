using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimate : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float variability = 0;

    public bool animating = false;
    private float timer = 0;
    private byte index = 0;
    private bool playOnce = false;
    private void Start()
    {
        timer = speed;
    }
    public void PlayOnce()
    {
        animating = true;
        index = 0;
        playOnce = true;
    }
    private void Update()
    {
        if (!animating)
            return;
        if (timer <= 0)
        {
            timer = speed + Random.Range(-variability, variability);
            sprites[index].enabled = false;
            index++;
            if (index >= sprites.Count)
            {
                index = 0;
                if (playOnce)
                {
                    playOnce = false;
                    animating = false;
                }
            }
            sprites[index].enabled = true;
        }
        timer -= Time.deltaTime;
    }

}

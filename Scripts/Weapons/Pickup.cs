using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class Pickup : MonoBehaviour
{
    [SerializeField] public bool pickedUp = false;
    [SerializeField] private Collider2D objectCollider = null;
    [SerializeField] private TMP_Text interactText;

    private void Update()
    {
        if (pickedUp)
        {
            interactText.enabled = false;
            return;
        }
        else
        {
            interactText.enabled = true;
        }
        if (Vector2.Distance(MovementScript.instance.transform.position, transform.position) < 2)
        {
            interactText.color = new Color(0, 0, 0, 1f);
        }
        else
        {
            interactText.color = new Color(0, 0, 0, .3f);
        }
    }
    public void Pick()
    {
        transform.parent = MovementScript.instance.transform;
        transform.localPosition = new Vector2(.5f, 0);
        pickedUp = true;
        objectCollider.isTrigger = true;
    }
    public void Drop()
    {
        transform.parent = null;
        pickedUp = false;
        objectCollider.isTrigger = false;
    }
    public void TempReattach()
    {
        transform.parent = MovementScript.instance.transform;
        transform.localPosition = new Vector2(.5f, 0);
    }
    public void TempSeperate()
    {
        transform.parent = null;
    }
}

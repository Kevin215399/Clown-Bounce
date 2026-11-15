using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MovementScript : MonoBehaviour
{
    public static MovementScript instance;
    [SerializeField] private InputActionReference horizontalInput;
    [SerializeField] private InputActionReference verticalInput;
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private InputActionReference pickInput;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float pickupSizse = 4;
    [SerializeField] private float health = 100;
    [SerializeField] private Slider healthbar;
    [SerializeField] private GameObject loseScreen;

    public Transform holding = null;
    private bool canPickup = true;
    private bool startShoot = true;
    private bool endShoot = true;
    private void Start()
    {
        instance = this;
    }
    public void Damage(float amount)
    {
        health -= amount;
    }
    // Update is called once per frame
    void Update()
    {
        healthbar.value = health/100;
        if(health <= 0)
        {
            loseScreen.SetActive(true);
            return;
        }
        float horizontalMove = horizontalInput.action.ReadValue<float>();
        float verticalMove = verticalInput.action.ReadValue<float>();
        Vector2 movement = new Vector2(horizontalMove, verticalMove);
        movement.Normalize();
        movement *= Time.deltaTime;
        movement *= speed;
        rb2d.AddForce(movement, ForceMode2D.Impulse);


        if (pickInput.action.IsPressed())
        {
            if (canPickup)
            {
                Debug.Log("clicked");
                Collider2D[] hit =
                        Physics2D.OverlapCircleAll(transform.position, pickupSizse);
                bool didPickup = false;
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.parent == null)
                        continue;
                    if (hit[i].transform.parent.GetComponent<Pickup>() == null)
                        continue;
                    if (hit[i].transform.parent == holding)
                        continue;
                    didPickup = true;
                    canPickup = false;
                    SetHolding(hit[i].transform.parent);
                }
                if (!didPickup && holding != null)
                {
                    canPickup = false;
                    DropObject();
                }
            }
        }
        else
        {
            canPickup = true;
        }
        if (holding != null)
        {
            if (shootInput.action.IsPressed())
            {
                endShoot = true;
                if (startShoot)
                {
                    startShoot = false;
                    holding.GetComponent<IWeapon>().Shoot();
                }
            }
            else
            {
                if (endShoot)
                {
                    endShoot = false;
                    holding.GetComponent<IWeapon>().StopShoot();
                }
                startShoot = true;
            }
        }

    }
    public void SetHolding(Transform pickup)
    {
        if (holding != null)
        {
            holding.GetComponent<Pickup>().Drop();
        }
        pickup.GetComponent<Pickup>().Pick();
        holding = pickup;
    }
    public void DropObject()
    {
        if (holding != null)
        {
            holding.GetComponent<Pickup>().Drop();
            holding = null;
        }
    }
}

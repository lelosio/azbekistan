using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    //reference
    public GunSystem gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public GameObject canvas;
    
    //floats
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    //bools
    public bool equipped;
    public static bool slotFull;



    private void Start()
    {
        gunScript = GetComponent<GunSystem>();
        
        //Setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;            
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }

      
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        
    }

    private void PickUp()
    {
        canvas.SetActive(true);
        equipped = true;
        slotFull = true;
        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;
        gunScript.enabled = true;
    }

    private void Drop()
    {
        canvas.SetActive(false);
        equipped = false;
        slotFull = false;
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        gunScript.enabled = false;
    }
}
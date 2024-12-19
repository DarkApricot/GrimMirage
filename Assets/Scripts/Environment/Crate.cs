using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private bool isGettingCarried;
    private bool isGettingThrown;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isGettingCarried && Input.GetKeyDown(KeyCode.Mouse1))
        {
            ThrowCrate();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGettingThrown = false;
        }
    }

    private void OnMouseOver()
    {
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 1 && !isGettingThrown)
        {
            isGettingCarried = true;
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3;
        }
    }

    private void ThrowCrate()
    {
        isGettingThrown = true;
        isGettingCarried = false;

        rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
    }
}

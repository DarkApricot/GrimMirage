using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool useKey;
    [SerializeField] private Button associatedButton;

     private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseOver()
    {
    }

    private void OnMouseDown()
    {
        if (useKey)
        {
            FindObjectOfType<KeyInventory>().UseKey(gameObject);
        }
        else if (associatedButton != null)
        {
            //if (associatedButton.isButtonPressed)
            //{
            //    Debug.Log("sesam open u");
            //}

            anim.enabled = associatedButton.isButtonPressed;
        }
    }


    private void Update()
    {
         if (associatedButton != null)
        {
            //if (associatedButton.isButtonPressed)
            //{
            //    Debug.Log("sesam open u");
            //}

            anim.SetBool("Open", associatedButton.isButtonPressed);
        }
    }
}

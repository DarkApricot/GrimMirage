using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamMovemnet : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.localEulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X"), 0);
        if (Camera.main.transform.localEulerAngles.x + -Input.GetAxisRaw("Mouse Y") < 80 || Camera.main.transform.localEulerAngles.x + -Input.GetAxisRaw("Mouse Y") > 280)
        {
            Camera.main.transform.localEulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y"), 0, 0);
        }
    }
}

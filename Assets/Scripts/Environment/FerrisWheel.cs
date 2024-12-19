using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    Vector3 v3;
    [SerializeField] float speed = 25f;

    void Start()
    {
        transform.localEulerAngles = v3;
    }

    void Update()
    {
        v3.x += speed * Time.deltaTime;
        transform.localEulerAngles = v3;
    }
}

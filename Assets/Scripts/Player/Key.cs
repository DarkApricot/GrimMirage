using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnMouseOver()
    {
    }

    private void OnMouseDown()
    {
        FindObjectOfType<KeyInventory>().CollectKey(gameObject);
    }
}

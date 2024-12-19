using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> heldKeys;

    public void CollectKey(GameObject _keyObj)
    {
        heldKeys.Add(_keyObj);
        _keyObj.SetActive(false);
    }

    public void UseKey(GameObject _doorObj)
    {
        for (int i = 0; i < heldKeys.Count; i++)
        {
            if (_doorObj.name.Contains(heldKeys[i].name[..1]))
            {
                Debug.Log("open sesame *throws brick*");
            }
        }
    }
}
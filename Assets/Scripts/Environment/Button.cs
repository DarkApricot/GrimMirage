using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool isButtonPressed;
    private Coroutine buttonPushing;
    private Material mat;
    Rigidbody ghg;
   
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        PerformChecks(collision);

        buttonPushing = StartCoroutine(MoveButton(new(0, 0.5f, 0)));
    }

    private void OnCollisionExit(Collision collision)
    {
        PerformChecks(collision);

        buttonPushing = StartCoroutine(MoveButton(new(0, 1.5f, 0)));
    }

    private void PerformChecks(Collision _collision)
    {
        if (PlayerCollided(_collision))
        {
            UpdateCoroutine();
        }
    }

    private bool PlayerCollided(Collision _collision)
    {
        return _collision.gameObject.GetComponent<PlayerMovement>() != null;
    }

    private void UpdateCoroutine()
    {
        if (buttonPushing != null)
        {
            StopCoroutine(buttonPushing);
        }
    }

    private IEnumerator MoveButton(Vector3 _endPos)
    {
        float _timer = 0;
        Vector3 _startPos = transform.localPosition;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(_startPos, _endPos, _timer);
            yield return new WaitForEndOfFrame();
        }

        isButtonPressed = !isButtonPressed;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (isButtonPressed)
        {
            mat.color = Color.green;
        }
        else
        {
            mat.color = Color.white;
        }
    }
}


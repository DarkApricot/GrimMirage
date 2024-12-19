using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionObjectMirror : MonoBehaviour
{
    [SerializeField] private GameObject reflectionObject;

    private Transform player;
    private LayerMask mirrorInvisibleLayer;
    private bool gamePaused;
    private Collider col;

    private bool isInMirrorView;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
        mirrorInvisibleLayer = reflectionObject.layer;
        col = GetComponent<Collider>();
    }

    void Update()
    {
        InMirrorReflection();
    }

    private void InMirrorReflection()
    {
        if (!IsGamePaused())
        {
            //Set layer reflectionObj for if Player has Mirror reflection in view
            //reflectionObject.layer = (Vector3.Dot((transform.position - player.transform.position).normalized, player.transform.forward) > 0.9f) ? 7 : 1;
        }
        else
        {
          //  reflectionObject.layer = 7;
        }
    }

    private bool IsGamePaused()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;

            if (gamePaused && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), col.bounds))
            {
                reflectionObject.layer = 1;
            }
            else
            {
                reflectionObject.layer = 7;
            }
        }


        return gamePaused;    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionPlayerWarping : MonoBehaviour
{

    private Transform player;
    private Transform playerCam;

    private bool isGamePaused;
    private Vector3 mirrorDims;
    private Vector3 regCamPos = new(0, 0.75f, 0);

    private Coroutine smoothCamTo;
    private Coroutine smoothCamBack;

    private bool isSmoothCamToDone;

    private readonly List<ReflectionPlayerWarping> allWarpingMirrors = new();
    public ReflectionPlayerWarping FocussedMirror;

    private BoxCollider col;
    Plane[] planes;

    void Start()
    {
        allWarpingMirrors.AddRange(FindObjectsOfType<ReflectionPlayerWarping>());

        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
        col = GetComponent<BoxCollider>();

        playerCam = Camera.main.transform;

        mirrorDims = transform.lossyScale;
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            isSmoothCamToDone = false;

            ClearAllSmoothCoroutines();
        }

        if (!isGamePaused)
        {
            if (InMirrorReflection())
            {
                SetMirrorFocus();

                ClearSmoothCoroutine(ref smoothCamBack);

                if (smoothCamTo == null) { smoothCamTo = StartCoroutine(SmoothCamera(playerCam.localPosition, EndPosPlayerCam())); }
                else if (isSmoothCamToDone)
                {
                    playerCam.localPosition = EndPosPlayerCam();
                }
            }
            else if (FocussedMirror == this)
            {
                ClearSmoothCoroutine(ref smoothCamTo);
                isSmoothCamToDone = false;

                if (smoothCamBack == null && playerCam.localPosition != regCamPos) { smoothCamBack = StartCoroutine(SmoothCamera(playerCam.localPosition, regCamPos)); }
            }
        }
    }

    private void SetMirrorFocus()
    {
        foreach (var _script in allWarpingMirrors)
        {
            _script.FocussedMirror = this;
            _script.ClearAllSmoothCoroutines();
        }
    }

    public void ClearAllSmoothCoroutines()
    {
        ClearSmoothCoroutine(ref smoothCamBack);
        ClearSmoothCoroutine(ref smoothCamTo);
    }

    private void ClearSmoothCoroutine(ref Coroutine _clearedCoroutine)
    {
        if (_clearedCoroutine != null)
        {
            StopCoroutine(_clearedCoroutine);
            _clearedCoroutine = null;
        }
    }

    private bool InMirrorReflection()
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), col.bounds);
        //Vector3.Dot((transform.position - player.position).normalized, player.forward) > 0.1f &&
    }

    private Vector3 EndPosPlayerCam()
    {
        float _playerDistance = Vector3.Distance(player.position, transform.position);
        float _yPos = -_playerDistance / (mirrorDims.x * (mirrorDims.y / 1.5f)) + (mirrorDims.y / mirrorDims.x);
        float _yPosClamped = Mathf.Clamp(_yPos, -0.7f, 7f);
        return new(playerCam.localPosition.x, _yPosClamped, playerCam.localPosition.z);
    }

    private IEnumerator SmoothCamera(Vector3 _startPos, Vector3 _endPos)
    {
        isSmoothCamToDone = false;

        float _timeTracker = 0;

        while (_timeTracker < 0.5f)
        {
            playerCam.localPosition = Vector3.Lerp(_startPos, _endPos, _timeTracker / 0.5f);
            _timeTracker += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        isSmoothCamToDone = !isGamePaused;
    }
}

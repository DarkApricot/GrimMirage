using UnityEngine;

public class MirrorShaderHandler : MonoBehaviour
{
    [SerializeField] private GameObject _postFXSphere;
    private Material _mat;
    [SerializeField] public Color ScreenTint;

    private RenderTexture _currentTex;
    private RenderTexture _previousTex;

    private bool isGamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_mat == null)
        {
          //  camera.GetUniversalAdditionalCameraData().scriptableRenderer.cameraColorTargetHandle
            _mat = _postFXSphere.GetComponent<MeshRenderer>().material;
        }

        // Ensure both textures are created and match source dimensions
        if (!isGamePaused)
        {
            if (_currentTex == null || _currentTex.width != source.width || _currentTex.height != source.height)
            {
                InitializeTextures(source.width, source.height, source.depth, source.format);
            }

            _mat.SetColor("_ScreenTint", ScreenTint);

            // Pass the previous texture as the main texture for feedback
            _mat.mainTexture = _previousTex;

            // First pass: Apply shader from source to _currentTex
            Graphics.Blit(source, _currentTex, _mat, 0);
        
            // Final pass: Blit _currentTex to destination
            Graphics.Blit(_currentTex, destination);

            // Swap textures for the next frame
            SwapBuffers();
        }
    }

    private void InitializeTextures(int width, int height, int depth, RenderTextureFormat format)
    {
        ReleaseTextures();

        _currentTex = new RenderTexture(width, height, depth, format);
        _previousTex = new RenderTexture(width, height, depth, format);
        _currentTex.Create();
        _previousTex.Create();
    }

    private void ReleaseTextures()
    {
        if (_currentTex != null)
        {
            _currentTex.Release();
            Destroy(_currentTex);
        }

        if (_previousTex != null)
        {
            _previousTex.Release();
            Destroy(_previousTex);
        }
    }

    private void SwapBuffers()
    {
        RenderTexture temp = _previousTex;
        _previousTex = _currentTex;
        _currentTex = temp;
    }

    private void OnDisable()
    {
        ReleaseTextures();
    }
}

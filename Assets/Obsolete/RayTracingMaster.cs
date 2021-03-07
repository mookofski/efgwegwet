    using UnityEngine;

    
    public class RayTracingMaster : MonoBehaviour
    {



        public ComputeShader RayTracingShader;
        private RenderTexture _target;
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            SetShaderParameters();
            Render(destination);
        }
        private void Render(RenderTexture destination)
        {
            // Make sure we have a current render target
            InitRenderTexture();
           
            // Set the target and dispatch the compute shader
            RayTracingShader.SetTexture(0, "Result", _target);
            int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
            RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            // Blit the result texture to the screen
if (_addMaterial == null)
    _addMaterial = new Material(Shader.Find("Hidden/AddShader"));

_addMaterial.SetFloat("_Sample", _currentSample);
Graphics.Blit(_target, destination, _addMaterial);
_currentSample++;          
  }

                public uint _currentSample = 0;
                 private Material _addMaterial;
        private void InitRenderTexture()
        {
            if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
            {
                            _currentSample = 0;

                // Release render texture if we already have one
                if (_target != null)
                    _target.Release();
                // Get a render target for Ray Tracing
                _target = new RenderTexture(Screen.width, Screen.height, 0,
                    RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                _target.enableRandomWrite = true;
                _target.Create();
            }
        }
    private void Update()
    {
        if (transform.hasChanged)
        {
            _currentSample = 0;
            transform.hasChanged = false;
        }
    }            private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    public Texture SkyboxTexture;
    private void SetShaderParameters()
    {//Name Must Match, case sensitive. but ordering does not
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));
        RayTracingShader.SetFloat("time",Time.time);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
        RayTracingShader.SetMatrix("_CameraToWorl", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
    }
    }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MicrophoneInput : MonoBehaviour {
    public float Mikesensitivity = 100.0f;
    public float loudness = 0;

    public float LoudnessFloor;
    public float widthPersecond;
    public float widthFloor;


    public Transform ScannerOrigin;
    public Material EffectMaterial;
    public float ScanDistance;

    private Camera _camera;
    
    bool _scanning;
    float currentWidth;

    public Text UITEXTLOUD;

    AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        aud.mute = true;
        aud.Play();
        aud.mute = true;

        while (!(Microphone.GetPosition(Microphone.devices[0]) > 0)) { }
    }

    bool loudEnough;


    void Update()
    {
        if (_scanning)
        {
            ScanDistance += Time.deltaTime * 50;
        }
        loudness = GetAveragedVolume() * Mikesensitivity;
        UITEXTLOUD.text = loudness.ToString();

        if(loudness >= LoudnessFloor)
        {
            if(loudEnough == false)
            {
                loudEnough = true;
                _scanning = true;
                ScanDistance = 0;
                currentWidth = widthFloor;
            }
            else
            {
                EffectMaterial.SetFloat("Scan Width", currentWidth);
                currentWidth += Time.deltaTime * widthPersecond;
            }
        }
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        aud.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }


    // scanner code


    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
        EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
        RaycastCornerBlit(src, dst, EffectMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
}

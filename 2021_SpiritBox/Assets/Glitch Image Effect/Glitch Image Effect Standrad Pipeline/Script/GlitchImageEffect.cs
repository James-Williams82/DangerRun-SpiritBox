using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchImageEffect : MonoBehaviour
{
    public enum GlitchType
    {
        Type1 = 0, 
        Type2 = 1,
        Type3 = 2,
        Type4 = 3
    }

    public GlitchType type = GlitchType.Type1;

    [Range(0, 1)]
    public float blend = 1;

    [Header("Parameters of Type1")]
	[Range(0, 10)]
	public float frequency = 1;

	[Range(0, 500)]
	public float interference = 130;

	[Range(0, 5)]
	public float noise = 0.15f;

	[Range(0, 20)]
	public float scanLine = 1;

	[Range(0, 1)]
	public float colored = 0.25f;

    [Header("Parameters of Type3")]
    [Range(0, 30)]
    public float intensityType3 = 10;

    [Header("Parameters of Type4")]
    [Range(100, 500)]
    public float lines = 240;

    [Range(1, 6)]
    public float scanSpeed = 2;

    [Range(0.1f, 0.9f)]
    public float linesThreshold = 0.7f;

    [Range(0, 0.8f)]
    public float exposure = 0.3f;

    private Shader shader = null;

    private Material mtrl = null;

    private Texture2D noiseTex = null;

    private void Awake()
    {
        shader = Shader.Find("Hidden/GlitchImageEffect");
        if (!shader.isSupported)
        {
            enabled = false;
            return;
        }

        mtrl = new Material(shader);

        noiseTex = Resources.Load<Texture2D>("GlitchNoiseTex");
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mtrl == null || mtrl.shader == null || !mtrl.shader.isSupported)
        {
            enabled = false;
            return;
        }

        mtrl.SetFloat("_Blend", blend);
		mtrl.SetFloat("_Frequency", frequency);
		mtrl.SetFloat("_Interference", interference);
		mtrl.SetFloat("_Noise", noise);
		mtrl.SetFloat("_ScanLine", scanLine);
		mtrl.SetFloat("_Colored", colored);
        mtrl.SetTexture("_NoiseTex", noiseTex);
        mtrl.SetFloat("_IntensityType3", intensityType3);
        mtrl.SetFloat("_Lines", lines);
        mtrl.SetFloat("_ScanSpeed", scanSpeed);
        mtrl.SetFloat("_LinesThreshold", linesThreshold);
        mtrl.SetFloat("_Exposure", exposure);
        Graphics.Blit(src, dest, mtrl, (int)type);
    }

    private void OnDestroy()
    {
        shader = null;

        if (mtrl != null)
        {
            DestroyImmediate(mtrl);
            mtrl = null;
        }

        if(noiseTex != null)
        {
            Resources.UnloadAsset(noiseTex);
            noiseTex = null;
        }
    }
}

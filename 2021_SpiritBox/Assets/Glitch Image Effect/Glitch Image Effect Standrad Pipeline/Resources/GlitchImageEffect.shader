Shader "Hidden/GlitchImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "GlitchImageEffect.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag2
			#pragma target 3.0
			#include "GlitchImageEffect.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag3
			#pragma target 3.0
			#include "GlitchImageEffect.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag4
			#pragma target 3.0
			#include "GlitchImageEffect.cginc"
			ENDCG
		}
	}
}

Shader "ToOn/FX/ParticleZTestAlways" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
		
	SubShader 
 	{
        Tags {"Queue" = "Transparent" "IgnoreProjector"="True" }

		BindChannels
		{
			Bind "Vertex", vertex
			Bind "texcoord", texcoord
			Bind "Color", color
		}
		Pass
		{
			Blend SrcAlpha One
			AlphaTest Greater .01
			ZTest Always
			ColorMask RGB
			Cull Off 
			Lighting Off 
			ZWrite Off 

			Fog { Mode Off } 

			SetTexture[_MainTex]
			{
				Combine Texture * Primary
			}
		}
	} 

	Fallback "VertexLit"
}

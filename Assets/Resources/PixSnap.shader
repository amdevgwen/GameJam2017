Shader "Hidden/PixSnap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorTex("Texture", 2D) = "white"{}

		_Primary("Primary", Color) = (0,1,0,0.5)
		_Secondary("Secondary", Color) = (0,1,0,0.5)
		_Tetriary("Tetriary", Color) = (0,1,0,0.5)
		_Complementary("Complementary", Color) = (0,1,0,0.5)


		_POne("Primary One", Color) = (0,1,0,0.5)
		_PTwo("Primary One", Color) = (0,1,0,0.5)
		_PThree("Primary One", Color) = (0,1,0,0.5)
		_PFour("Primary One", Color) = (0,1,0,0.5)
		
		_SOne("Secondary Color", Color) = (0,0,1,0.5)
		_STwo("Secondary Color", Color) = (1,0,0,0.5)
		_SThree("Secondary Color", Color) = (0,1,0,0.5)
		_SFour("Secondary Color", Color) = (0,0,1,0.5)

		_TOne("Tetriary One", Color) = (0,1,0,0.5)
		_TTwo("Tetriary One", Color) = (0,1,0,0.5)
		_TThree("Tetriary One", Color) = (0,1,0,0.5)
		_TFour("Tetriary One", Color) = (0,1,0,0.5)

		_COne("Complementary Color", Color) = (0,0,1,0.5)
		_CTwo("Complementary Color", Color) = (1,0,0,0.5)
		_CThree("Complementary Color", Color) = (0,1,0,0.5)
		_CFour("Complementary Color", Color) = (0,0,1,0.5)

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Primary;
			float4 _Secondary;
			float4 _Tetriary;
			float4 _Complementary;



			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

			float4 primary = col - _Primary;
			float4 secondary = col - _Secondary;
			float4 tetriary = col - _Tetriary;
			float4 complementary = col - _Complementary;

			int pint = primary[0] + primary[1] + primary[2] + primary[3];
			int sint = secondary[0] + secondary[1] + secondary[2] + secondary[3];
			int tint = tetriary[0] + tetriary[1] + tetriary[2] + tetriary[3];
			int cint = complementary[0] + complementary[1] + complementary[2] + complementary[3];


			float4 temp;
			int current = 1000;
			if (pint < current) {
				current = pint;
				temp = _Primary;
			}
			if (sint < current) {
				current = sint;
				temp = _Secondary;
			}
			if (tint < current) {
				current = tint;
				temp = _Tetriary;
			}
			if (cint < current) {
				current = cint;
				temp = _Tertiary;
			}	

			col = temp;
				return col;
			}
			ENDCG
		}
	}
}

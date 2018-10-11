// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/Overdraw" {
 Properties {
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 }
 
 SubShader {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
     
     // Include functions common to both passes
     CGINCLUDE
	 #include "UnityCG.cginc"

	 	 struct appdata{
			 float4 vertex : POSITION;
			 float4 normal : NORMAL;
			 UNITY_VERTEX_INPUT_INSTANCE_ID
		 };

         struct v2f {
             float4 vertex : SV_POSITION;
             half2 texcoord : TEXCOORD0;
			 UNITY_VERTEX_INPUT_INSTANCE_ID
			 UNITY_VERTEX_OUTPUT_STEREO
         };
             
         #pragma vertex vert
         #pragma fragment frag
 
         sampler2D _MainTex;
         float4 _MainTex_ST;
             
         v2f vert (appdata v)
         {
             v2f o;
             //o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			 UNITY_SETUP_INSTANCE_ID(v);
			 UNITY_TRANSFER_INSTANCE_ID(v, o);
			 UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			 o.vertex = UnityObjectToClipPos(v.vertex);

             return o;
         }
     ENDCG
 
     // Pass for fully visible parts of the object
     Pass {  
         ZTest LEqual
         CGPROGRAM
             fixed4 frag (v2f i) : COLOR
             {
                 fixed4 col = tex2D(_MainTex, i.texcoord);
                 return col;
             }
         ENDCG
     }
     
     //Pass for obscured parts of the object
     Pass {  
         ZTest Greater
         CGPROGRAM
             fixed4 frag (v2f i) : COLOR
             {
                 fixed4 col = tex2D(_MainTex, i.texcoord);
                 col.a *= 0.5;
				 col.r = 255;
				 col.g = 165;
                 return col;
             }
         ENDCG
     }
 }
 }

Shader "Hidden/AbstractSpriteBaseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelCountU("Pixel Count U", float) = 960
		_PixelCountV("Pixel Count V", float) = 540
    }
    SubShader
    {
        Tags
        { 
			//"Queue"="Transparent" 
			//"IgnoreProjector"="True" 
			"RenderType"="Opaque" 
			//"PreviewType"="Plane"
			//"CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        //Lighting Off
        //ZWrite Off
        //Blend One OneMinusSrcAlpha
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
				float4 color : COLOR;
				float4 screenPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float _PixelCountU;
			float _PixelCountV;
            
            v2f vert (appdata v)
            {
                v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.x = (floor(o.vertex.x*_PixelCountU)) / _PixelCountU;
				o.vertex.y = (floor(o.vertex.y*_PixelCountV)) / _PixelCountV;

				o.screenPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
				half4 col = tex2D(_MainTex, i.uv);
				col.rgb = i.color.xyz;//Apply Vertex Color

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
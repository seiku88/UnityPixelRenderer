Shader "Hidden/AbstractSpriteBaseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelCountU("Pixel Count U", float) = 960
		_PixelCountV("Pixel Count V", float) = 540
		_Cutoff("Base Alpha cutoff", Range(0,.9)) = .5
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
				o.vertex.x = (o.vertex.x);
				//o.vertex.y = floor(o.vertex.y);

				o.screenPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
				//float pixelWidth = 1.0f / _PixelCountU;
				//float pixelHeight = 1.0f / _PixelCountV;

                // sample the texture
				//fixed4 u = tex2D(_MainTex, i.uv + fixed2(0, pixelHeight));
				//fixed4 d = tex2D(_MainTex, i.uv - fixed2(0, pixelHeight));
				//fixed4 r = tex2D(_MainTex, i.uv + fixed2(pixelWidth, 0));
				//fixed4 l = tex2D(_MainTex, i.uv - fixed2(pixelWidth, 0));
                //fixed4 col = tex2D(_MainTex, i.uv);
				//col.r *= 0.75;
				//float alphaclip = 1;
				
				
				float2 uv = i.uv;
				half4 col = tex2D(_MainTex, uv);

				//fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				//if (col.a < 0.8) alphaclip = -1;

				//i.vertex.xy /= _PixelCountU;
				//i.vertex.xy = i.vertex.xy * 0.5 + 0.5;

				col.rgb = i.color.xyz;


				// clip HLSL instruction stops rendering a pixel if value is negative
				//clip(alphaclip);

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
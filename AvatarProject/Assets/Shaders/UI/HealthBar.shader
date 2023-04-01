Shader "UI/HealthBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill", float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay"}
        LOD 100

        Pass
        {
            ZTest Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            // Add compiling for instancing
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            // Define what is our instance data
            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float, _Fill)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                // Accesss the right version of the Fill property in the buffer
                UNITY_SETUP_INSTANCE_ID(v);

                float fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);

                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // Generate UVs from fill level
                o.uv = v.uv;
                o.uv.x += 0.5 - fill;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}

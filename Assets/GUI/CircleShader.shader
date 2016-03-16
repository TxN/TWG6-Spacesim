
Shader "Custom/CircleShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Angle ("Angle (Float)", Float) = 0
		_Color ("Tint", Color) = (1.0, 0.6, 0.6, 1.0)
    }

    SubShader {        
	Tags{"Queue" = "Transparent" }
	Pass {
			Blend SrcAlpha OneMinusSrcAlpha 
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
			uniform float _Angle;
			uniform fixed4 _Color;

            float4 frag(v2f_img i) : COLOR 
			{		 
				float4 result = tex2D(_MainTex, i.uv);
				float angle = i.uv.y;
		
				if(angle > _Angle)
				{				
					result.a = 0;
				}

                return result*_Color;  
			}
            ENDCG
        }
    }
}

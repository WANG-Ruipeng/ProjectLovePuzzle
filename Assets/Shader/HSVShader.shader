Shader "Custom/HSVShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Hue("Hue", Range(-180, 180)) = 0
        _Saturation("Saturation", Range(-100, 100)) = 0
        _Value("Value", Range(-100, 100)) = 0
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float _Hue;
            float _Saturation;
            float _Value;

            struct Input
            {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);

                // Convert RGB to HSV
                half maxValue = max(max(c.r, c.g), c.b);
                half minValue = min(min(c.r, c.g), c.b);
                half delta = maxValue - minValue;
                half hue;
                if (delta == 0)
                    hue = 0;
                else if (maxValue == c.r)
                    hue = (c.g - c.b) / delta;
                else if (maxValue == c.g)
                    hue = (c.b - c.r) / delta + 2;
                else
                    hue = (c.r - c.g) / delta + 4;
                hue /= 6;

                half saturation = maxValue == 0 ? 0 : delta / maxValue;
                half value = maxValue;

                // Apply hue shift
                hue = (hue + _Hue / 360) % 1;

                // Apply saturation
                saturation = clamp(saturation + _Saturation * 0.01, 0, 1);

                // Apply value
                value = clamp(value + _Value * 0.01, 0, 1);

                // Convert HSV back to RGB
                int hi = (int)(hue * 6);
                float f = hue * 6 - hi;
                float p = value * (1 - saturation);
                float q = value * (1 - f * saturation);
                float t = value * (1 - (1 - f) * saturation);

                switch (hi)
                {
                    case 0: c.r = value; c.g = t; c.b = p; break;
                    case 1: c.r = q; c.g = value; c.b = p; break;
                    case 2: c.r = p; c.g = value; c.b = t; break;
                    case 3: c.r = p; c.g = q; c.b = value; break;
                    case 4: c.r = t; c.g = p; c.b = value; break;
                    case 5: c.r = value; c.g = p; c.b = q; break;
                }

                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
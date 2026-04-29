Shader "Custom/FOVStencil"
{
    SubShader
    {
        Tags { "Queue"="Geometry-10" }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            ColorMask 0
            ZWrite Off
        }
    }
}

Shader "xiao_D/PostEffect/WaterWave/Repeat"   
{  
    Properties   
    {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
    }  
  
    CGINCLUDE  
    #include "UnityCG.cginc"  
    uniform sampler2D _MainTex;  
    uniform float _distanceFactor;  
    uniform float _timeFactor;  
    uniform float _totalFactor;  
    uniform float _waveWidth;  
    //uniform float _curWaveDis;  
  
    fixed4 frag(v2f_img i) : SV_Target  
    {  
       //计算uv到中间点的向量(向外扩，反过来就是向里缩)  
        half2 dv = float2(0.5, 0.5) - (float2)i.uv;  

        //计算像素点距中点的距离  
        half dis = sqrt(dv.x * dv.x + dv.y * dv.y);  


        //用sin函数计算出波形的偏移值factor  
        //dis在这里都是小于1的，所以我们需要乘以一个比较大的数，比如60，这样就有多个波峰波谷  
        //sin函数是（-1，1）的值域，我们希望偏移值很小，所以这里我们缩小100倍，据说乘法比较快,so...  
        half sinFactor = sin(dis * _distanceFactor + _Time.y * _timeFactor) * _totalFactor * 0.01;  

		dv=clamp(dv,0.01,1);		//过小出问题
		//像素采样时偏移offset
		fixed4 c=tex2D(_MainTex,normalize(dv)* sinFactor + i.uv);

		return c;
    }  
  
    ENDCG  
  
    SubShader   
    {  
        Pass  
        {  
            ZTest Always  
            Cull Back 
            ZWrite Off
            Fog { Mode off }  
  
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag  
            ENDCG  
        }  
    }  
    Fallback off  
}  
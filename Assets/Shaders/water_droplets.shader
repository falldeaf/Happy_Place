Shader "Unlit/RainyWindow"
{
 Properties
 {
  _MainTex ("Texture", 2D) = "white" {}
  _Size ("Size", float) = 1
  
  _Blur("Blur", range(0., .1)) = .1

  _Aspect("Aspect", float) = 1
  _DropSize("Drop Size", range(.01, .1)) = .1
  _TrailSize("Trail Size", range(0, 1)) = 1

  _Distortion("Distortion", range(-1,1)) = .1
  _MinFog("MinFog", range(0,8)) = .5
  _MaxFog("MaxFog", range(0,8)) = .5
 }
 SubShader
 {
  Tags { "RenderType"="Opaque" "Queue"="Transparent" }
  LOD 100

  GrabPass {"_GrabTexture"}
  Pass
  {
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  // make fog work
  #pragma multi_compile_fog
   
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
   float4 grabUv : TEXCOORD1;
  };

  sampler2D _MainTex, _GrabTexture;
  float4 _MainTex_ST;
   
  v2f vert (appdata v)
  {
   v2f o;
   o.vertex = UnityObjectToClipPos(v.vertex);
   o.uv = TRANSFORM_TEX(v.uv, _MainTex);

   float4 pos = UnityObjectToClipPos(v.vertex);
   o.grabUv = ComputeGrabScreenPos(pos);

   return o;
  }
   
  float _Size, _TrailSize, _Aspect, _DropSize, _Distortion, _MinFog, _MaxFog, _Blur;

  float N21(float2 p) {
   p = frac(p*float2(123.345, 345.456));
   p += dot(p, p + 1000.23);
   return frac(p.x*p.y);
  }

  float4 Layer(float2 UV, float t) {
   float2 aspect = float2(2, 1);
   float2 uv = (UV - .5)*aspect*_Size;
   uv.y += t * .25;

   float2 gv = frac(uv) - .5;
   float2 id = floor(uv);

   float n = N21(id);

   float x = frac(n * 10) - .5;

   float w = UV.y + n;
   x = x * .75 + .1*sin(w * 10*_Size)*pow(sin(w), 6)*(.5 - x * x);
   //x = 0;
   float y = t + n * 6.2831;

   y = -sin(y + sin(y) + .2*sin(2 * y))*.4;
   y -= (x-gv.x)*(x-gv.x);      // add sag

   float2 dropPos = float2(x, y);
   float2 dp = (dropPos - gv) / aspect;
   float drop = smoothstep(_DropSize, _DropSize*.3, length(dp));

   y = t * .25;
   float2 trailPos = float2(x, y) - gv;
   trailPos.y = frac(trailPos.y * 8) - .5;
   aspect.y *= 8;
   float trail = smoothstep(_DropSize*_TrailSize, 0, length(trailPos / aspect));

   float trailMask = smoothstep(.5, dropPos.y, gv.y);
   trailMask *= smoothstep(dropPos.y - _DropSize, dropPos.y, gv.y);

   float fogTrail = trailMask * smoothstep(_DropSize*2., _DropSize*.9, abs(gv.x - x));

   //col += drop;
   //col += trail*trailMask;

   dropPos -= gv;
   float z = sqrt(max(0, _DropSize * _DropSize - dp.x*dp.x - dp.y*dp.y))/_DropSize;
   float2 offs = dp*drop;
   offs = normalize(float3(dp.x, z, dp.y)).xz*smoothstep(0., .5, drop);

   offs += trailPos * trail * trailMask / aspect;
   //offs = normalize(float3(offs.x, 1, offs.y));
   //if (abs(gv.x) > .485 || abs(gv.y) > .485) col.rgb = float3(1, 0, 0);

   fogTrail = max(fogTrail, max(drop, trail*trailMask));
   //fogTrail = 0;
   return float4(offs, fogTrail, z);
  }

  fixed4 frag (v2f i) : SV_Target
  {
   fixed4 col = 0;

  float t = fmod(_Time.y,1000);

  float4 drops = Layer(i.uv, t);
  drops += Layer(i.uv*1.3 + 123.45, t);
  drops += Layer(i.uv*1.56 - 23.45, t);
  drops += Layer(i.uv*1.66 - 243.45, t);

  float hole = smoothstep(.3, .1, length(i.uv - .5));
  //drops.xy *= 1.-hole;
  //drops.z += hole;

  col = tex2Dlod(_MainTex, float4(i.uv+drops.xy*_Distortion*10, 0, lerp(_MaxFog, _MinFog, drops.z)));
  //col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.grabUv+float4(drops.xy,0,0)))*.9;
  float4 projUv = UNITY_PROJ_COORD(i.grabUv + float4(drops.xy, 0, 0));
  projUv.xy /= projUv.w;
  projUv.w = 5;

  col = 0;

  float n = N21(i.uv)*6.2831;

  float d = (1 - drops.z)*_Blur;

  const float samples = 32;
  for (float i = 0; i < samples; i++) {
   n += i;
   float dist = frac(sin((i+1)*234.4)*546.2);
   //dist *= dist;
   //dist = 1;
   dist = sqrt(dist);
   col += tex2D(_GrabTexture, projUv.xy+ float2(sin(n), cos(n))*d*dist);
  }
  col /= samples;
  //col *= 0; col.rg = drops.xy*10;
   return col*.9;
  }
  ENDCG
  }
 }
}
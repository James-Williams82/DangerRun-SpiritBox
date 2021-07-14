#ifndef __GLITCH_IMAGE_EFFECT__
#define __GLITCH_IMAGE_EFFECT__

#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float4 pos : SV_POSITION;
	float4 scrPos : TEXCOORD0;
	float2 uv : TEXCOORD1;
};

sampler2D _MainTex;
float4 _MainTex_TexelSize;

uniform sampler2D _NoiseTex;
uniform float _Blend;
uniform float _Frequency;
uniform float _Interference;
uniform float _Noise;
uniform float _ScanLine;
uniform float _Colored;
uniform float _IntensityType3;

v2f vert(appdata v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.scrPos = ComputeScreenPos(o.pos);
	o.uv = v.uv;
	return o;
}

float3 abc(float3 x) {
	return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float2 abc(float2 x) {
	return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float3 def(float3 x) {
	return abc(((x*34.0) + 1.0)*x);
}

float ns(float2 v)
{
	const float4 C = float4(0.211324865405187,
		0.366025403784439,
		-0.577350269189626,
		0.024390243902439);

	float2 i = floor(v + dot(v, C.yy));
	float2 x0 = v - i + dot(i, C.xx);

	float2 i1;
	i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
	float4 x12 = x0.xyxy + C.xxzz;
	x12.xy -= i1;

	i = abc(i);
	float3 p = def(def(i.y + float3(0.0, i1.y, 1.0))
		+ i.x + float3(0.0, i1.x, 1.0));

	float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
	m = m*m;
	m = m*m;

	float3 x = 2.0 * frac(p * C.www) - 1.0;
	float3 h = abs(x) - 0.5;
	float3 ox = floor(x + 0.5);
	float3 a0 = x - ox;

	m *= 1.79284291400159 - 0.85373472095314 * (a0*a0 + h*h);

	float3 g;
	g.x = a0.x  * x0.x + h.x  * x0.y;
	g.yz = a0.yz * x12.xz + h.yz * x12.yw;
	return _Interference * dot(m, g);
}

float ad(float2 co)
{
	return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

fixed4 frag(v2f i) : SV_Target
{
	float2 uvs = (i.scrPos.xy / i.scrPos.w) * _ScreenParams.xy;
	float4 c = 0;
	float2 uv = uvs.xy / _ScreenParams.xy;
	float time = _Time.y * _Frequency;
	float noise = max(0.0, ns(float2(time, uv.y * 0.3)) - 0.3) * (1.0 / 0.7);
	noise = noise + (ns(float2(time*10.0, uv.y * 2.4)) - 0.5) * _Noise;
	float xpos = uv.x - noise * noise * 0.25;
	c = tex2D(_MainTex, float2(xpos, uv.y));
	float misrifl0 = uv.y * time;
	float misrifl = ad(float2(misrifl0, misrifl0));
	c.rgb = lerp(c.rgb, float3(misrifl, misrifl, misrifl), noise * 0.3).rgb;
	if (floor(fmod(uvs.y * 0.25, 2.0)) == 0.0)
	{
		c.rgb *= _ScanLine - (0.15 * noise);
	}
	c.g = lerp(c.r, tex2D(_MainTex, float2(xpos + noise * 0.05, uv.y)).g, _Colored);
	c.b = lerp(c.r, tex2D(_MainTex, float2(xpos - noise * 0.05, uv.y)).b, _Colored);
	half4 mainC = tex2D(_MainTex, uv);
	return c * _Blend + mainC * (1 - _Blend);
}

float rnd(float n){return frac(sin(n) * 43758.5453123);}

float nie(float p){
    float fl = floor(p);
    float fc = frac(p);
    return lerp(rnd(fl), rnd(fl + 1.0), fc);
}

float bnie(float2 uv, float threshold, float scale, float seed)
{
    float scroll = floor(_Time.y + sin(11.0 *  _Time.y) + sin(_Time.y) ) * 0.77;
    float2 noiseUV = uv.yy / scale + scroll;
    float noise2 = tex2D(_NoiseTex, noiseUV).r;
    
    float id = floor( noise2 * 20.0);
    id = nie(id + seed) - 0.5;
    
  
    if ( abs(id) > threshold )
        id = 0.0;

    return id;
}

half4 frag2(v2f i) : SV_Target
{
    float rgbIntesnsity = 0.1 + 0.1 * sin(_Time.y* 3.7);
    float displaceIntesnsity = 0.2 +  0.3 * pow( sin(_Time.y * 1.2), 5.0);
    float interlaceIntesnsity = 0.01;
    float dropoutIntensity = 0.1;

    float2 uv = i.scrPos.xy/i.scrPos.w;

    float displace = bnie(uv + float2(uv.y, 0.0), displaceIntesnsity, 25.0, 66.6);
    displace *= bnie(uv.yx + float2(0.0, uv.x), displaceIntesnsity, 111.0, 13.7);
    
    uv.x += displace;
    
    float2 offs = 0.1 * float2(bnie(uv.xy + float2(uv.y, 0.0), rgbIntesnsity, 65.0, 341.0), 0.0);
    
    float colr = tex2D(_MainTex, uv-offs).r;
    float colg = tex2D(_MainTex, uv).g;
    float colb = tex2D(_MainTex, uv +offs).b;
    
    float li = frac(uv.y / 3.0);
    float3 mask = float3(3.0, 0.0, 0.0);
        if (li > 0.333)
            mask = float3(0.0, 3.0, 0.0);
        if (li > 0.666)
            mask = float3(0.0, 0.0, 3.0);
    
    
    float maskNoise = bnie(uv, interlaceIntesnsity, 90.0, _Time.y) * max(displace, offs.x);
    
    maskNoise = 1.0 - maskNoise;
    if ( maskNoise == 1.0)
        mask = float3(1.0,1.0,1.0);
    
    float dropout = bnie(uv, dropoutIntensity, 11.0, _Time.y) * bnie(uv.yx, dropoutIntensity, 90.0, _Time.y);
    mask *= (1.0 - 5.0 * dropout);
    
	half4 finalC = half4(mask * half3(colr, colg, colb), 1.0);
	half4 mainC = tex2D(_MainTex, i.scrPos.xy/i.scrPos.w);
	return finalC * _Blend + mainC * (1 - _Blend);
}

float sat( float t ) {
	return clamp( t, 0.0, 1.0 );
}

float2 sat( float2 t ) {
	return clamp( t, 0.0, 1.0 );
}

float remap  ( float t, float a, float b ) {
	return sat( (t - a) / (b - a) );
}

float linterp( float t ) {
	return sat( 1.0 - abs( 2.0*t - 1.0 ) );
}

float3 spectrum_offset( float t ) {
    float t0 = 3.0 * t - 1.5;
	return clamp( float3( -t0, 1.0-abs(t0), t0), 0.0, 1.0);
}

float rand( float2 n ) {
  return frac(sin(dot(n.xy, float2(12.9898, 78.233)))* 43758.5453);
}

float srand( float2 n ) {
	return rand(n) * 2.0 - 1.0;
}

float mytrunc( float x, float num_levels )
{
	return floor(x*num_levels) / num_levels;
}

float2 mytrunc( float2 x, float num_levels )
{
	return floor(x*num_levels) / num_levels;
}

fixed4 frag3(v2f i) : SV_Target
{
	float2 uv = (i.scrPos.xy / i.scrPos.w);
	float4 c = 0;
	half4 mainC = tex2D(_MainTex, uv);

	float aspect = _ScreenParams.x / _ScreenParams.y;

	float time = fmod(_Time.y, 32.0);

	float GLITCH = 0.01 * _IntensityType3;

	float gnm = sat( GLITCH );
	float rnd0 = rand( mytrunc( float2(time, time), 6.0 ) );
	float r0 = sat((1.0-gnm)*0.7 + rnd0);
	float rnd1 = rand( float2(mytrunc( uv.x, 10.0*r0 ), time) );
	float r1 = 0.5 - 0.5 * gnm + rnd1;
	r1 = 1.0 - max( 0.0, ((r1<1.0) ? r1 : 0.9999999) ); 
	float rnd2 = rand( float2(mytrunc( uv.y, 40.0*r1 ), time) ); 
	float r2 = sat( rnd2 );

	float rnd3 = rand( float2(mytrunc( uv.y, 10.0*r0 ), time) );
	float r3 = (1.0-sat(rnd3+0.8)) - 0.1;

	float pxrnd = rand( uv + time );

	float ofs = 0.05 * r2 * GLITCH * ( rnd0 > 0.5 ? 1.0 : -1.0 );
	ofs += 0.5 * pxrnd * ofs;

	uv.y += 0.1 * r3 * GLITCH;

    const int NUM_SAMPLES = 10;
    const float RCP_NUM_SAMPLES_F = 1.0 / float(NUM_SAMPLES);
    
	float4 sum = float4(0,0,0,0);
	float3 wsum = float3(0,0,0);
	for( int i=0; i<NUM_SAMPLES; ++i )
	{
		float t = float(i) * RCP_NUM_SAMPLES_F;
		uv.x = sat( uv.x + ofs * t );
		float4 samplecol = tex2D( _MainTex, uv);
		float3 s = spectrum_offset( t );
		samplecol.rgb = samplecol.rgb * s;
		sum += samplecol;
		wsum += s;
	}
	sum.rgb /= wsum;
	c = sum;

	return c * _Blend + mainC * (1 - _Blend);
}

float4 hash42(float2 p){
	float4 p4 = frac(float4(p.xyxy) * float4(443.8975,397.2973, 491.1871, 470.7827));
	p4 += dot(p4.wzxy, p4+19.19);
	return frac(float4(p4.x * p4.y, p4.x*p4.z, p4.y*p4.w, p4.x*p4.w));
}

float hash( float n ){
	return frac(sin(n)*43758.5453123);
}

float n( in float3 x ){
	float3 p = floor(x);
	float3 f = frac(x);
	f = f*f*(3.0-2.0*f);
	float n = p.x + p.y*57.0 + 113.0*p.z;
	float res = lerp(lerp(lerp(hash(n+0.0),hash(n+1.0),f.x),lerp(hash(n+57.0),hash(n+58.0),f.x),f.y),lerp(lerp(hash(n+113.0),hash(n+114.0),f.x),lerp(hash(n+170.0),hash(n+171.0),f.x),f.y),f.z);
	return res;
}

float _Lines;
float _ScanSpeed;
float _LinesThreshold;
float _Exposure;

fixed4 frag4(v2f i) : SV_Target
{
	float2 fc = i.scrPos.xy/i.scrPos.w;
	float4 c = 0;
	float2 uv = fc.xy;

	float linesN = _Lines;
	float one_y = _ScreenParams.y / linesN;
	uv = floor(uv*_ScreenParams.xy/one_y)*one_y;

	float blend = 0;
	float y = uv.y;
	float s = _Time.y*_ScanSpeed;	
	float v = (n(float3(y*.01+s,1., 1.0))+.0)*(n(float3(y*.011+1000.0+s,1.,1.0))+.0)*(n(float3(y*.51+421.0+s,1.,1.0))+.0);
	v*= hash42(float2(uv.x+_Time.y*0.01,uv.y)).x+.3;
	v = v+_Exposure;
	if(v<_LinesThreshold) v = 0.;
	blend = v;
	
	half4 mainC = tex2D(_MainTex, fc.xy);
	c = lerp(mainC, mainC + half4(0.3,0.3,0.3,1) * blend, blend);
	return c;
}

#endif
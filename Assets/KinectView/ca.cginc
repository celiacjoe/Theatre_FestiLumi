float cross2d( in float2 a, in float2 b ) { return a.x*b.y - a.y*b.x; }

float2 invBilinear( in float2 p, in float2 a, in float2 b, in float2 c, in float2 d ){
float2 res = float2(-1.0,-1.0);
float2 e = b-a;
float2 f = d-a;
float2 g = a-b+c-d;
float2 h = p-a;        
float k2 = cross2d( g, f );
float k1 = cross2d( e, f ) + cross2d( h, g );
float k0 = cross2d( h, e );       
float w = k1*k1 - 4.0*k0*k2;
if( w<0.0 ) return float2(-1.0,-1.0);
w = sqrt( w );       
float ik2 = 0.5/k2;
float v = (-k1 - w)*ik2; if( v<0.0 || v>1.0 ) v = (-k1 + w)*ik2;
float u = (h.x - f.x*v)/(e.x + g.x*v);
if( u<0.0 || u>1.0 || v<0.0 || v>1.0 ) return float2(-1.0,-1.0);
res = float2( u, v );		   
return res;}
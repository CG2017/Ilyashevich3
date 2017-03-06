sampler2D input : register(s0);

// new HLSL shader
// modify the comment parameters to reflect your shader parameters

/// <summary>Difference.</summary>
/// <minValue>0/minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.12</defaultValue>
float Tolerance : register(C0);
/// <summary>The color that becomes changed.</summary>
/// <defaultValue>Green</defaultValue>
float4 KeyColor : register(C1);

/// <summary>The target color.</summary>
/// <defaultValue>Yellow</defaultValue>
float4 TargetColor : register(C2);

float3 rgb_to_lab(float3 rgb : COLOR) : COLOR;
float3 lab_to_rgb(float3 lab : COLOR) : COLOR;

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	
	float4 color; 
	color= tex2D( input , uv.xy); 

  float3 rgb_source = color.rgb / color.a;
  float3 lab_source = rgb_to_lab(rgb_source);
  
  float3 lab_target = rgb_to_lab(TargetColor.rgb / TargetColor.a);
  float3 lab_key = rgb_to_lab(KeyColor.rgb / KeyColor.a);
  
  float dif_l = lab_source.r - lab_key.r;
  float dif_a = lab_source.g - lab_key.g;
  float dif_b = lab_source.b - lab_key.b;
  
  float4 rgb3 = color;
  
  if (sqrt( pow(abs(dif_l), 2) + pow(abs(dif_a), 2) + pow(abs(dif_b), 2)) < Tolerance)
	{
      float3 lab_source2 = float3(lab_target.r + dif_l, lab_target.g + dif_a, lab_target.b + dif_b);
      float3 rgb2 = lab_to_rgb(lab_source2);
      rgb3 = float4(rgb2 * color.a, color.a);
  }
  else
  {
  }
	return rgb3;
}

float3 rgb_to_lab(float3 rgb : COLOR)
{
	float var_R = ( rgb.r / 255.0f );        //R from 0 to 255
	float var_G = ( rgb.g / 255.0f );        //G from 0 to 255
	float var_B = ( rgb.b / 255.0f );        //B from 0 to 255

	if ( var_R > 0.04045 ) 
	{
		var_R = pow( (abs( var_R + 0.055f ) / 1.055f ), 2.4f);
	}
	else  
	{
		var_R = var_R / 12.92f;
	}
	if ( var_G > 0.04045f ) 
	{
		var_G = pow( ( abs( var_G + 0.055f ) / 1.055f ), 2.4f);
	}
	else
	{
		var_G = var_G / 12.92f;
	}
	if ( var_B > 0.04045f ) 
	{
		var_B = pow( ( abs( var_B + 0.055f ) / 1.055f ), 2.4f);
	}
	else 
	{
		var_B = var_B / 12.92f;
	}

	var_R = var_R * 100.0f;
	var_G = var_G * 100.0f;
	var_B = var_B * 100.0f;

	//Observer. = 2°, Illuminant = D65
	float X = var_R * 0.4124f + var_G * 0.3576f + var_B * 0.1805f;
	float Y = var_R * 0.2126f + var_G * 0.7152f + var_B * 0.0722f;
	float Z = var_R * 0.0193f + var_G * 0.1192f + var_B * 0.9505f;
	
	float ref_X = 95.047f;
  float ref_Y = 100.000f;
  float ref_Z = 108.883f;
  float var_X = X/ref_X;          //ref_X =  95.047   Observer= 2°, Illuminant= D65
  float var_Y = Y/ref_Y;          //ref_Y = 100.000
  float var_Z = Z/ref_Z;         //ref_Z = 108.883

  if (var_X > 0.008856f) 
  {
  	var_X = pow(abs(var_X), (1.0f/3.0f));
  }
  else 
  {
  	var_X = (7.787*var_X) + (16.0f/116.0f);
  }
  if (var_Y > 0.008856f) 
  {
  	var_Y = pow(abs(var_Y), (1.0f/3.0f));
  }
  else 
  {
  	var_Y = (7.787f*var_Y) + (16.0f/116.0f);
  }
  if (var_Z > 0.008856f) 
  {
  	var_Z = pow(abs(var_Z), 1.0f/3.0f);
  }
  else 
  {
  	var_Z = (7.787f*var_Z) + (16.0f/116.0f);
  }
	
	return float3((116.0f*var_Y) - 16.0f,(116.0f*var_Y) - 16.0f, 200.0f*(var_Y - var_Z));
}

float3 lab_to_rgb(float3 lab) : COLOR
{
						float var_Y = (lab.r + 16.0f)/116.0f;
            float var_X = lab.g / 500.0f + var_Y;
            float var_Z = var_Y - lab.b /200.0f;

            if (pow(var_Y, 3.0f) > 0.008856f) 
            {
            	var_Y = pow(var_Y, 3.0f);
            }
            else 
            {
            	var_Y = (var_Y - 16.0f/116.0f)/7.787f;
            }
            if (pow(var_X, 3.0f) > 0.008856f) 
            {
            	var_X = pow(var_X, 3.0f);
            }
            else 
            {
            	var_X = (var_X - 16.0f/116.0f)/7.787f;
            }
            if (pow(var_Z, 3.0f) > 0.008856f) 
            {
            	var_Z = pow(var_Z, 3.0f);
            }
            else 
            {
            	var_Z = (var_Z - 16.0f/116.0f)/7.787f;
            }

            float ref_X = 95.047f;
            float ref_Y = 100.000f;
            float ref_Z = 108.883f;
            float X = ref_X*var_X;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
            float Y = ref_Y*var_Y;     //ref_Y = 100.000
            float Z = ref_Z*var_Z;     //ref_Z = 108.883


            var_X = X/100.0f;        //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
            var_Y = Y/100.0f;        //Y from 0 to 100.000
            var_Z = Z/100.0f;        //Z from 0 to 108.883

            float var_R = var_X*3.2406f + var_Y*-1.5372f + var_Z*-0.4986f;
            float var_G = var_X*-0.9689f + var_Y*1.8758f + var_Z*0.0415f;
            float var_B = var_X*0.0557f + var_Y*-0.2040f + var_Z*1.0570f;

            if (var_R > 0.0031308f) 
            {
            	var_R = 1.055f*pow(abs(var_R), 1.0f/2.4f) - 0.055f;
            }
            else 
            {
            	var_R = 12.92f*var_R;
            }
            if (var_G > 0.0031308f) 
            {
            	var_G = 1.055f* pow(abs(var_G), 1.0f/2.4f) - 0.055f;
            }
            else 
            {
            	var_G = 12.92f*var_G;
            }
            if (var_B > 0.0031308f) 
            {
            	var_B = 1.055f* pow(abs(var_B), 1.0f/2.4f) - 0.055f;
            }
            else 
            {
            	var_B = 12.92f*var_B;
            }


						return float3(var_R*255.0f, var_G*255.0f, var_B*255.0f);
}


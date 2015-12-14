Shader "Toon/my_Toon_Shader" {
	Properties {
		_Color ("Diffuse Material Color", Color) = (1,1,1,1) //main colour
   	 	_UnlitColor ("Unlit Color", Color) = (0.5,0.5,0.5,1) //shadow colour
   		_DiffuseThreshold ("Lighting Threshold", Range(0,1)) = 0.1 //the amount of shadow on the object
   		
   		_OutlineThickness ("Outline Thickness", Range(0,1)) = 0.1
   		_MainTex ("Base (RGB)", 2D) = "white" { } 
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { } //this needs to go into the new shader
	}
	
	//Ok something has gonna kinda wrong
	//At least something is there but clearly it's taking the MainTex as the shadow
	
	SubShader {
	        Tags{"RenderType"="Opaque"  }
			UsePass "Toon/Basic/BASE"
     Pass {

        // pass for ambient light and first light source
	
	
		CGPROGRAM
		#pragma vertex vert
        //tells the cg to use a vertex-shader called vert
        
        #pragma fragment frag
        //tells the cg to use a fragment-shader called frag
        
        //== User defined ==//
       
        //TOON SHADING UNIFORMS
        uniform float4 _Color;
        uniform float4 _UnlitColor;
        uniform float _DiffuseThreshold;
        uniform float _OutlineThickness;      
        
        //== UNITY defined ==//
        uniform float4 _LightColor0;
        uniform sampler2D _MainTex;
        uniform samplerCUBE _ToonShade;
        uniform float4 _MainTex_ST;            
        
		struct vertexInput { // info from unity
			//TOON SHADING VAR
	        float4 vertex : POSITION;
	        float3 normal : NORMAL;
	        float4 texcoord : TEXCOORD0;
		};
		
		struct vertexOutput { // info given to unity          
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD1;
                float4 lightDir : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float2 uv : TEXCOORD0;
                float3 cubenormal : TEXCOORD4;
		};
		
		//fucking syntax always messes things up :(
		
		 vertexOutput vert(vertexInput input) //how the light should behave with the mesh
        {
        	vertexOutput output;
                        //normalDirection
                output.normalDir = normalize ( mul( float4( input.normal, 0.0 ), _World2Object).xyz ); // transforms normals to worldspace
               
                //World position
                float4 posWorld = mul(_Object2World, input.vertex); //local space position
               
                //view direction
                output.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz ); //vector from object to the camera
               
                //light direction
                float3 fragmentToLightSource = ( _WorldSpaceCameraPos.xyz - posWorld.xyz);
                output.lightDir = float4(
                        normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w) ),
                        lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
                );
               
                //fragmentInput output;
                output.pos = mul( UNITY_MATRIX_MVP, input.vertex );  
               
                //UV-Map
                output.uv =input.texcoord;
                
                output.cubenormal = mul (UNITY_MATRIX_MV, float4(input.normal,0));
                
        	return output;
        }
        
        float4 frag(vertexOutput input) : COLOR // this is where it gets tricky :(
        {
        	
        float nDotL = saturate(dot(input.normalDir, input.lightDir.xyz)); //gets the dot product and restricts it (saturate) between 0,1
                       
        //Diffuse threshold calculation
        float diffuseCutoff = saturate( ( max(_DiffuseThreshold, nDotL) - _DiffuseThreshold )* 1000); // the 1000 gives it a hard edge rather then a gradual fade
        
        //Calculate Outlines
        float outlineStrength = saturate( (dot(input.normalDir, input.viewDir ) - _OutlineThickness) * 1000 );
               
        fixed4 cube = texCUBE(_ToonShade, input.cubenormal);               
        float3 ambientLight = (1-diffuseCutoff) * _UnlitColor.xyz; //adds general ambient illumination
        float3 diffuseReflection = 2.0f * cube.rgb * _Color.xyz * diffuseCutoff;
               
        float3 combinedLight = (ambientLight + diffuseReflection) * outlineStrength;
                       //let's see if commenting this out helps
        return float4(combinedLight, 1.0);// + tex2D(_MainTex, input.uv); // DELETE LINE COMMENTS & ';' TO ENABLE TEXTURE
               
        }

		ENDCG
	} 
}
//	FallBack "Diffuse" //This is grand, can be left the way it is but needs to be commented out while I test :)
}
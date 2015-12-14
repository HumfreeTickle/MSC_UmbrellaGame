Shader "unityShaders/testing/2_lambert"{ //name stuff
	Properties {
		_Color ("Color", Color) = (1,1,1,1) //all the color base information
	}
	SubShader{
		Pass{
			Tags{"Lightmode" = "ForwardBase" } //what lightmode you are using in the scene
			//helps to blend the shaders together
			//uses a forward rendering, not sure what that means but thats what ForwardBase does
		
			CGPROGRAM//start programming in CG
			
			//where to find the vertex and fragment stuff
			#pragma vertex vert
			#pragma fragment frag
			
			//user defined variables
			uniform float4 _Color; //define our colour so we can access it
			
			//Unity defined variables
			uniform float4 _LightColor0; // defines the float 4 colour so we can use the light olur in the shader 
										// ie. the directional light bouning off it
			
			//base input struts
			struct vertexInput{
			//grabs the position and normal from the objects information
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct vertexOutput{
			//output the position and colour
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};
			
			//vertex function
			vertexOutput vert(vertexInput v){ //fuction to say what gets outputed
											  //so all the calculation stuff
											  //takes the inputs as a base and works off that
											  
				vertexOutput o; //not a float4 but a full on class
				
				float3 normalDirection = normalize( mul( float4( v.normal, 0.0 ), _World2Object ).xyz ); 
										//multiply the normal vector by the world to object transpos
										//then normalize that to keep it in a nice range of -1,1
				float3 lightDirection; //define a lightdrection
				float atten = 1.0; //define an attenuation
				
				lightDirection = normalize(_WorldSpaceLightPos0.xyz); //normalize the world light position
				
				float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection) );
										   //something in here (atten  or _LightColor0.xyz) has been changed between 4 and 5
										   //need to figure out what cause at the moment, the colour is just the reflection of
										   //the direcional light rather then the object itself	
										   
										   //Nevermind _LightColor0.xyz is for the directional light
										   //Color.rgb is for the colour of the object itself	
				float3 lightFinal = diffuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
										   // adds ambient lighting into the mix
										   // its not added into the equation straight awat because that would just tint the colour
										   			   				
				o.col = float4(lightFinal * _Color.rgb, 1.0); //assigns the calculated diffuse colour and then adds the 4th value of alpha (1.0)
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); //finds the position
				return o;
			}
			
			//fragment function
			float4 frag(vertexOutput i) : COLOR
			{
				return i.col;
			}
			
			ENDCG
		}
	}
	//Fallback "Diffuse"
 } 
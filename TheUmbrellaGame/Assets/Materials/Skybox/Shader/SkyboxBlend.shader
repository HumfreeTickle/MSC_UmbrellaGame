Shader "RenderFX/Skybox Blended" {  
Properties {  
    _Tint ("Tint Color", Color) = (.5, .5, .5, .5)  
    _Blend ("Blend", Range(0.0,1.0)) = 0.5  
    
// ------------------ Dawn Textures --------------------// 
        
    _FrontTex ("Front_0 (+Z)", 2D) = "white" {}  
    _BackTex ("Back_0 (-Z)", 2D) = "white" {}  
    _LeftTex ("Left_0 (+X)", 2D) = "white" {}  
    _RightTex ("Right_0 (-X)", 2D) = "white" {}  
    _UpTex ("Up_0 (+Y)", 2D) = "white" {}  
    _DownTex ("Down_0 (-Y)", 2D) = "white" {}
     
// ------------------ Day Textures --------------------//    
      
    _FrontTex2("Front_1 (+Z)", 2D) = "white" {}  
    _BackTex2("Back_1 (-Z)", 2D) = "white" {}  
    _LeftTex2("Left_1 (+X)", 2D) = "white" {}  
    _RightTex2("Right_1 (-X)", 2D) = "white" {}  
    _UpTex2("Up_1 (+Y)", 2D) = "white" {}  
    _DownTex2("Down_1 (-Y)", 2D) = "white" {}  
    
//// ------------------ Evening Textures --------------------//
//
//    _FrontTex3("Front_Evening (+Z)", 2D) = "white" {}  
//    _BackTex3("Back_Evening (-Z)", 2D) = "white" {}  
//    _LeftTex3("Left_Evening (+X)", 2D) = "white" {}  
//    _RightTex3("Right_Evening (-X)", 2D) = "white" {}  
//    _UpTex3("Up_Evening (+Y)", 2D) = "white" {}  
//    _DownTex3("Down_Evening (-Y)", 2D) = "white" {}  
// 
//// ------------------ Night Textures --------------------//  
//
//    _FrontTex4("Front_Night (+Z)", 2D) = "white" {}  
//    _BackTex4("Back_Night (-Z)", 2D) = "white" {}  
//    _LeftTex4("Left_Night (+X)", 2D) = "white" {}  
//    _RightTex4("Right_Night (-X)", 2D) = "white" {}  
//    _UpTex4("Up_Night (+Y)", 2D) = "white" {}  
//    _DownTex4("Down_Night (-Y)", 2D) = "white" {}    
}  
  
SubShader {  
    Tags { "Queue" = "Background" }  
    Cull Off  
    Fog { Mode Off }  
    Lighting Off  
    Color [_Tint] 
	    
	    //previous == last SetTexture result
	    //primary == color from the lighting calculation
	    //+- is simply adds the first part to a negative of the second { a+(-b) }
	    // I think combine just lerps them together, maybe. Makes more sense if it adds them though

	    Pass {  
	        SetTexture [_FrontTex] { combine texture }  
	        SetTexture [_FrontTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_FrontTex2] { combine previous + primary, previous * primary }  
	    }  
	    Pass {  
	        SetTexture [_BackTex] { combine texture }  
	        SetTexture [_BackTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_BackTex2] { combine previous + primary, previous * primary }  
	    }  
	    Pass {  
	        SetTexture [_LeftTex] { combine texture }  
	        SetTexture [_LeftTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_LeftTex2] { combine previous + primary, previous * primary }  
	    }  
	    Pass {  
	        SetTexture [_RightTex] { combine texture }  
	        SetTexture [_RightTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_RightTex2] { combine previous + primary, previous * primary }  
	    }  
	    Pass {  
	        SetTexture [_UpTex] { combine texture }  
	        SetTexture [_UpTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_UpTex2] { combine previous + primary, previous * primary }  
	    }  
	    Pass {  
	        SetTexture [_DownTex] { combine texture }  
	        SetTexture [_DownTex2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }  
	        SetTexture [_DownTex2] { combine previous + primary, previous * primary }  
	    }  
}  
  
Fallback "RenderFX/Skybox", 0  
}  
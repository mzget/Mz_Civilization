using UnityEngine;
using System.Collections;
 
// Use this on a guiText or guiTexture object to automatically have them
// adjust their aspect ratio when the game starts.
 
public class GuiRatioFixer : MonoBehaviour
{
	private const float gameWidth = 1024f;
	private const float gameHeight = 768f;
    //private float m_NativeRatio;
	float defaultPosX;
	float defaultPosY;
	float scaleX;
	float scaleY;
    
    void Start ()
    {
        //m_NativeRatio = gameWidth/gameHeight;
        //float currentRatio = (float)Screen.width / (float)Screen.height;
		
//        Vector3 scale = transform.localScale;
//        scale.x *= m_NativeRatio / currentRatio;
//        transform.localScale = scale;
		
		
		GUITexture myTexture = this.GetComponent<GUITexture>();
		
		if(Screen.height == gameHeight) {
			defaultPosY = myTexture.pixelInset.y;
			scaleY = myTexture.pixelInset.height;
		}
		else {
			defaultPosY = (Screen.height * myTexture.pixelInset.y) / gameHeight;
			scaleY = (Screen.height * myTexture.pixelInset.height) / gameHeight;		
            
            if(Screen.width == gameWidth) {
			    defaultPosX = myTexture.pixelInset.x;
			    scaleX = myTexture.pixelInset.width;
		    }
		    else {
//                defaultPosX = (Screen.width * myTexture.pixelInset.x) / gameWidth;
//                scaleX = (Screen.width * myTexture.pixelInset.width) / gameWidth;
				defaultPosX = (Screen.height * myTexture.pixelInset.x) / gameHeight;
				scaleX = (Screen.height * myTexture.pixelInset.width) / gameHeight;
		    }
		}
						
	    myTexture.pixelInset = new Rect(defaultPosX, defaultPosY, scaleX, scaleY);
    }
    
}

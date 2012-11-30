using UnityEngine;
using System.Collections;

public class Mz_ResizeScale {

	public static void ResizingScale(Transform r_transform)
    {
        float currentRatio = (float)Screen.width / (float)Screen.height;
        Vector3 newScale = r_transform.localScale;
		float fixedRatio = Main.FixedGameWidth/Main.FixedGameHeight;

        if (currentRatio == fixedRatio)
        {			
			Debug.Log("automatic scale by engine.");
        }
        else
        {
            Vector3 vec3 = new Vector3((currentRatio / fixedRatio), r_transform.localScale.y, r_transform.localScale.z);
            newScale.x = vec3.x;
        }

        r_transform.localScale = newScale;
	}
}

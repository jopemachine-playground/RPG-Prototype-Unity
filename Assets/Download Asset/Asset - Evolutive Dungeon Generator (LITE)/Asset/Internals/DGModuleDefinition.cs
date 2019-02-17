using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DGModuleDefinition {

	public int type       = (int)eDGModuleType.NONE;
	public int difficulty = 1;
	public int quantity = 1;
	
	public bool accessLeft   = false;
	public bool accessTop    = false;
	public bool accessRight  = false;
	public bool accessBottom = false;
	
	public int index = -1;
	public int rotation = 0;



	public void rotate() {

		bool left = accessLeft;

		accessLeft   = accessBottom;
		accessBottom = accessRight;
		accessRight  = accessTop;
		accessTop    = left;

		rotation += 90;
		if(rotation>360) rotation-=360;

	}
	
}

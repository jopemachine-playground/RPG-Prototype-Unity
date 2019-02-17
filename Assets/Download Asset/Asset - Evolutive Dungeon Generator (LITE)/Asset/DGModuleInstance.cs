using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DGModuleInstance : MonoBehaviour {

	public bool accessLeft = false;
	public bool accessTop = false;
	public bool accessRight = false;
	public bool accessBottom = false;

	public int difficulty = 1;

	[HideInInspector]
	public int rotation = 0;
}

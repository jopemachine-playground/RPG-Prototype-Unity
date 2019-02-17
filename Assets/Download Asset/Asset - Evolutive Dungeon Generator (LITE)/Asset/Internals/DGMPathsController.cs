using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DGM - DungeonGeneratorModule
public class DGMPathsController : MonoBehaviour {

	static private bool loaded = false;
	static private DGModuleInstance[] modules;
	
	
	
	static public bool isLoaded() {
		return loaded;
	}
	
	static public void load() {

		//LOAD
		Debug.LogWarning("Loading dungeon PATH modules...");
		modules = Resources.LoadAll<DGModuleInstance>("DGMPaths");
		Debug.Log(modules.Length + " modules loaded.");

		loaded=true;
	}

	static public DGModuleDefinition find(DGModuleDefinition search) {

		//LOAD
		if(!loaded) {
			load(); }


		//FIND
		List<DGModuleDefinition> modulesFound = new List<DGModuleDefinition>();
		for(int i = 0; i<modules.Length; i++) {

			DGModuleDefinition moduleDef = new DGModuleDefinition();
			moduleDef.index        = i;

			moduleDef.type         = search.type;
			moduleDef.difficulty   = search.difficulty;

			moduleDef.accessBottom = modules[i].accessBottom;
			moduleDef.accessLeft   = modules[i].accessLeft;
			moduleDef.accessRight  = modules[i].accessRight;
			moduleDef.accessTop    = modules[i].accessTop;

			moduleDef.rotation     = 0;
			
			for(int r=0; r<4; r++) {
				if(moduleDef.accessLeft   == search.accessLeft   &&
				   moduleDef.accessTop    == search.accessTop    &&
				   moduleDef.accessRight  == search.accessRight  &&
				   moduleDef.accessBottom == search.accessBottom
				){
					modulesFound.Add(moduleDef);
					break;
				} else {
					moduleDef.rotate();
				}
			}

		}

		//NOT FOUND
		if(modulesFound.Count < 1)
			return new DGModuleDefinition();

		//SORT ONE
		return modulesFound[Random.Range(0, modulesFound.Count)];
	}

	static public void place(int index, float x, float y, int rotation = 0) {

		//LOAD
		if(!loaded) {
			load(); }

		//CHECK
		if(index >= modules.Length || index<0) {
			Debug.LogError("Module index ("+index+") out of range!");
			return; }

		//INSTANTIATE
		DGModuleInstance module = Instantiate(modules[index]) as DGModuleInstance;
		module.transform.position = new Vector3(DungeonGenerator.moduleWidth*x, DungeonGenerator.moduleDepth, DungeonGenerator.moduleWidth*y*-1);
		//Parent
		if(DungeonGenerator.levelParent)
			module.transform.SetParent(DungeonGenerator.levelParent.transform);
		//Rotate
		module.GetComponent<DGModuleInstance>().rotation = rotation;
		if(rotation==90 || rotation==180 || rotation==270)
			module.transform.rotation = Quaternion.Euler(0, rotation, 0);


	}

}

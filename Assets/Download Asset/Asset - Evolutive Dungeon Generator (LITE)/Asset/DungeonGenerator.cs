using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

	//#######################################################
	//### ERROR ###
	//#######################################################
	static private bool   errorStatus = false;
	static private string errorMsg = "";

	static public void errorClear() {
		errorStatus = false;
		errorMsg = "";
	}

	static public bool error() {
		return errorStatus;
	}

	static public void errorSet(string msg) {
		errorMsg = msg+"\n";
	}

	static public string errorGet(string msg) {
		return errorMsg;
	}



	//#######################################################
	//### DUNGEON GENERATOR ###
	//#######################################################
	static public DGRangedValue rows            = new DGRangedValue(10, 100);
	static public DGRangedValue cols            = new DGRangedValue(10, 100);
	static public DGRangedValue difficulty      = new DGRangedValue(1, 4);
	static public DGRangedValue accesses        = new DGRangedValue(0, 2, 2);
	static public DGRangedValue pointsDensity   = new DGRangedValue(0, 10, 100);

	static private DGModuleDefinition[,] levelArray;
	
	static public List<Vector2> accessesList = new List<Vector2>();
	static public List<Vector2> pointsList   = new List<Vector2>();
	
	[HideInInspector]
	static public GameObject levelParent;

	static public float moduleWidth   = 5;
	static public float moduleDepth   = 0;
	
	static public int seed = 0;

	

	//### LEVEL ###
	static public DGModuleDefinition[,] getLevel() {
		return levelArray;
	}

	static public void setLevel(DGModuleDefinition[,] level) {
		DungeonGenerator.levelArray = level;
	}



    //### GERAL ###
    static public bool checkInputValues() {
		errorClear();

		if(cols.value < 1) {
			errorSet("The number of cols cannot be smaller than 1!"); }
		if(cols.check()) {
			errorMsg += cols.errorGet();
			errorStatus=true; }
		

		if(rows.value < 1) {
			errorSet("The number of rows cannot be smaller than 1!"); }
		if(rows.check()) {
			errorMsg += rows.errorGet();
			errorStatus=true; }


		levelAccessesCalculate();
		

		if(difficulty.value < 1) {
			errorSet("The difficulty cannot be smaller than 1!"); }
		if(difficulty.check()) {
			errorMsg += difficulty.errorGet();
			errorStatus=true; }


		return errorStatus;
	}

	static public int getLevelAvailableModulesQuantity() {
		int available = 0;

		for(int y = 0; y<rows.value; y++) {
			for(int x = 0; x<cols.value; x++) {
				if(levelArray[x,y].type == (int)eDGModuleType.NONE) {
					available++;
				}
			}
		}

		return available;
	}

	static public int getLevelModulesQuantity() {
		int quantity = 0;

		for(int y = 0; y<rows.value; y++) {
			for(int x = 0; x<cols.value; x++) {
				if(levelArray[x,y].type != (int)eDGModuleType.NONE) {
					quantity++;
				}
			}
		}

		return quantity;
	}

	static public void levelAccessesCalculate() {

		accesses.max = 0;

		if(rows.value > 0)
			accesses.max += cols.value;

		if(rows.value > 1)
			accesses.max += cols.value;

		if(rows.value > 2)
			accesses.max += ((rows.value-2) * 2);
		
		accesses.adjust();
	}
		
	static public void difficultyCalculate() {

		Debug.Log("Calculating difficulty map size...");

		if(checkInputValues()) {
			Debug.LogError("!!!ERROR!!! Please correct the following erros:\n"+errorMsg);
			return; }



		float difficultyDiff = Mathf.Max(1, difficulty.max - difficulty.min);
		float rowsInc = (rows.max - rows.min) / difficultyDiff;
		float colsInc = (cols.max - cols.min) / difficultyDiff;
		
		float difficultyTemp = difficulty.value - difficulty.min;

		rows.value = rows.min + (int)(difficultyTemp * rowsInc);
		cols.value = cols.min + (int)(difficultyTemp * colsInc);
		
		
		Debug.Log(
			"RESULT:\n"+
			"Difficulty: "+difficulty.value+"\n"+
			"Rows: "+rows.value+"\n"+
			"Cols: "+cols.value+"\n"
		);
	}
	
    static public Vector2 positionToWorld(Vector2 pos)
    {
        return new Vector2(DungeonGenerator.moduleWidth * pos.x, DungeonGenerator.moduleWidth * pos.y * -1);
    }



    //### DEBUG ###
    static public void debugLevelModules() {

		string output = "";

		for(int y = rows.value-1; y>=0; y--) {
			for(int x = 0; x<cols.value; x++) {

				if(levelArray[x, y].type == (int)eDGModuleType.ACCESS)
					output+="A";
				else if(levelArray[x, y].type == (int)eDGModuleType.POINT)
					output+="P";
				else
					output+="_";

			}
			output+="\n";
		}

		Debug.Log(output);
	}

	static public void debugLevelPaths() {

		string output = "";

		for(int y = rows.value-1; y>=0; y--) {
			
			//TOP
			for(int x = 0; x<cols.value; x++) {
				for(int x2 = 0; x2<3; x2++) {
					if(x2==1 && levelArray[x, y].accessBottom)
						output+="^";
					else
						output+="#";
				}
			}
			output+="\n";
			//MIDDLE
			for(int x = 0; x<cols.value; x++) {
				for(int x2 = 0; x2<3; x2++) {
					if(x2==1)
						if(levelArray[x, y].type == (int)eDGModuleType.ACCESS)
							output+="A";
						else if(levelArray[x, y].type == (int)eDGModuleType.POINT)
							output+="P";
						else
							output+="#";
					else
						if(x2==0 && levelArray[x, y].accessLeft)
						output+="<";
					else if(x2==2 && levelArray[x, y].accessRight)
						output+=">";
					else
						output+="#";
				}
			}
			output+="\n";
			//BOTTOM
			for(int x = 0; x<cols.value; x++) {
				for(int x2 = 0; x2<3; x2++) {
					if(x2==1 && levelArray[x, y].accessTop)
						output+="v";
					else
						output+="#";
				}
			}
			output+="\n";
		}

		Debug.Log(output);
	}

	static public void debugLevelWalkable() {

		string output = "";

		for(int y = rows.value-1; y>=0; y--) {
			for(int x = 0; x<cols.value; x++) {

				if(levelArray[x, y].type == (int)eDGModuleType.NONE)
					output+="_";
				else
					output+="+";

			}
			output+="\n";
		}

		Debug.Log(output);

	}
	


	//### CREATE ###
	static private void createParent() {

		//PARENT
		levelParent = GameObject.Find("Dungeon");
		
		if(levelParent!=null)
			GameObject.DestroyImmediate(levelParent);
		
		levelParent = new GameObject("Dungeon");

	}

	static private void createArrays() {
		Debug.LogWarning("Creating Arrays ...");
		if(errorStatus) return;

		//LEVEL ARRAY
		levelArray = new DGModuleDefinition[cols.value, rows.value];
		for(int y = 0; y<rows.value; y++) {
			for(int x = 0; x<cols.value; x++) {
				levelArray[x, y] = new DGModuleDefinition();
			}
		}
		
		//LISTS
		accessesList.Clear();
		pointsList.Clear();
		
		//debugLevelModules();
	}
	
	static private void createAccesses() {
		Debug.LogWarning("Creating accesses...");
		if(errorStatus) return;

		string output = "";
		for(int i = 0; i<accesses.value; i++) {
			bool created = false;
			while(!created) {

				int x = 0;
				int y = 0;


				//Access on the X axis (cols)
				if(DGRandom.Range(0, 2) == 0) {
					x = (DGRandom.Range(0, 2) == 0) ? 0 : cols.value-1;
					y = DGRandom.Range(0, rows.value);
				}
				//Access on the Y axis (rows)
				else {
					x = DGRandom.Range(0, cols.value);
					y = (DGRandom.Range(0, 2) == 0) ? 0 : rows.value-1;
				}


				//Check Availability
				if(levelArray[x, y].type == (int)eDGModuleType.NONE) {
					//CREATE
					created=true;
					output+="Access #"+i+"   x:"+x+"   y:"+y+"\n";

					levelArray[x, y].type = (int)eDGModuleType.ACCESS;

					accessesList.Add(new Vector2(x, y));
				}

			}
		}

		Debug.Log(output);
		debugLevelModules();
	}

	static private void createPoints() {
		Debug.LogWarning("Creating points...");
		if(errorStatus)
			return;

		string output = "";


		//Quantity
		int quantity = Mathf.RoundToInt(((float)(cols.value*rows.value)) * (((float)pointsDensity.value)/1000.0f));
		if(quantity < 1 && pointsDensity.min > 0)
			quantity = 1;
		
		int quantityAvailable = getLevelAvailableModulesQuantity();
		if(quantity > quantityAvailable)
			quantity = quantityAvailable;
		
		output+="Quantity: "+quantity;
		
		

		//Create
		for(int i = 0; i<quantity; i++) {
			bool created = false;
			while(!created) {

				//Sort point
				int x = Random.Range(0, cols.value);
				int y = Random.Range(0, rows.value);

				//Check Avaliable
				if(levelArray[x, y].type == (int)eDGModuleType.NONE) {
					//CREATE
					created=true;
					output+=("#"+i+"   x:"+x+"   y:"+y+"\n");

					levelArray[x, y].type = (int)eDGModuleType.POINT;
					levelArray[x, y].difficulty = difficulty.value;

					pointsList.Add(new Vector2(x, y));
				}

			}
		}
		Debug.Log(output);

		debugLevelModules();
	}

	static private void createConnectedPoints(Vector2 origin, Vector2 destiny, ref string output) {

		output += ("Connecting points: ("+origin.x+", "+origin.y+")   ("+destiny.x+", "+destiny.y+")\n");

		Vector2 distance;
		while(true) {

			distance = new Vector2(origin.x - destiny.x, origin.y - destiny.y);
			if(distance.x<0) distance.x *= -1;
			if(distance.y<0) distance.y *= -1;
			
			//### MOVE ###
			if(distance.x > distance.y){
				if(origin.x-1 >= 0   &&   origin.x-1 >= destiny.x)
					origin.x--;
				else if(origin.x+1 < cols.value   &&   origin.x+1 <= destiny.x)
					origin.x++;
				else if(origin.y-1 >= 0   &&   origin.y-1 >= destiny.y)
					origin.y--;
				else if(origin.y+1 < rows.value   &&   origin.y+1 <= destiny.y)
					origin.y++;
			}else{
				if(origin.y-1 >= 0   &&   origin.y-1 >= destiny.y)
					origin.y--;
				else if(origin.y+1 < rows.value   &&   origin.y+1 <= destiny.y)
					origin.y++;
				else if(origin.x-1 >= 0   &&   origin.x-1 >= destiny.x)
					origin.x--;
				else if(origin.x+1 < cols.value   &&   origin.x+1 <= destiny.x)
					origin.x++;
			}
			
			//### CONNECT ###
			if(levelArray[(int)origin.x, (int)origin.y].type == (int)eDGModuleType.NONE) {
				output+="Creating path: ("+origin.x+", "+origin.y+")\n";
				levelArray[(int)origin.x, (int)origin.y].type = (int)eDGModuleType.PATH;
			}
			
			//### REACHED ###
			if(origin.x == destiny.x && origin.y == destiny.y) {
				break; }
		}

	}
	
	static private void createPaths() {
		Debug.LogWarning("Creating Paths...");
		if(errorStatus)
			return;


		//Get Points
		List<Vector2> levelPoints   = new List<Vector2>();
		for(int y = 0; y<rows.value; y++) {
			for(int x = 0; x<cols.value; x++) {
				if(levelArray[x, y].type != (int)eDGModuleType.NONE) {
					levelPoints.Add(new Vector2(x, y));
				}
			}
		}


		//Shuffle Points
		List<Vector2> levelPointsSorted = new List<Vector2>();
		for(int i=0; i<levelPoints.Count; i++) {
			while(true) {
				Vector2 point = levelPoints[DGRandom.Range(0, levelPoints.Count)];
				if(levelPointsSorted.IndexOf(point)==-1) {
					levelPointsSorted.Add(point);
					break;
				}
			}
		}
		

		//Connect points
		string output = "";
		for(int i = 1; i<levelPointsSorted.Count; i++) {
			createConnectedPoints(levelPointsSorted[i], levelPointsSorted[i-1], ref output);
			output += "\n";
		}
		Debug.Log(output);


		Debug.Log("All Paths Created!");
		debugLevelModules();
	}

	static private void createPathsInModules() {
		Debug.Log("Creating Paths in modules...\n");
		if(errorStatus)
			return;

		//CREATE PATHS BETWEEN MODULES
		for(int x = 0; x<cols.value; x++) {
			for(int y = 0; y<rows.value; y++) {

				if(levelArray[x,y].type == (int)eDGModuleType.NONE)
					continue;


				//LEFT
				if(x-1 >= 0) {
					if(levelArray[x-1, y].type != (int)eDGModuleType.NONE)
						levelArray[x, y].accessLeft = true;
				}

				//RIGHT
				if(x+1 < cols.value) {
					if(levelArray[x+1, y].type != (int)eDGModuleType.NONE)
						levelArray[x, y].accessRight = true;
				}

				//TOP
				if(y-1 >= 0) {
					if(levelArray[x, y-1].type != (int)eDGModuleType.NONE)
						levelArray[x, y].accessTop = true;
				}

				//BOTTOM
				if(y+1 < rows.value) {
					if(levelArray[x, y+1].type != (int)eDGModuleType.NONE)
						levelArray[x, y].accessBottom = true;
				}

			}
		}

		//CREATE ACCESSES CONNECTIONS
		for(int x = 0; x<cols.value; x++) {
			for(int y = 0; y<rows.value; y++) {

				if(levelArray[x,y].type != (int)eDGModuleType.ACCESS)
					continue;

				//LEFT
				if(x-1 < 0) {
					levelArray[x, y].accessLeft = true; }

				//RIGHT
				else if(x+1 >= cols.value) {
					levelArray[x, y].accessRight = true; }

				//TOP
				else if(y-1 < 0) {
					levelArray[x, y].accessTop = true; }

				//BOTTOM
				if(y+1 >= rows.value) {
					levelArray[x, y].accessBottom = true;
				}
			}
		}

		debugLevelPaths();
	}

	static private void createModulesFill() {
		Debug.LogWarning("Searching for modules that match the level array...");
		if(errorStatus)
			return;

		//MODULES
		for(int x = 0; x<cols.value; x++) {
			for(int y = 0; y<rows.value; y++) {
				if(levelArray[x, y].type != (int)eDGModuleType.NONE) {

					DGModuleDefinition module;

					//SEARCH
					module = DGMPathsController.find(levelArray[x, y]);
					
					//RESULT
					if(module.type == (int)eDGModuleType.NONE) {
						Debug.LogError("A module cannot be found for a specific combination!\nPosition x: "+x+" y:"+y); }
					else
						levelArray[x, y] = module;
				}
			}
		}
		
		Debug.Log("Level filled!");
	}

	//### RENDER ###
	static private void renderLevel() {

		//LOAD
		DGMPathsController.load();
		
		//MODULES (PATHS, ACCESSES, POINTS)
		for(int x = 0; x<cols.value; x++) {
			for(int y = 0; y<rows.value; y++) {

				if(levelArray[x,y].type==(int)eDGModuleType.PATH || levelArray[x,y].type==(int)eDGModuleType.ACCESS || levelArray[x,y].type==(int)eDGModuleType.POINT)
					DGMPathsController.place(levelArray[x,y].index, x, y, levelArray[x,y].rotation);
				
			}
		}
		
	}
	
	static public void create() {
		Debug.Log("Create Dungeon Started!");
		if(checkInputValues()){
			Debug.LogError(errorMsg);
			Debug.LogError("Correct the errors before create the dungeon.");
			return; }
		

		//Random Seed
		DGRandom.setSeed(seed);

		//Dungeon
		createParent();
		createArrays();

		createAccesses();
		createPoints();
		
		createPaths();
		createPathsInModules();
		createModulesFill();
		
		//Render
		renderLevel();

		Debug.Log("Create Dungeon Complete!");
	}

}

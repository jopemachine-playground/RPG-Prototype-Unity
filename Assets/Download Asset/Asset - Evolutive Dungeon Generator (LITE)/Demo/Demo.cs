using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

	void Start () {

        //DGMPathsController.load();

        //for(int x=0; x<3; x++) {
        //	for(int y=0; y<3; y++) {
        //		if(x==1 && y==1)
        //			DGMPathsController.place(0, x, y, 90);
        //		else
        //			DGMPathsController.place(0, x, y);
        //	}
        //}




        //int first = DGRandom.Value();

        //DGRandom.setSeed(1);
        //for(int i = 0; i<50; i++) {
        //	//Debug.Log(DGRandom.Value());
        //	DGRandom.Range(0, 2);
        //	Debug.Log(DGRandom.Range(0, 10));
        //	//Debug.Log(DGRandom.Range(0, 3));
        //	//if(first==DGRandom.Value()) {
        //	//	Debug.Log(i);
        //	//}
        //}

        //### DEFINITION ###
		DungeonGenerator.seed = (int)Time.time;

		DungeonGenerator.cols      .setRange(85, 30);
		DungeonGenerator.rows      .setRange(75, 40);
		DungeonGenerator.difficulty.setRange( 4,  1);
		DungeonGenerator.difficultyCalculate();//change

		DungeonGenerator.accesses.setValue(2);
		DungeonGenerator.moduleDepth = 1.56f;


		//### CREATION ###
		DungeonGenerator.create();



        //### POSITIONING ###
        Vector2 playerStart = DungeonGenerator.accessesList[0];
        Vector2 playerEnd   = DungeonGenerator.accessesList[1];
		Debug.Log("Player Start in array: "+playerStart+" "+playerEnd);
        
        playerStart = DungeonGenerator.positionToWorld(playerStart);
        playerEnd   = DungeonGenerator.positionToWorld(playerEnd);
        Debug.Log("Player Start in world: "+playerStart+" "+playerEnd);

		
    }
	


}

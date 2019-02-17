using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

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

        //CREATION
        DungeonGenerator.accesses.setValue(2);

        DungeonGenerator.cols.setRange(85, 30);
        DungeonGenerator.rows.setRange(75, 40);

        DungeonGenerator.difficulty.setRange(4, 1);

        DungeonGenerator.difficultyCalculate();

        //DungeonGenerator.monstersDensity.setValue(100);

        DungeonGenerator.seed = (int)Time.time;

        DungeonGenerator.moduleDepth = 1.56f;

        DungeonGenerator.create();

        //POSITIONING
        Vector2 playerStart = DungeonGenerator.accessesList[0];
        Vector2 playerEnd   = DungeonGenerator.accessesList[1];
        Debug.Log(playerStart);
        Debug.Log(playerEnd);

        playerStart = DungeonGenerator.positionToWorld(playerStart);
        playerEnd   = DungeonGenerator.positionToWorld(playerEnd);

        Debug.Log(playerStart);
        Debug.Log(playerEnd);


        //PLAYER POS
        GameObject player = GameObject.Find("_Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player.transform.position = new Vector3(playerStart.x, 0, playerStart.y);

        //EXIT POS
        GameObject portalExit = GameObject.Find("_DungeonPortalExit");
        if (portalExit == null)
        {
            Debug.LogError("Exit Portal not found!");
            return;
        }

        portalExit.transform.position = new Vector3(playerEnd.x, 0, playerEnd.y);


        //DungeonModuleController.load(); //this showld be inside of the DungeonGenerator::generate();

        //DungeonGenerator::start();

        ////DungeonGenerator dungeonGenerator = new DungeonGenerator();
        //DungeonGenerator dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        //dungeonGenerator.difficulty.value = 12;
        //dungeonGenerator.difficultyCalculate();
        //dungeonGenerator.objectivesDensity.value = 3;
        //dungeonGenerator.create();

        //DungeonModuleController.setLevel(dungeonGenerator.getLevel(), dungeonGenerator.cols.value, dungeonGenerator.rows.value);

        //DungeonModuleController.render();


        //GameObject player = GameObject.Find("_Player");
        //GameObject start  = GameObject.Find("009(Clone)");

        //player.transform.position = new Vector3(start.transform.position.x, start.transform.position.y+1, start.transform.position.z);
        //      //player.transform.localScale *= 5;

        //      //player.GetComponent<Player>().speedMax = 800;
        //      //player.GetComponent<Player>().speedAccel = 80;
        //      //player.GetComponent<Player>().speedRun = 300;
        //      //player.GetComponent<Player>().speedWalk = 600;
        //      //player.GetComponent<Player>().speedSneak = 2;
        //      //player.GetComponent<Player>().stopSpeed = 20000;



    }
	
	void Update () {
		
	}
}

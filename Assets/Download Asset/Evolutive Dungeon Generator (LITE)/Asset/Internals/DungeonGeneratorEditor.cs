#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
[CanEditMultipleObjects]
public class DungeonGeneratorEditor : Editor {

	SerializedProperty cols;
	SerializedProperty rows;
	SerializedProperty difficulty;

	public override void OnInspectorGUI()
	{
		GUIStyle styleBox = new GUIStyle();
		styleBox.font = EditorStyles.boldFont;
		styleBox.padding = new RectOffset(10, 10, 0, 10);
		styleBox.border  = new RectOffset(1, 1, 1, 1);

		GUIStyle styleBold = new GUIStyle();
		styleBold.font = EditorStyles.boldFont;
		
		//DrawDefaultInspector();
		EditorGUILayout.Space();
		

		//### COLS ###
		EditorGUILayout.LabelField("Cols", styleBold);
		GUILayout.BeginVertical(styleBox);
			DungeonGenerator.cols.min = EditorGUILayout.IntSlider(new GUIContent("Min"), DungeonGenerator.cols.min, 0, 100);
			DungeonGenerator.cols.max = EditorGUILayout.IntSlider(new GUIContent("Max"), DungeonGenerator.cols.max, DungeonGenerator.cols.min, 100);
			EditorGUILayout.LabelField("Current", DungeonGenerator.cols.value.ToString(), styleBold);
		GUILayout.EndVertical();
		DungeonGenerator.cols.adjust();
		
		//### ROWS ###
		EditorGUILayout.LabelField("Rows", styleBold);
		GUILayout.BeginVertical(styleBox);
			DungeonGenerator.rows.min = EditorGUILayout.IntSlider(new GUIContent("Min"), DungeonGenerator.rows.min, 0, 100);
			DungeonGenerator.rows.max = EditorGUILayout.IntSlider(new GUIContent("Max"), DungeonGenerator.rows.max, DungeonGenerator.rows.min, 100);
			EditorGUILayout.LabelField("Current", DungeonGenerator.rows.value.ToString(), styleBold);
		GUILayout.EndVertical();
		DungeonGenerator.rows.adjust();
		
		//### DIFFICULTY ###
		EditorGUILayout.LabelField("Difficulty", styleBold);
		GUILayout.BeginVertical(styleBox);
			DungeonGenerator.difficulty.min = EditorGUILayout.IntSlider(new GUIContent("Min"), DungeonGenerator.difficulty.min, 1, 100);
			DungeonGenerator.difficulty.max = EditorGUILayout.IntSlider(new GUIContent("Max"), DungeonGenerator.difficulty.max, DungeonGenerator.difficulty.min, 100);
			EditorGUILayout.LabelField("Current", DungeonGenerator.difficulty.value.ToString(), styleBold);
			DungeonGenerator.difficulty.value = EditorGUILayout.IntSlider(DungeonGenerator.difficulty.value, DungeonGenerator.difficulty.min, DungeonGenerator.difficulty.max);
		GUILayout.EndVertical();
		DungeonGenerator.difficulty.adjust();
		DungeonGenerator.difficultyCalculate();
		
		//### GENERAL ###
		EditorGUILayout.LabelField("General", styleBold);
		GUILayout.BeginVertical(styleBox);
			DungeonGenerator.accesses.value = EditorGUILayout.IntSlider(new GUIContent("Accesses"), DungeonGenerator.accesses.value, DungeonGenerator.accesses.min, DungeonGenerator.accesses.max);
			DungeonGenerator.pointsDensity.value = EditorGUILayout.IntSlider(new GUIContent("Points Density"), DungeonGenerator.pointsDensity.value, DungeonGenerator.pointsDensity.min, DungeonGenerator.pointsDensity.max);
			DungeonGenerator.seed = EditorGUILayout.IntField(new GUIContent("Seed"), DungeonGenerator.seed);
		GUILayout.EndVertical();
		DungeonGenerator.accesses.adjust();
		DungeonGenerator.levelAccessesCalculate();
		
		//### CREATE ###
		if (GUILayout.Button("\nCreate Dungeon\n"))
			DungeonGenerator.create();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

	}

}



#endif
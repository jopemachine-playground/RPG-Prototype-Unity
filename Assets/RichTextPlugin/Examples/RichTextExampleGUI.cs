using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RichTextPlugin;
using UnityEngine.UI;

[RequireComponent( typeof(Text) )]
public class RichTextExampleGUI : MonoBehaviour {

	Text myGUItext{ 	get{ return GetComponent<Text>(); }	   }

	public RichText myInspectorRichTextObject;

	void Start () {

		myGUItext.supportRichText = true;  // < just to be sure.

		myGUItext.text = myInspectorRichTextObject;

		// Besides, you can create and manage RichText Objects at runtime.

		RichText myScriptedRichtext =  new RichText("another RichText created by script", Color.yellow, false, false);

		// Opacity Are also compatible
		myGUItext.text += System.Environment.NewLine; // add new line to the gui text

		// You Can set the alpha value throug color's alpha value...
		myInspectorRichTextObject.color = new Color(1,0,0,0.6f);
		myGUItext.text += myScriptedRichtext;

		myGUItext.text += System.Environment.NewLine;

		//Or, you can set it using richText object alpha propiety.
		myScriptedRichtext.alpha = 0.6f;
		myGUItext.text += myScriptedRichtext;

		myGUItext.text += System.Environment.NewLine;

		myScriptedRichtext.alpha = 0.3f;
		myGUItext.text += myScriptedRichtext;

		//Gardient example

		RichText myGardient = new RichText("My Gradient Example", Color.red, Color.yellow, true, false);
		myGUItext.text += System.Environment.NewLine;
		myGUItext.text += myGardient;

		// Creating Baked-RichText without creating object instance
		myGUItext.text += System.Environment.NewLine;
		myGUItext.text += RichText.Paint("Baked RichText", Color.cyan, true, false); 
	}
	
	
}

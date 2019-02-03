using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RichTextPlugin;  // <--- 1. add the namespace

public class RicxhTextExampleConsole : MonoBehaviour {

	void Start () {

		// 2. Create new richtext object       Text           Color     bold?  italic? 
		RichText myRichText = new RichText("Hello world",  Color.blue,  true,  false );

		// 3. print the richtext object in the console.
		print( myRichText );

		//4. you can change richText settings anytime.

		myRichText.bold = false;
		myRichText.italic = true;
		myRichText.color = Color.red;

		//5. print the new result.
		print(myRichText);

		// Opacity are also compatible.
		myRichText.alpha = 0.5f;
		myRichText.text += " alpha 0.5";
		print(myRichText);

		//7. now attach this to a GameObject in the scene, then click play and see the console.
	}
	

}

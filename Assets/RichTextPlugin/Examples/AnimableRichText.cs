using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RichTextPlugin;
using UnityEngine.UI;

[RequireComponent( typeof(Text)	    ) ]
[RequireComponent( typeof(Animator)	) ]

public class AnimableRichText : MonoBehaviour {
	// How to create richText GUI animation-

	//Create 2 public richText field
	RichText animatedRichText1;
	RichText animatedRichText2;

	// Create script animation handler for RichText objects
	public bool bold1;
	public bool italic1;
	public bool bold2;
	public bool italic2;

	[SerializeField]
	public Color richTextColor1;
	public Color richTextColor2;


	void Start () {
		animatedRichText1 = new RichText("", richTextColor1,bold1, italic1);
		animatedRichText2 = new RichText("", richTextColor1,bold2, italic2);
	}

	//3. Use Update loop to synchronize the GUI text with the animated richText object
	void Update () {
		animatedRichText1.color = richTextColor1;
		animatedRichText1.bold = bold1;
		animatedRichText1.italic = italic1;

		animatedRichText2.color = richTextColor2;
		animatedRichText2.bold = bold2;
		animatedRichText2.italic = italic2;
		GetComponent<Text>().text = animatedRichText1 + animatedRichText2;
	}

	//4. You can't animate string direclty through animation keys. but you can do it through animation event.
	//   so, we gonna create an event for animation system to animate the text.
	public void AnimateString1(string text){
		animatedRichText1.text = text;
	}

	public void AnimateString2(string text){
		animatedRichText2.text = text;
	}
}

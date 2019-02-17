using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour {


	public Transform MainTarget;


	void Update(){


	}

	void LateUpdate () {

		transform.position = new Vector3 (MainTarget.position.x,transform.position.y,MainTarget.position.z);
		transform.eulerAngles = new Vector3( transform.eulerAngles.x, MainTarget.eulerAngles.y, transform.eulerAngles.z );

	}
}

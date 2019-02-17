using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float movementSpeed = 10;
	public float turningSpeed = 60;

	void Update() {
		float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
		transform.Rotate(0, horizontal, 0);

		float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);
	}
}
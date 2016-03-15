using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed; //How fast we should move, in units/second

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}

	Vector3 initialPos; //The initial position we are in when MoveToLocation is called
	Vector3 moveTarget; //Where we want to move to
	float lerpPecentage; //How far we are along said movement bounded from [0, 1]
	bool isMoving; //Are we moving right now?

	public void MoveToLocation(Vector3 location){ //Moves player to given location at set speed
		initialPos = transform.position; //Set the inital pos
		moveTarget = location; //Set where we wanna go
		lerpPecentage = 0; //We just started movement, so we're 0% of the way there
		isMoving = true; //And now, we're ready to move!
	}

	void HandleMovement(){ //Handles movement from initalPos to moveTarget
		if(isMoving){ //If we're moving
			// % Journey complete = 1 - ((Distance from our pos to target - speed * time elapsed this frame) / distance from initial pos to target);
			lerpPecentage = 1 - (Vector3.Distance(transform.position, moveTarget) - (speed * Time.deltaTime)) / Vector3.Distance(initialPos, moveTarget);
			transform.position = Vector3.Lerp(initialPos, moveTarget, lerpPecentage); //Set our position dependent on lerp
			if(Vector3.Distance(transform.position, moveTarget) <= 0.01f){ //If we're close enough to our target position
				transform.position = moveTarget; //clip position to that target
				isMoving = false; //We're done moving
			}
		}
	}
}

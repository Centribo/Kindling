using UnityEngine;
using System.Collections;

public class RoomDoor : MonoBehaviour {

	public enum States {Open, Closed, Opening, Closing}
	
	public States state;
	public Vector3 openPositionRelative;
	public float speed;

	public AudioClip openingSound;
	public AudioClip openedSound;
	public AudioClip closingSound;
	public AudioClip closedSound;

	Vector3 closedPosition;
	CardboardAudioSource cas;

	void Awake(){
		cas = GetComponent<CardboardAudioSource>();
	}

	// Use this for initialization
	void Start () {
		closedPosition = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}

	void HandleMovement(){
		float lerpPercentage;

		switch(state){
			case States.Open:
				transform.position = closedPosition + openPositionRelative;
			break;
			
			case States.Closed:
				transform.position = closedPosition;
			break;
			
			case States.Opening:
				// % Journey complete = 1 - ((Distance from our pos to target - speed * time elapsed this frame) / distance from initial pos to target);
				lerpPercentage = 1 - (Vector3.Distance(transform.position, closedPosition + openPositionRelative) - speed * Time.deltaTime) / Vector3.Distance(closedPosition, closedPosition + openPositionRelative);
				transform.position = Vector3.Lerp(closedPosition, closedPosition + openPositionRelative, lerpPercentage); //Set our position dependent on lerp
				if(lerpPercentage >= 0.999f){
					cas.loop = false;
					cas.clip = openedSound;
					cas.Play();
					state = States.Open;
				}
			break;

			case States.Closing:
				// % Journey complete = 1 - ((Distance from our pos to target - speed * time elapsed this frame) / distance from initial pos to target);
				lerpPercentage = 1 - (Vector3.Distance(transform.position, closedPosition) - speed * Time.deltaTime) / Vector3.Distance(closedPosition, closedPosition + openPositionRelative);
				transform.position = Vector3.Lerp(closedPosition + openPositionRelative, closedPosition, lerpPercentage); //Set our position dependent on lerp
				if(lerpPercentage >= 0.999f){
					cas.loop = false;
					cas.clip = closedSound;
					cas.Play();
					state = States.Closed;
				}
			break;
		}
	}

	public void OpenDoor(){
		state = States.Opening;
		cas.loop = true;
		cas.clip = openingSound;
		cas.Play();
	}

	public void CloseDoor(){
		state = States.Closing;
		cas.loop = true;
		cas.clip = closingSound;
		cas.Play();
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		if(state == States.Closed){
			Gizmos.DrawLine(transform.position + openPositionRelative, transform.position);	
		} else if(state == States.Open){
			Gizmos.DrawLine(transform.position - openPositionRelative, transform.position);	
		} else {
			Gizmos.DrawLine(closedPosition, closedPosition + openPositionRelative);
		}
		
	}
}

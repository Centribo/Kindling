using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance = null;
	public static PlayerController Instance { //Singleton pattern instance
		get { //Getter
			if(instance == null){ //If its null,
				instance = (PlayerController)FindObjectOfType(typeof(PlayerController)); //Find it
			}
			return instance; //Return it
		}
	}

	//Public variables:
		//Script references, to be linked in Unity Editor
	public CardboardHead cardboardHead;
	public PlayerUIController uiController;

		//Controllable variables:
	public float height; //The height of the player
	public float speed; //How fast we should move, in units/second
	public AudioClip walkingSound; //The sound to make when we're walking

	//Private variables:
		//References to components:
	CardboardAudioSource cas;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject); //Don't destroy us on loading new scenes
		
		cas = GetComponent<CardboardAudioSource>();
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
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

		//Audio:
		cas.clip = walkingSound; //set the walking clip
		cas.Play(); //Start making walking noises

		isMoving = true; //And now, we're ready to move!
	}

	bool isMovingToInteract; //If we are moving to an object that we want to interact with
	InteractableObject interactingObject; //What object we're trying to interact with

	public void InteractWithObject(InteractableObject io){ //Moves player to given object's interact location, then interacts with it
		isMovingToInteract = true; //We're not moving towards the given object
		interactingObject = io; //Set the object we're going to interact with
		MoveToLocation(io.transform.position + io.interactLocation + new Vector3(0, height, 0)); //Move to that object
	}

	void HandleMovement(){ //Handles movement from initalPos to moveTarget
		if(isMoving){ //If we're moving
			// % Journey complete = 1 - ((Distance from our pos to target - speed * time elapsed this frame) / distance from initial pos to target);
			lerpPecentage = 1 - (Vector3.Distance(transform.position, moveTarget) - (speed * Time.deltaTime)) / Vector3.Distance(initialPos, moveTarget);
			transform.position = Vector3.Lerp(initialPos, moveTarget, lerpPecentage); //Set our position dependent on lerp
			if(Vector3.Distance(transform.position, moveTarget) <= 0.01f){ //If we're close enough to our target position
				transform.position = moveTarget; //clip position to that target
				cas.Stop(); //Stop making walking noises
				isMoving = false; //We're done moving
				if(isMovingToInteract){ //If we are moving to an object that we want to interact with aswell
					isMovingToInteract = false;
					interactingObject.Interact(); //Interact with it
					interactingObject = null;
				}
			}
		}
	}

	void HandleInput(){
		if(Cardboard.SDK.Triggered){ //If the player clicked the button this frame
			RaycastHit targetHit;
			//Check the raycast what they are looking at, and see if we can move there, if we can:
			if(Physics.Raycast(cardboardHead.Gaze.origin, cardboardHead.Gaze.direction, out targetHit) && (targetHit.transform.tag == "Floor" || targetHit.transform.tag == "Ground")){
				MoveToLocation(targetHit.point + new Vector3(0, height, 0)); //Move there
			}
			if(isMovingToInteract){
				isMovingToInteract = false;
			}
		}
	}
}

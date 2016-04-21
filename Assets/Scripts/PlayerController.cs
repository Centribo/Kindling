using UnityEngine;
using UnityEngine.UI;
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
	public ReticleController reticleController;
	public Image fadingImage;
	public CardboardAudioListener cal;

		//Controllable variables:
	public float fadeRate; //How fast we should fade transitions
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
		Cardboard.SDK.Recenter();
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
		HandleInspection();
		HandleMovement();
		HandleFade();
	}

	Vector3 initialPos; //The initial position we are in when MoveToLocationRaw is called
	Vector3 moveTarget; //Where we want to move to
	float lerpPercentage; //How far we are along said movement bounded from [0, 1]
	bool isMoving; //Are we moving right now?

	public void MoveToLocationRaw(Vector3 location){ //Moves player to given location at set speed
		initialPos = transform.position; //Set the inital pos
		moveTarget = location; //Set where we wanna go
		lerpPercentage = 0; //We just started movement, so we're 0% of the way there

		//Audio:
		cas.clip = walkingSound; //set the walking clip
		cas.Play(); //Start making walking noises

		isMoving = true; //And now, we're ready to move!
	}

	public void MoveToLocation(Vector3 location){
		MoveToLocationRaw(location + new Vector3(0, height, 0));
	}

	bool isMovingToInteract; //If we are moving to an object that we want to interact with
	InteractableObject interactingObject; //What object we're trying to interact with

	public void InteractWithObject(InteractableObject io){ //Moves player to given object's interact location, then interacts with it
		if(!isInspecting){
			isMovingToInteract = true; //We're not moving towards the given object
			interactingObject = io; //Set the object we're going to interact with
			MoveToLocationRaw(io.transform.position + io.interactLocation + new Vector3(0, height, 0)); //Move to that object
		} else {
			io.Interact();
		}
	}

	bool isInspecting = false; //Are we inspecting something right now?
	Vector3 initialInspectOrientation; //Holds the orientation of the player's head once they start inspecting an item
	InspectableObject inspectingObj; //What object we're inspecting

	public void InspectObject(InspectableObject obj){
		isMoving = false; //Stop movement
		isMovingToInteract = false;
		
		//Move the object infront of our face
		obj.transform.SetParent(cardboardHead.transform);
		obj.transform.localPosition = new Vector3(0, 0, 1);
		initialInspectOrientation = Cardboard.SDK.HeadPose.Orientation.eulerAngles; //Set our initial orientation
		inspectingObj = obj; //Set the object we're inspecting
		isInspecting = true; //And now we're inspecting!
	}

	public void StopInspecting(){
		inspectingObj = null;
		isInspecting = false;
	}

	public void SetFadeColour(Color c){
		fadingImage.color = c;
	}

	int fadeState = 0; //0 = Not fading, -1 = fading out, 1 = fading in
	
	public void FadeIn(float delay){
		Invoke("FadeIn", delay);
	}

	public void FadeOut(float delay){
		Invoke("FadeOut", delay);
	}

	public void FadeIn(){
		fadeState = 1;
	}

	public void FadeOut(){
		fadeState = -1;
	}

	void HandleFade(){
		Color originalColor = fadingImage.color; //Get our current color
		float audioGain;
		float fadeTolerance = 0.01f; //When to just snap to fully transparent or opaque
		if(fadeState == 1){ //If we're fading in
			originalColor.a = 0; //Set our target color to be transparent
			audioGain = 0; //Set our target audio level
			cal.globalGainDb = Mathf.Lerp(cal.globalGainDb, audioGain, fadeRate * Time.deltaTime); //LERP!
			fadingImage.color = Color.Lerp(fadingImage.color, originalColor, fadeRate * Time.deltaTime); 
			if(fadingImage.color.a <= fadeTolerance){ //If we're within tolerance
				cal.globalGainDb = 0; //Set it to be normal
				fadingImage.color = originalColor; //Set it to be fully transparent
				fadeState = 0; //We're not longer fading
			}
		} else if (fadeState == -1){ //Similar to above
			originalColor.a = 1;
			audioGain = -24.0f;
			cal.globalGainDb = Mathf.Lerp(cal.globalGainDb, audioGain, fadeRate * Time.deltaTime);
			fadingImage.color = Color.Lerp(fadingImage.color, originalColor, fadeRate * Time.deltaTime);
			if(fadingImage.color.a >= 1 - fadeTolerance){
				cal.globalGainDb = -24.0f;
				fadingImage.color = originalColor;
				fadeState = 0;
			}
		}
		
	}

	void HandleInspection(){
		if(isInspecting && inspectingObj != null){ //If we're inspecting an object, and that object isn't null
			Vector3 orientationDelta = initialInspectOrientation - Cardboard.SDK.HeadPose.Orientation.eulerAngles;

			float padding = 20;
			float rotateSpeed = 50;

			if(orientationDelta.x < -padding){
				inspectingObj.transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
			} else if (orientationDelta.x > padding){
				inspectingObj.transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
			}

			if(orientationDelta.y < -padding){
				inspectingObj.transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime, Space.World);
			} else if (orientationDelta.y > padding){
				inspectingObj.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
			}

			reticleController.UpdateVisuals("Blank");
		}
	}

	void HandleMovement(){ //Handles movement from initalPos to moveTarget
		if(isMoving){ //If we're moving
			// % Journey complete = 1 - ((Distance from our pos to target - speed * time elapsed this frame) / distance from initial pos to target);
			lerpPercentage = 1 - (Vector3.Distance(transform.position, moveTarget) - (speed * Time.deltaTime)) / Vector3.Distance(initialPos, moveTarget);
			transform.position = Vector3.Lerp(initialPos, moveTarget, lerpPercentage); //Set our position dependent on lerp
			if(lerpPercentage >= 0.999f){ //If we're close enough to our target position
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
				MoveToLocationRaw(targetHit.point + new Vector3(0, height, 0)); //Move there
			}
			if(isMovingToInteract){
				isMovingToInteract = false;
			}
		}
	}
}

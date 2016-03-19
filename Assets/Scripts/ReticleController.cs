using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReticleController : MonoBehaviour, ICardboardPointer {

	public Sprite defaultReticle;
	public Sprite blankReticle;
	public Sprite walkReticle;
	public Sprite doorReticle;
	public Sprite pressReticle;
	public Sprite inspectReticle;

	Image reticleImage;

	void Awake(){
		reticleImage = GetComponent<Image>();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		GazeInputModule.cardboardPointer = this;
	}

	/// This is called when the 'BaseInputModule' system should be enabled.
	public void OnGazeEnabled(){
		
	}
	/// This is called when the 'BaseInputModule' system should be disabled.
	public void OnGazeDisabled(){
		
	}

	/// Called when the user is looking on a valid GameObject. This can be a 3D
	/// or UI element.
	///
	/// The camera is the event camera, the target is the object
	/// the user is looking at, and the intersectionPosition is the intersection
	/// point of the ray sent from the camera on the object.
	public void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition){
		UpdateVisuals(targetObject.tag);
	}

	/// Called every frame the user is still looking at a valid GameObject. This
	/// can be a 3D or UI element.
	///
	/// The camera is the event camera, the target is the object the user is
	/// looking at, and the intersectionPosition is the intersection point of the
	/// ray sent from the camera on the object.
	public void OnGazeStay(Camera camera, GameObject targetObject, Vector3 intersectionPosition){
		UpdateVisuals(targetObject.tag);
	}

	/// Called when the user's look no longer intersects an object previously
	/// intersected with a ray projected from the camera.
	/// This is also called just before **OnGazeDisabled** and may have have any of
	/// the values set as **null**.
	///
	/// The camera is the event camera and the target is the object the user
	/// previously looked at.
	public void OnGazeExit(Camera camera, GameObject targetObject){
		reticleImage.sprite = defaultReticle;
	}

	/// Called when the Cardboard trigger is initiated. This is practically when
	/// the user begins pressing the trigger.
	public void OnGazeTriggerStart(Camera camera){

	}

	/// Called when the Cardboard trigger is finished. This is practically when
	/// the user releases the trigger.
	public void OnGazeTriggerEnd(Camera camera){
	
	}

	public void UpdateVisuals(string targetTag){
		switch(targetTag){
			case "Blank":
				reticleImage.sprite = blankReticle;
			break;
			case "Floor":
			case "Ground":
				reticleImage.sprite = walkReticle;
			break;
			case "Door":
				reticleImage.sprite = doorReticle;
			break;
			case "Passcode Panel":
				reticleImage.sprite = pressReticle;
			break;
			case "InspectableObject":
				reticleImage.sprite = inspectReticle;
			break;
			default:
				reticleImage.sprite = defaultReticle;
			break;
		}
	}
}

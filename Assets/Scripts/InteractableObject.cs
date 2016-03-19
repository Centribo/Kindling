using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

public class InteractableObject : ClickableObject {

	public Vector3 interactLocation; //Where the player moves to before the object is used

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ClickObject(){ //If the player clicks on this object
		PlayerController.Instance.InteractWithObject(this); //Tell the player object to move to use then interact with us
	}

	public virtual void Interact(){
		does.Invoke();
	}
}

#if UNITY_EDITOR
//For drawing UI:
[CustomEditor(typeof(InteractableObject))]
public class testedi : Editor {
	void OnSceneGUI(){
		InteractableObject io = target as InteractableObject;
		if(io == null){
			return;
		} else {
			Handles.DrawDottedLine(io.transform.position, io.transform.position + io.interactLocation, 4.0f);
		}
	}
}
#endif
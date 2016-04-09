using UnityEngine;
using System.Collections;

public class Stair : MonoBehaviour {

	public Vector3 lower;
	public Vector3 upper;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void WalkStairs(){
		PlayerController.Instance.MoveToLocation(transform.position + lower);
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + lower, transform.position + upper);
		
	}
}

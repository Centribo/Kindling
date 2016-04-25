using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class WalkHereScript : MonoBehaviour {

	[Range(0, 100)]
	public float radius;
	public UnityEvent does;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= radius){
			does.Invoke();
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 1, 1, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
	}
}

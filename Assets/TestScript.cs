using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		if (GeometryUtility.TestPlanesAABB(planes , GetComponent<Collider>().bounds)){
			Debug.Log("SEEN!");
		} else {
			Debug.Log("UNSEEN!");
		}
	}
}

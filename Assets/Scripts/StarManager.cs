using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {

	public static StarManager instance = null;
	public static StarManager Instance { //Singleton pattern instance
		get { //Getter
			if(instance == null){ //If its null,
				instance = (StarManager)FindObjectOfType(typeof(StarManager)); //Find it
			}
			return instance; //Return it
		}
	}

	public GameObject starPrefab;

	[Range(0, 100)]
	public float radius;
	public int starCount;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < starCount; i++){
			Vector3 spawnPosition = Random.onUnitSphere * radius;
			if(spawnPosition.y < transform.position.y){
				i--;
			} else {
				SpawnStar(spawnPosition);
			}
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 1, 1, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
	}

	public void SpawnStar(Vector3 position){
		GameObject star = (GameObject) Instantiate(starPrefab, position, Quaternion.identity);
		star.transform.rotation = Random.rotation; //To give the illusion of random scales, since our stars are 2d planes, we just rotate them randomly
		star.transform.SetParent(transform);
	}
}

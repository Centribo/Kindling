using UnityEngine;
using System.Collections;

public class CampfireLight : MonoBehaviour {

	public float minIntensity;
	public float maxIntensity;
	public float flickerRate;

	Light light;
	float x = 0;
	float timer = 0;
	Vector3 originalPos;

	void Awake(){
		light = GetComponent<Light>();
		originalPos = transform.position;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		light.intensity = minIntensity + Mathf.PerlinNoise(x, 0) * maxIntensity;
		x += flickerRate;
		timer += Time.deltaTime;
		if(timer >= flickerRate){
			transform.position = originalPos + Random.insideUnitSphere * 0.05f;
			timer = 0;	
		}
		
	}
}

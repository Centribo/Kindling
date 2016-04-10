using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
[RequireComponent (typeof (MeshFilter))]
public class CampfireTransitionObject : MonoBehaviour {

	[Range(0, 1)]
	public float maxWidth;
	[Range(0, 1)]
	public float rate;
	[Range(0, 1)]
	public float starPercentage; //What percent of vertices should become stars

	public UnityEvent does;

	LineRenderer lr;
	Vector3[] vertices;
	Renderer r;
	Color originalColor;
	float width = 0;
	bool isTranitioning = false;

	void Awake(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		lr = GetComponent<LineRenderer>();
		r = GetComponent<Renderer>();
		originalColor = r.material.color;
		//lr.SetWidth(0, 0);
	}

	// Use this for initialization
	void Start () {
		lr.SetVertexCount(vertices.Length);

		for(int i = 0; i < vertices.Length; i++){
			lr.SetPosition(i, vertices[i]);
			Vector3 pos = vertices[i];
			pos.x *= transform.localScale.x;
			pos.y *= transform.localScale.y;
			pos.z *= transform.localScale.z;
			if(Random.value <= starPercentage){
				StarManager.Instance.SpawnStar(transform.position + pos);	
			}
		}
	}
	
	int j = 0;

	// Update is called once per frame
	void Update () {
		if(isTranitioning){
			width += rate * Time.deltaTime;
			lr.SetWidth(width, width);
			lr.SetVertexCount(j);
			for(int i = 0; i < j; i++){
				lr.SetPosition(i, vertices[i]);
			}
			if(j < vertices.Length){
				j++;
			} else {
				j = vertices.Length;
			}

			if(width >= maxWidth){
				width = maxWidth;
				lr.SetWidth(width, width);
			}
			if(width >= maxWidth && j == vertices.Length){
				Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
				if (GeometryUtility.TestPlanesAABB(planes , GetComponent<Collider>().bounds)){
					isTranitioning = false;
					Invoke("StartNextScene", 1);
				}
			}
		}
	}

	public void StartNextScene(){
		does.Invoke();
	}

	public void StartTransition(){
		isTranitioning = true;
	}
}

using UnityEngine;
using System.Collections;

public class SphereMovement : MonoBehaviour {

	const int fieldSizeX = 2;
	const int fieldSizeZ = 2;
	const float moveSpeed = 0.05f;
	Vector3 startPosition = Vector3.up;
	public GameObject sphere;
	public GameObject helper;
	public GameObject arrow;
	public enum sphereStates
	{
		HIDDEN,
		MOVING,
		DROPPING
	}

	public sphereStates state = sphereStates.HIDDEN;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (state);
		switch (state)
		{
		case sphereStates.DROPPING:
			break;
		case sphereStates.HIDDEN:
			break;
		case sphereStates.MOVING:
			float x = Input.GetAxis ("L_XAxis_1"); 
			float z = -Input.GetAxis ("L_YAxis_1");
			Vector3 v = sphere.transform.position;
			v.x = v.x + x * moveSpeed;
			if ( v.x < -fieldSizeX )
			{
				v.x = -fieldSizeX;
			}
			else if ( v.x > fieldSizeX)
			{
				v.x = fieldSizeX;
			}
			v.z = v.z + z * moveSpeed;
			if ( v.z < -fieldSizeZ )
			{
				v.z = -fieldSizeZ;
			}
			else if ( v.z > fieldSizeZ)
			{
				v.z = fieldSizeZ;
			}
			sphere.transform.position = v;
			if (Input.GetButtonDown("A_1"))
			{
				switchState(sphereStates.DROPPING);
			}

			break;
		}
	}

	public void switchState(sphereStates newState)
	{
		this.state = newState;
		switch (newState)
		{
			case sphereStates.DROPPING:
			arrow.renderer.enabled = false;
			sphere.renderer.enabled = false;
			helper.SendMessage("newTrial");
				break;
			case sphereStates.HIDDEN:
			arrow.renderer.enabled = false;
				sphere.renderer.enabled = false;
				break;
			case sphereStates.MOVING:
				sphere.transform.position = startPosition;
				sphere.renderer.enabled = true;
				break;
		}

	}


}

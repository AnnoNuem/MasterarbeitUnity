using UnityEngine;
using System.Collections;
//using Parameters;

public class SphereMovement : MonoBehaviour {

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
			v.x = v.x + x * Parameters.moveSpeed;
			if ( v.x < -Parameters.fieldSizeX )
			{
				v.x = -Parameters.fieldSizeX;
			}
			else if ( v.x > Parameters.fieldSizeX)
			{
				v.x = Parameters.fieldSizeX;
			}
			v.z = v.z + z * Parameters.moveSpeed;
			if ( v.z < -Parameters.fieldSizeZ )
			{
				v.z = -Parameters.fieldSizeZ;
			}
			else if ( v.z > Parameters.fieldSizeZ)
			{
				v.z = Parameters.fieldSizeZ;
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

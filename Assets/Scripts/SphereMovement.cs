/**
 * ReachOut 2D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sphere movement. Handles the sphere. State machine for different sphere states. Checks if sphere is moved by user, dropped, colides with ground. Logs the position the sphere is at
/// </summary>
public class SphereMovement : MonoBehaviour {

	Vector3 startPosition;
	public Vector3 dropPosition;
	public GameObject sphere;
	public GameObject helper;
	public GameObject arrow;
	public GameObject goal;
	public WindSpeed windSpeed;
	public Trials trials;
	public Statistics statistics;
	public enum sphereStates
	{
		HIDDEN,
		MOVING,
		DROPPING,
		COLLIDED,
	}
	public sphereStates state = sphereStates.HIDDEN;

	private List<Vector3> positions;

	void Start () 
	{
		windSpeed = WindSpeed.Instance;
		trials = Trials.Instance;
		statistics = Statistics.Instance;
		positions = new List<Vector3>();
		startPosition = Vector3.zero;
		startPosition.y = Parameters.startPositionHeight;
	}

	void FixedUpdate()
	{
		// apply windforce to sphere if it is dropped
		if (state == sphereStates.DROPPING)
		{
			Vector2 force = windSpeed.ComputeWindForce(sphere.transform.position);
			sphere.rigidbody.AddForce(new Vector3(force.x, 0, force.y));
		}
	}
		
	void Update () 
	{
		switch (state)
		{
		case sphereStates.MOVING:
			// set sphere position depending on joystick input and confine sphere in field
			positions.Add(sphere.transform.position);
			float x = Input.GetAxis (Parameters.joyXAxis); 
			float z = -Input.GetAxis (Parameters.joyYAxis);
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
			// drop sphere if drop button is pressed
			if (Input.GetButtonDown(Parameters.dropButton))
			{
				SwitchState(sphereStates.DROPPING);
			}
			break;
		}
	}

	/// <summary>
	/// Switchs state of sphere to "newState"
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SwitchState(sphereStates newState)
	{
		this.state = newState;
		switch (newState)
		{
			case sphereStates.DROPPING:
				dropPosition =sphere.transform.position;
				arrow.SetActive(false);
				sphere.rigidbody.useGravity = true;
				sphere.rigidbody.isKinematic = false;
				sphere.renderer.enabled = true;
				break;
			case sphereStates.HIDDEN:
				arrow.SetActive(false);
				sphere.renderer.enabled = false;
				sphere.rigidbody.useGravity = false;
				sphere.rigidbody.isKinematic = true;
				break;
			case sphereStates.MOVING:
				sphere.transform.position = startPosition;
				sphere.renderer.enabled = true;
				sphere.rigidbody.useGravity = false;
				sphere.rigidbody.isKinematic = true;
				// display the arrow indicating wind speed and direction if the current trial is a training or intro trial
				if (trials.currentTrial.type == Trials.typeOfTrial.INTRO || trials.currentTrial.type == Trials.typeOfTrial.TRAINING)
				{
					arrow.SetActive(true);
				}
				break;
		}
	}


	/// <summary>
	/// Raises the collision enter event. Compute statistics of the trial. Display sphere at hit position to allow subject to process where sphere hit the ground. Start new trial.
	/// </summary>
	/// <param name="col">Col.</param>
	IEnumerator OnCollisionEnter(Collision col)
	{	
		if (state == sphereStates.DROPPING && (col.collider.gameObject.name == "Ground"))
		{
			statistics.computeTrialStatistics(dropPosition, sphere.transform.position, goal.transform.position, positions);
			positions.Clear();
			sphere.rigidbody.useGravity = false;
			sphere.rigidbody.velocity = Vector3.zero;
			sphere.rigidbody.angularVelocity = Vector3.zero;
			SwitchState(sphereStates.COLLIDED);
			yield return new WaitForSeconds(Parameters.dispayOfHit);
			sphere.renderer.enabled = false;
			helper.SendMessage("newTrial");
		}
	}
}

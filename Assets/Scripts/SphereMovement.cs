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
			Vector2 force = windSpeed.ComputeWindForceForSphere(sphere.transform.position);
			sphere.rigidbody.AddForce(new Vector3(force.x, 0, force.y));
		}
	}

		
	bool grabbed = false;
	bool wasGrabbed = false;
	Vector3 offset;
	void Update () 
	{
		switch (state)
		{
		case sphereStates.MOVING:
			// set sphere position depending on joystick input and confine sphere in field
			if(grabbed)
			{
				positions.Add(sphere.transform.position);
			}
				
			Vector3 v = sphere.transform.position;

			// if sphere is grabbed move it with mouse cursor
			if (Input.GetMouseButton(Parameters.mouseButton) && grabbed)
			{
				v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - Parameters.startPositionHeight)) - offset;
			}

			// if button is clicked and cursor above sphere grab sphere
			if (Input.GetMouseButtonDown(Parameters.mouseButton))
			{
				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - Parameters.startPositionHeight));
				mousePosition.y = sphere.transform.position.y;
				if (Vector3.Distance(mousePosition, sphere.transform.position) < sphere.GetComponent<Renderer>().bounds.size.x/2)
				{
					grabbed = true;
					wasGrabbed = true;
					offset.x = mousePosition.x -sphere.transform.position.x;
					offset.z = mousePosition.z -sphere.transform.position.z;
				}
			}

			//keep sphere in field
			v.y = Parameters.startPositionHeight;
			if ( v.x < -Parameters.fieldSizeX )
			{
				v.x = -Parameters.fieldSizeX;
			}
			else if ( v.x > Parameters.fieldSizeX)
			{
				v.x = Parameters.fieldSizeX;
			}
			if ( v.z < -Parameters.fieldSizeZ )
			{
				v.z = -Parameters.fieldSizeZ;
			}
			else if ( v.z > Parameters.fieldSizeZ)
			{
				v.z = Parameters.fieldSizeZ;
			}
			sphere.transform.position = v;

//			// drop sphere if mouse button is released
			if (Input.GetMouseButtonUp(Parameters.mouseButton) && wasGrabbed)
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
				// check if sphere should be hidden while dropping in testing trials
				if (Parameters.hideDroppingSphere && trials.currentTrial.type == Trials.typeOfTrial.TESTING)
				{
					sphere.renderer.enabled = false;
				}
				else
				{
					sphere.renderer.enabled = true;
				}
				break;
			case sphereStates.HIDDEN:
				arrow.SetActive(false);
				sphere.renderer.enabled = false;
				sphere.rigidbody.useGravity = false;
				sphere.rigidbody.isKinematic = true;
				break;
			case sphereStates.MOVING:
				grabbed = false;
				wasGrabbed = false;
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

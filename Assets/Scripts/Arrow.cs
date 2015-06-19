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

/// <summary>
/// Arrow. Class to compute arrow size and rotation depending on wind speed and direction. Also sets position of arrow depending on position of sphere.
/// </summary>
public class Arrow : MonoBehaviour {

	public GameObject arrow;
	public GameObject sphere;
	static WindSpeed windSpeed;

	void Start () {
		windSpeed = WindSpeed.Instance;
	}	

	void Update () {
		if (arrow.renderer.enabled)
		{
			//rotate arrow arround itself
			arrow.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);

			//set arrow scale depending on local wind speed
			arrow.transform.localScale = new Vector3(
				windSpeed.ComputeWindSpeed(sphere.transform.position)*Parameters.arrowScale + Parameters.arrowMinSize,
				1, 
				windSpeed.ComputeWindSpeed(sphere.transform.position)*Parameters.arrowScale + Parameters.arrowMinSize);
			//set position of arrow leeward to sphere
			Vector3 pos = sphere.transform.position;
			pos.y = Parameters.arrowY;
			pos.z += Parameters.distanceFromSphere;
			arrow.transform.position = pos;
			arrow.transform.RotateAround(sphere.transform.position, Vector3.up, windSpeed.ComputeWindDirection(sphere.transform.position));
		}
	}
}

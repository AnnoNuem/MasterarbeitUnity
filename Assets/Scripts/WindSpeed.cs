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
using System;

/// <summary>
/// Wind speed. Provides methods to compute wind speed, direction and angles
/// </summary>
public sealed class WindSpeed 
{
	Trials trials;
	// singleton functions and variables
	private static readonly WindSpeed instance = new WindSpeed();
	
	static WindSpeed()
	{
	}
	
	private WindSpeed()
	{
		trials = Trials.Instance;
	}
	
	public static WindSpeed Instance
	{
		get
		{
			return instance;
		}
	}

	private float[] GetXY(Vector3 position)
	{
		float x = ((position[0] + Parameters.fieldSizeX) / (Parameters.fieldSizeX * 2f)) * trials.currentTrial.windScaleX;
		float z = ((position[2] + Parameters.fieldSizeZ ) / (Parameters.fieldSizeZ * 2f)) * trials.currentTrial.windScaleZ;
		float[] a = {x,z};
		return a;
	}
	
	//compute wind forces in x and z direction at given cordinate nominated betweeen 0 and 1 if x and z are within field
	public Vector2 ComputeWindForce(Vector3 position)
	{
		float[] a = GetXY(position);
		return new Vector2(a[0],a[1]);
	}
	
	// computes wind force and multiplies with factor to adjust speed of sphere
	public Vector2 ComputeWindForceForSphere(Vector3 position)
	{
		return ComputeWindForce(position) * Parameters.windForceForSphereFactor;
	}
	
	public float ComputeWindDirection(Vector3 position)
	{
		float hypo = (float)Math.Sqrt(trials.currentTrial.windScaleX * trials.currentTrial.windScaleX + trials.currentTrial.windScaleZ * trials.currentTrial.windScaleZ);
		float direction  = -1;
		if ( trials.currentTrial.windScaleZ == 0)
		{
			direction = (float) Math.Asin(trials.currentTrial.windScaleX/hypo);
		}
		else
		{
			direction  = (float) Math.Acos(trials.currentTrial.windScaleZ/hypo);
		}
		return  (float)(direction * (180.0f / Math.PI) );
	}
	
	public float ComputeWindSpeed(Vector3 position)
	{
		float[] a = GetXY(position);
		return (float) Math.Sqrt(a[0] * a[0] + a[1] * a[1]);
	}
}

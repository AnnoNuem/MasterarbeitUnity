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
using System;

/// <summary>
/// Trials. Creates Queue of trials. Handles current trials and returns new trials to main script
/// </summary>
public sealed class Trials
{
	public trial currentTrial;
	static public Logger logger;
		
	public enum typeOfTrial{
		INTRO,
		TRAINING,
		TESTING,
		END,
	}
	
	public struct trial{
		public typeOfTrial type;
		public Vector3 position;
		public string text;
		public float displaytime;
		public float windScaleX;
		public float windScaleZ;
	}
	
	private Queue trialQueue;
	private Vector3[] introPositions;
	private Vector3[] trainingPositions;
	private Vector3[] testingPositions;
	private double radiantIntro = Math.PI * Parameters.degreeIntro / 180.0;
	private double radiantTraining = Math.PI * Parameters.degreeTraining / 180.0;
	private double radiantTesting = Math.PI * Parameters.degreeTesting1 / 180.0;

	// singleton variables and functions
	private static readonly Trials instance = new Trials();
	
	static Trials()
	{
	}
	
	private Trials()
	{
		trialQueue = new Queue();	
		logger = Logger.Instance;
		// compute postion of possible goal positions
		introPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantIntro)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI/2 )*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI/2)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+3*Math.PI/2)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+3*Math.PI/2)*Parameters.goal_distance_intro)};
		trainingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTraining)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI/2 )*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI/2)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+3*Math.PI/2)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+3*Math.PI/2)*Parameters.goal_distance_training)};
		testingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTesting)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI/2 )*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI/2)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+3*Math.PI/2)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+3*Math.PI/2)*Parameters.goal_distance_testing)};
	}
	
	public static Trials Instance
	{
		get
		{
			return instance;
		}
	}

	public void CreateTrials()
	{
		//intro trials
		for (int i = 0; i <= Parameters.numberOfIntroTrials; i++)
		{
			trial t;
			t.windScaleX = Parameters.windScaleXIntro;
			t.windScaleZ = Parameters.windScaleZIntro;
			t.text = "Introduction\n\nHover with the mouse pointer over the green sphere.\nPress the left mouse button to grab the green sphere.\nMove the green sphere onto the red sphere and drop the green sphere by releasing the left mouse button.\n" +
				"Try to hit the red sphere exactly.\n You may notice that the green sphere is drifting if it is dropped. This is due to wind. The arrow above the green sphere tells you from where the wind blows and how strong it is.\n\n";
			t.displaytime = float.MaxValue;
			t.type = typeOfTrial.INTRO;
			// randomly select a position from the positions list
			t.position = introPositions[UnityEngine.Random.Range(0, introPositions.Length)];
			trialQueue.Enqueue(t);
		}

		//create superblocks of training trials followed by testing trials
		for (int i = 0; i < Parameters.numberOfRepetitions; i++)
		{
			//training trials
			for (int j = 0; j < Parameters.numberOfTrainingTrials; j++)
			{
				trial t;
				t.windScaleX = Parameters.windScaleXTraining0;
				t.windScaleZ = Parameters.windScaleZTraining0;
				if (i == 1)
				{
					t.windScaleX = Parameters.windScaleXTraining1;
					t.windScaleZ = Parameters.windScaleZTraining1;
				}
				if (i == 2)
				{
					t.windScaleX = Parameters.windScaleXTraining2;
					t.windScaleZ = Parameters.windScaleZTraining2;
				}
				t.text = "";
				t.displaytime = 0;
				if (j == 0)
				{
					t.text = "Training trials.\n\nThe arrow indicates wind speed and direction. The green sphere is influenced by the wind if it is dropped.\nWind conditions may have changed.\nTry to hit the red sphere exactly.\n\n" +
						"This text will disappear in 20 seconds.";
					t.displaytime = 20;
				}
				t.type = typeOfTrial.TRAINING;
				t.position = trainingPositions[UnityEngine.Random.Range(0, trainingPositions.Length)];
				trialQueue.Enqueue(t);
			}

			// testing trials
			for (int j = 0; j < Parameters.numberOfTestingTrials; j++)
			{
				trial t;
				t.windScaleX = Parameters.windScaleXTesting0;
				t.windScaleZ = Parameters.windScaleZTesting0;
				if (i == 1)
				{
					t.windScaleX = Parameters.windScaleXTesting1;
					t.windScaleZ = Parameters.windScaleZTesting1;
				}
				if (i == 2)
				{
					t.windScaleX = Parameters.windScaleXTesting2;
					t.windScaleZ = Parameters.windScaleZTesting2;
				}
				t.text = "";
				t.displaytime = 0;
				if (j == 0)
				{
					t.text = "Testing trials.\n\nNo arrow is indicating wind speed and direction. The green sphere is still influenced by the same wind as in previous trials if it is dropped.\nTry to hit the red sphere exactly.\n\n" +
						"This text will disappear in 10 seconds.";
					t.displaytime = 20;
				}
				t.type = typeOfTrial.TESTING;
				t.position = testingPositions[UnityEngine.Random.Range(0, testingPositions.Length)];
				trialQueue.Enqueue(t);
			}
		}

		//trial indicating the end of the experiment
		trial tEnd;
		tEnd.type = typeOfTrial.END;
		tEnd.text = "Well Done.\n\nThe experiment is over.\nThank you for your participation.";
		tEnd.displaytime = float.MaxValue;
		tEnd.position = Vector3.zero;
		tEnd.windScaleX = 0;
		tEnd.windScaleZ = 0;
		trialQueue.Enqueue(tEnd);
		// write information about trial creation into log file
		logger.Write(System.DateTime.Now + " Trial List Created\nNumberOfSuperblocks: " + Parameters.numberOfRepetitions + "\nNumber of IntroTrials: " + Parameters.numberOfIntroTrials +
		             "\nNumber of TrainingTrials: " + Parameters.numberOfTrainingTrials + "\nNumber of Testing Trials: " +
		             Parameters.numberOfTestingTrials + "\n");
	}

	/// <summary>
	/// Advances to the enxt trial
	/// </summary>
	public void NextTrial()
	{
		currentTrial = (trial)trialQueue.Dequeue();
	}
}

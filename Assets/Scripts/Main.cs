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
using UnityEngine.UI;

/// <summary>
/// Main. Handles the experiment. State Machine for different states of the game.
/// </summary>
public class Main : MonoBehaviour {
	
	public GameObject sphere;
	public GameObject arrow;
	public GameObject helper;
	public GameObject goal;
	public GameObject ground;
	public Canvas startscreen;
	public states state;
	public SphereMovement sphereScript;
	static Logger logger;
	static Trials trials;
	static Statistics statistics;
	public Text text;

	public enum states
	{
		STARTSCREEN,
		INTRO,
		TRAINING,
		TESTING,
		PAUSE,
		END
	}

	void Start () {
		logger = Logger.Instance;
		trials = Trials.Instance;
		statistics = Statistics.Instance;
		// begin experiment with displaying the startscreen
		switchState(states.STARTSCREEN);
		text.text = "";
	}

	void Awake()
	{
		Parameters.readParameters();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && state == states.END)
		{
			Application.Quit();
		}	
	}

	/// <summary>
	/// Switchs the state of the experiment to "newState"
	/// </summary>
	/// <param name="newState">New state.</param>
	void switchState(states newState)
	{
		switch (newState)
		{
		case states.PAUSE:
			startscreen.enabled = false;
			arrow.SetActive(false);
			goal.renderer.enabled = false;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.INTRO:
			startscreen.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.STARTSCREEN:
			Debug.Log("Startscreen");
			startscreen.enabled = true;
			arrow.SetActive(false);
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.TESTING:
			startscreen.enabled = false;
			arrow.SetActive(false);
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.TRAINING:
			startscreen.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			arrow.SetActive(false);
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.END:
			startscreen.enabled = false;
			arrow.SetActive(false);
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			logger.CloseLogFile();
			break;
		}
		state = newState;
	}	

	public IEnumerator newTrial()
	{
		Trials.typeOfTrial oldType = trials.currentTrial.type;
		switchState(states.PAUSE);
		yield return new WaitForSeconds(Parameters.pauseBetweenTrials);
		trials.NextTrial();
		StartCoroutine(displayText(trials.currentTrial.text, trials.currentTrial.displaytime));
		if (trials.currentTrial.type != Trials.typeOfTrial.END)
		{
			//if new block of traisl of other type compute statistics for previous trial block
			if (oldType != trials.currentTrial.type)
			{
				statistics.computeBlockStatistics();
				logger.Write("\n" + System.DateTime.Now + " New Block of " + trials.currentTrial.type + " trials.\n");
			}
			logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
			//set position of the goal defined in the curernt trial
			goal.transform.position = trials.currentTrial.position;
		}
		switch (trials.currentTrial.type){
		case Trials.typeOfTrial.INTRO:
			switchState(states.INTRO);
			break;
		case Trials.typeOfTrial.TESTING:
			switchState(states.TESTING);
			break;
		case Trials.typeOfTrial.TRAINING:
			switchState(states.TRAINING);
			break;
		case Trials.typeOfTrial.END:
			statistics.computeBlockStatistics();
			logger.Write("\n" + System.DateTime.Now + " Experimend ended");
			switchState(states.END);
			break;
		}
	}

	public void startExperimentPressed()
	{
		logger.CreateLogFile();
		trials.CreateTrials();
		logger.Write("\n" + System.DateTime.Now + " New Block of " + trials.currentTrial.type + " trials.\n");
		logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
		trials.NextTrial();
		StartCoroutine("newTrial");
	}

	IEnumerator displayText(string t, float length)
	{
		Debug.Log(t);
		text.text = t;
		yield return new WaitForSeconds(length);
		text.text = "";
	}

}

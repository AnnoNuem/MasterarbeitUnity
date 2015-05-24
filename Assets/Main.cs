using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public const float pauseBetweenTrials = 0.5f;
	public GameObject sphere;
	public GameObject arrow;
	public GameObject helper;
	public GameObject goal;
	public states state;
	static SphereMovement sphereScript;
	static Logger logger;
	static Trials trials;


	public enum states
	{
		STARTSCREEN,
		INTRO,
		TRAINING,
		TESTING,
		PAUSE,
		END
	}

	// Use this for initialization
	void Start () {
		logger = Logger.Instance;
		sphereScript = helper.GetComponent<SphereMovement>();
		trials = Trials.Instance;
		trials.CreateTrials();
		logger.Write("\n" + System.DateTime.Now + " New Blog of " + trials.currentTrial.type + " trials.\n");
		switchState(states.INTRO);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void switchState(states newState)
	{
		switch (newState)
		{
		case states.PAUSE:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			sphereScript.switchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.INTRO:
			Debug.Log("adsg");
			arrow.renderer.enabled = true;
			goal.renderer.enabled = true;
			sphereScript.switchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.STARTSCREEN:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			sphereScript.switchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.TESTING:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = true;
			sphereScript.switchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.TRAINING:
			arrow.renderer.enabled = true;
			goal.renderer.enabled = true;
			sphereScript.switchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.END:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			sphereScript.switchState(SphereMovement.sphereStates.HIDDEN);
			if (Input.GetButtonDown("A_1"))
			{
				logger.CloseLogFile();
				Application.Quit();
			}
			break;
		}
		state = newState;
	}	

	public IEnumerator newTrial()
	{
		Trials.typeOfTrial oldType = trials.currentTrial.type;
		switchState(states.PAUSE);
		yield return new WaitForSeconds(pauseBetweenTrials);
		if (!trials.NextTrial())
		{
			logger.Write("\n" + System.DateTime.Now + " Experimend ended");
			switchState(states.END);
		}
		else
		{
			if (oldType != trials.currentTrial.type)
			{
				logger.Write("\n" + System.DateTime.Now + " New Blog of " + trials.currentTrial.type + " trials.\n");
			}
			logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
			goal.transform.position = trials.currentTrial.position;
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
			}
		}
	}
}

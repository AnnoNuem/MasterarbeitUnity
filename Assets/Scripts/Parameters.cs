/**
 * ReachOut 2D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

/// <summary>
/// Parameters. Class to set important paramteres like number of trials
/// </summary>
public static class Parameters
{
	//fieldsize limiting the  area of movement and used for wind computation
	public const int fieldSizeX = 2;
	public const int fieldSizeZ = 2;

	//number of trials
	public const int numberOfIntroTrials = 4;
	public const int numberOfTrainingTrials = 4;
	public const int numberOfTestingTrials = 8;

	// Trial parameters. At with degrees spawns the spehre in different type of trials. 0 for example means north, east, south, west. 45 means northeast, southeast, southwest, northwest
	public const int degreeIntro = 0;
	public const int degreeTesting1 = 45;
	public const int degreeTraining = 45;

	//parameters for wind
	public const float xscale = 0.0f;
	public const float zscale = 1f;
	public const float xbias = 0.0f;
	public const float zbias = 0.1f;

	//scale of the arrow indicating wind direction and force
	public const float arrowScale = 0.001f;
	public const float arrowMinSize = 0.001f;
	public const float arrowY = 3.38f;
	//offset of arrow from sphere
	public static float distanceFromSphere = 0.05f;

	//how long should the sphere after hitting the ground bebe displayed
	public const float dispayOfHit = 0.7f;
	
	//pause between trials with only ground visible
	public const float pauseBetweenTrials = 0.3f;

	//Input
	public const string joyXAxis = "L_XAxis_1";
	public const string joyYAxis = "L_YAxis_1";
	public const string dropButton = "A_1";
	// how fast is the sphere moved by the joystick
	public const float moveSpeed = 0.01f;

	//parameters goal
	//how height should goal be diplayed over ground
	public const float goal_height = 0.0001f;
	// how far away should the goal be from the origin. If 1 the goal is positioned on a unit circle arround the origin. Denotes radius of circle the goal is positioned on
	public const float goal_scale = 0.4f;

	// start position of ball
	public const float startPositionHeight = 3.38f;
}

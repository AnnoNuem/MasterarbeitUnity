public static class Parameters
{
	//fieldsize limitin area of movement and used for wind computation
	public const int fieldSizeX = 2;
	public const int fieldSizeZ = 2;

	//speed of object moved by joystick
	public const float moveSpeed = 0.05f;

	//number of trials
	public const int numberOfIntroTrials = 2;
	public const int numberOfTrainingTrials = 2;
	public const int numberOfTestingTrials = 2;

	//parameters for wind
	public const float xscale = 0.0f;
	public const float zscale = 0.1f;
	public const float xbias = 0.0f;
	public const float zbias = 0.05f;

	//scale of the arrow indicating wind direction and force
	public const float arrowScale = 0.03f;
	public const float arrowMinSize = 0.01f;
}

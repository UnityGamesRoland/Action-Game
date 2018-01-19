using UnityEngine;
using UnityEngine.UI;

public class GradientPlayer : MonoBehaviour
{
	public enum Progression {linear, pingPong};
	public Progression progression;

	public Gradient gradient;
	[Range(0.1f, 10)] public float speed = 1f;

	private float gradientProgress;
	private Text text;
	private Image image;
	private RawImage rawImage;

	private void Start()
	{
		//Try to get a reference to any of these UI elements.
		text = GetComponent<Text>();
		image = GetComponent<Image>();
		rawImage = GetComponent<RawImage>();
	}

	private void Update()
	{
		//Calculate the progress on the gradient using linear progression.
		if(progression == Progression.linear)
		{
			//Continuously move the progress to 1, and then back to 0.
			gradientProgress = Mathf.MoveTowards(gradientProgress, 1, Time.fixedDeltaTime * speed);
			if(gradientProgress == 1) gradientProgress = 0;
		}

		//Calculate the progress on the gradient using ping-pong progression.
		else if(progression == Progression.pingPong)
		{
			//Move the progress from 0 to 1 via a sin function.
			gradientProgress = Mathf.Sin(Time.unscaledTime * speed) * 0.5f + 0.5f;
		}

		//Apply the gradient on the available UI element.
		if(text != null) text.color = gradient.Evaluate(gradientProgress);
		if(image != null) image.color = gradient.Evaluate(gradientProgress);
		if(rawImage != null) rawImage.color = gradient.Evaluate(gradientProgress);
	}
}

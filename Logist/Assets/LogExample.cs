using System;
using UnityEngine;

namespace LogistExamples
{
	public class LogExample : MonoBehaviour
	{
		private void Start()
		{

		}

		private void Update()
		{
			int r = UnityEngine.Random.Range(0, 10);

			switch (r)
			{
				case 0:
					Debug.LogWarning("Download more RAM");
				break;
				case 1: 
					Logist.Log("Button click", Logist.Tag.Input | Logist.Tag.GUI);
				break;
				case 2: 
					Logist.Log("Look! The Animator lost the animation references again!", Logist.Tag.Animation | Logist.Tag.Error);
				break;
				case 3:
					Logist.Log("Uhhhhh...", Logist.Tag.Warning);
				break;
				case 4:
					Logist.Log("The enemy is still spinning in circles", Logist.Tag.AI | Logist.Tag.Logic);
				break;
				case 5: 
					Logist.Log("Networked physics? Have fun with that.", Logist.Tag.Physics | Logist.Tag.Network);
				break;
				case 6:
					Logist.Log("You are clearly ignoring the profiler", Logist.Tag.Performance);
				break;
				case 7:
					Logist.Log("Sphere became magenta", Logist.Tag.Error | Logist.Tag.Graphics);
				break;
				case 8:
					Logist.Log("Pressed A", Logist.Tag.Input);
				break;
				case 9:
					Logist.Log("Hello world.", Logist.Tag.System);
				break;
			}
		}
	}
}
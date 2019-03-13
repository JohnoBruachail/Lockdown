using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

	public void exitGame()
	{
		Debug.Log ("Has quit the game");
		Application.Quit();
	}
}

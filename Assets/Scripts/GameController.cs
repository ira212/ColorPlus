using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject cubePrefab;
	float gameLength = 60;
	int gridX = 8;
	int gridY = 5;
	GameObject[,] grid;
	GameObject nextCube;
	Vector3 cubePos;
	Vector3 nextCubePos = new Vector3 (7, 10, 0);
	float turnLength = 3;
	int turn = 0;
	Color[] myColors = { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta };
	int score = 0;

	void CreateGrid () {
		grid = new GameObject[gridX, gridY];

		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				cubePos = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePos, Quaternion.identity);
			}
		}

	}

	void CreateNextCube () {
		nextCube = Instantiate (cubePrefab, nextCubePos, Quaternion.identity);
		nextCube.GetComponent<Renderer> ().material.color = myColors [ Random.Range(0, myColors.Length) ];
	}

	void EndGame (bool win) {
		// end the game!
		if (win) {
			// players win!
			print ("YOU WIN!!");
		} else {
			// players lose
			print ("You lose. Please try again!");
		}
	}

	int FindAvailableCube (int y) {
		// given a specific row, find a random white cube in that row, and return the x value
		// or, if there isn't one, return -1

		// placeholder, just return a random x value
		return Random.Range(0, gridX);
	}

	void PlaceNextCube (int y) {
		int x = FindAvailableCube (y);

		// no available cube in that row
		if (x == -1) {
			EndGame (false);
		}
		else {
			// assign the nextCube's color to the chosen cube
			grid [x, y].GetComponent<Renderer> ().material.color = nextCube.GetComponent<Renderer> ().material.color;
			Destroy (nextCube);
			nextCube = null;

		}

	}

	void ProcessKeyboardInput () {
		int numKeyPressed = 0;

		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			numKeyPressed = 1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			numKeyPressed = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			numKeyPressed = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
			numKeyPressed = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
			numKeyPressed = 5;
		}

		// if we still have a next cube and the player pressed a valid number key
		if (nextCube != null && numKeyPressed != 0) {
			
			// put it in the specified row, subtracting 1 since the grid array has a 0-based index
			PlaceNextCube (numKeyPressed - 1);
		}



	}

	void AddBlackCube() {
		// find a random white cube and turn it black
		// if that's impossible, end the game in a loss
		print ("Added a black cube!");
	}

	// Use this for initialization
	void Start () {

		CreateGrid ();
		
	}
	
	// Update is called once per frame
	void Update () {
		ProcessKeyboardInput ();

		if (Time.time > turnLength * turn) {
			turn++;

			// if we still have an existing Next Cube
			if (nextCube != null) {
				score -= 1;
				Destroy (nextCube);
				AddBlackCube ();
			}

			CreateNextCube ();
		}
	}
}

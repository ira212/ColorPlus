using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public GameObject cubePrefab;
	public Text cargoScoreText;
	Vector3 cubePosition;
	GameObject activeCube;
	int airplaneX, airplaneY, startX, startY;
	int depotX, depotY;
	GameObject[,] grid;
	int gridX, gridY;
	bool airplaneActive;
	float turnLength, turnTimer;
	int airplaneCargo, airplaneCargoMax;
	int cargoGain;
	int score;
	int moveY, moveX;
	int targetX, targetY;


	// Use this for initialization
	void Start () {
		turnLength = 1.5f;
		turnTimer = turnLength;

		score = 0;

		airplaneCargo = 0;
		airplaneCargoMax = 90;
		cargoGain = 10;

		gridX = 16;
		gridY = 9;
		grid = new GameObject[gridX, gridY];

		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				cubePosition = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePosition, Quaternion.identity);
				grid [x, y].GetComponent<CubeController> ().myX = x;
				grid [x, y].GetComponent<CubeController> ().myY = y;
			}
		}

		// airplane starts in the upper left
		startX = 0;
		startY = gridY - 1;
		airplaneX = startX;
		airplaneY = startY;
		targetX = airplaneX;
		targetY = airplaneY;
		grid [airplaneX, airplaneY].GetComponent<Renderer> ().material.color = Color.red;
		airplaneActive = false;
		depotX = gridX - 1;
		depotY = 0;
		grid [depotX, depotY].GetComponent<Renderer> ().material.color = Color.black;

		moveX = 0;
		moveY = 0;
	}
	
	public void ProcessClick (GameObject clickedCube, int x, int y) {
		// Was the airplane clicked?
		if (x == airplaneX && y == airplaneY) {
			if (airplaneActive) {
				// deactivate it
				airplaneActive = false;
				clickedCube.transform.localScale /= 1.5f;
			} else {
				// activate it
				airplaneActive = true;
				clickedCube.transform.localScale *= 1.5f;
			}
		}
		else if (airplaneActive) {
			targetX = x;
			targetY = y;
		}
	}

	void LoadCargo () {
		if (airplaneX == startX && airplaneY == startY) {
			airplaneCargo += cargoGain;

			if (airplaneCargo > airplaneCargoMax) {
				airplaneCargo = airplaneCargoMax;
			}
		}
	}

	void DeliverCargo() {
		if (airplaneX == depotX && airplaneY == depotY) {
			score += airplaneCargo;
			airplaneCargo = 0;
		}
	}

	void CalculateDirection() {
		if (airplaneY > targetY) {
			moveY = -1;
		}
		else if (airplaneY < targetY) {
			moveY = 1;
		}
		else {
			moveY = 0;
		}

		if (airplaneX < targetX) {
			moveX = 1;
		}
		else if (airplaneX > targetX) {
			moveX = -1;
		}
		else {
			moveX = 0;
		}
	}

	void MoveAirplane () {
		CalculateDirection ();

		if (airplaneActive) {
			// remove the airplane from it's old spot, if it's the depot set it to black
			if (airplaneX == depotX && airplaneY == depotY) {
				grid [depotX, depotY].GetComponent<Renderer> ().material.color = Color.black;
			}
			// otherwise, set it to white
			else {
				grid [airplaneX, airplaneY].GetComponent<Renderer> ().material.color = Color.white;
			}

			grid[airplaneX, airplaneY].transform.localScale /= 1.5f;

			// put the airplane in it's new spot
			airplaneX += moveX;
			airplaneY += moveY;

			// check to ensure the plane doesn't go out of bounds
			if (airplaneX >= gridX) {
				airplaneX = gridX - 1;
			}
			else if (airplaneX < 0) {
				airplaneX = 0;
			}

			if (airplaneY >= gridY) {
				airplaneY = gridY - 1;
			}
			else if (airplaneY < 0) {
				airplaneY = 0;
			}

			grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
			grid[airplaneX, airplaneY].transform.localScale *= 1.5f;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > turnTimer) {
			MoveAirplane ();

			LoadCargo();
			DeliverCargo ();
			cargoScoreText.text = "Cargo: " + airplaneCargo + "   Score: " + score;

			turnTimer += turnLength;
		}
		
	}
}

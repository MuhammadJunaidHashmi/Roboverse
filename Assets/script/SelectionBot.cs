//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// A simple manager script for all demo scenes. It has an array of spawnable player cars and public methods for spawning new cars, setting new behavior modes, restart, and quit application.
/// </summary>

public class SelectionBot : MonoBehaviour {

	[Header("Spawnable Bot")]
	public PrometeoCarController[] selectableBot;
	public GameObject btn;



	private int selectedCarIndex = 0;		// An integer index value used for spawning a new car.

	

	public void Spawn (int index) {

	
		if (index == -1) selectedCarIndex--;
		else selectedCarIndex++;

	
		if (selectableBot.Length> selectedCarIndex && 0<= selectedCarIndex)
        {
		


			Debug.Log("car = " + selectedCarIndex);


			// Getting all RCC cars on scene.
			PrometeoCarController[] activeVehicles = GameObject.FindObjectsOfType<PrometeoCarController>();
			Debug.Log(selectableBot.Length);

			// Last known position and rotation of last active car.
			Vector3 lastKnownPos = new Vector3();
			Quaternion lastKnownRot = new Quaternion();

			// Checking if there is at least one car on scene.
			if (activeVehicles != null && activeVehicles.Length > 0)
			{

				// Checking if car is AI or not. If it's not AI and controllable by player, last known position and rotation will be assigned to this car.
				// We will use this position and rotation for new spawned car.
				foreach (PrometeoCarController rcc in activeVehicles)
				{
					if(!rcc.AIController){
					lastKnownPos = rcc.transform.position;
					lastKnownRot = rcc.transform.rotation;
						break;
					}
				}

			}

			// If last known position and rotation is not assigned, camera's position and rotation will be used.
			if (lastKnownPos == Vector3.zero)
			{
				if (GameObject.FindObjectOfType<CameraFollow>())
				{
					lastKnownPos = GameObject.FindObjectOfType<CameraFollow>().transform.position;
					lastKnownRot = GameObject.FindObjectOfType<CameraFollow>().transform.rotation;
				}
			}

			// We don't need X and Z rotation angle. Just Y.
			lastKnownRot.x = 0f;
			lastKnownRot.z = 0f;

			for (int i = 0; i < activeVehicles.Length; i++)
			{

				// If we have controllable cars by players on scene, destroy them!
				if( !activeVehicles[i].AIController){
				Destroy(activeVehicles[i].gameObject);
				}

			}

			// Here we are creating a new gameobject for our new spawner car.
			GameObject newVehicle = (GameObject)GameObject.Instantiate(selectableBot[selectedCarIndex].gameObject, lastKnownPos + (Vector3.up), lastKnownRot);
		
			// Enabling canControl bool for our new car.
			//newVehicle.GetComponent<PrometeoCarController>().canControl = true;

			// Setting new target of RCC Camera to our new car.
			if (GameObject.FindObjectOfType<CameraFollow>())
			{

				GameObject.FindObjectOfType<CameraFollow>().SetPlayerBot(newVehicle);
			}

			// If our scene has RCC Customizer Example, this will set target car of that customizer and checks all UI elements belongs to customization.
			//	if (GameObject.FindObjectOfType<RCC_CustomizerExample> ()) {
			//	GameObject.FindObjectOfType<RCC_CustomizerExample> ().car = newVehicle.GetComponent<RCC_CarControllerV3> ();
			//GameObject.FindObjectOfType<RCC_CustomizerExample> ().CheckUIs ();
			//	}

			//if (selectableBot.Length != selectedCarIndex)
			//selectedCarIndex++;


		}
			if (selectedCarIndex > 3)
			selectedCarIndex = 3;
		if (selectedCarIndex < 0)
			selectedCarIndex = 0;
	}

	// An integer index value used for setting behavior mode.

	// Simply restarting the current scene

}

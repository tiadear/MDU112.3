using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public void LoadLevel (string levelName) {
		SceneManager.LoadScene (levelName);
	}
}

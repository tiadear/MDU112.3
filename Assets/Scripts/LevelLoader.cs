using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public IEnumerator LoadNextLevel (string levelName) {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene (levelName);
	}

	public string CurrentLevel() {
		Scene current = SceneManager.GetActiveScene ();
		return current.name;
	}

	public void LoadLevel(string levelName) {
		SceneManager.LoadScene (levelName);
	}
}

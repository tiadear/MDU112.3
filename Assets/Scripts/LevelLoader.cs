using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public IEnumerator LoadLevel (string levelName) {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene (levelName);
	}

	public string CurrentLevel() {
		Scene current = SceneManager.GetActiveScene ();
		return current.name;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour {

    public void LoadNewGame(int sceneIndex)
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            GameControl.control.DeleteData();
        }
        GameControl.control.setDefaults();
        SceneManager.LoadScene(sceneIndex);  
    }

	public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        GameControl.control.level = sceneIndex;
    }

    public void LoadSavedLevel()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            SceneManager.LoadScene(GameControl.control.level);
        }
    }
}

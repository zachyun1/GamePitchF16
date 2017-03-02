using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public float health;
    public int level;
    public bool saveExists;

	// Use this for initialization
	void Awake ()
    {
		if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }
	}

    void OnGUI()
    {

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");

        PlayerData data = new PlayerData();
        data.health = health;
        data.level = level;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);   

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            level = data.level;
        }
    }

    public void DeleteData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerData.dat");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void setDefaults()
    {
        health = 100;
        level = 1;
    }

}

[Serializable]
class PlayerData
{
    public float health;
    public int level;
}

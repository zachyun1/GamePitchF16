using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIButtons : MonoBehaviour {

    public GameObject saveNotif;
    public GameObject loadNotif;

    IEnumerator LateCall(GameObject obj)
    {
        yield return new WaitForSeconds(2.0f);

        obj.SetActive(false);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 60, 100, 30), "Damage"))
        {
            if(GameControl.control.health > 0)        
                GameControl.control.health -= 10;
        }
        if (GUI.Button(new Rect(10, 100, 100, 30), "Heal"))
        {
            if(GameControl.control.health < 100)
                GameControl.control.health += 10;
        }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Save"))
        {
            GameControl.control.Save();
            saveNotif.SetActive(true);
            StartCoroutine(LateCall(saveNotif));
        }
        if (GUI.Button(new Rect(10, 180, 100, 30), "Load"))
        {
            GameControl.control.Load();
            loadNotif.SetActive(true);
            StartCoroutine(LateCall(loadNotif));
        }
        if (GUI.Button(new Rect(10, 220, 100, 30), "Menu"))
        {
            GameControl.control.LoadMenu();
        }

    }
}

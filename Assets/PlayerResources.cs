using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    private Image healthbar;

	// Use this for initialization
	void Start () {
        healthbar = gameObject.transform.Find("PlayerCanvas").transform.Find("HealthbarFG").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(healthbar)
            healthbar.fillAmount = GameControl.control.health / 100;
    }
}

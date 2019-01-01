﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelloWorld : MonoBehaviour {

    public string[] hellos = new string[4]
    {
        "Hello World", "Hola Mundo", "Bonjour Le Monde", "Hallo Welt"
    };

    public Text textString;

	void Start () 
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
        int randomIndex = Random.Range(0, 4);
        textString.text = hellos[randomIndex];
	}
}

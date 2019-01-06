using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public float turnDelay = 0.1f;							//Delay between each Player turn.
	public int healthPoints = 100;							//Starting value for Player health points.
	public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
	
    [HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
    private bool enemiesMoving;                             //Boolean to check if enemies are moving.

    private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
    private BoardManager boardScript;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);	
		
		DontDestroyOnLoad(gameObject);
		
		enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
		
		InitGame();
	}
	
	void OnLevelWasLoaded(int index)
	{
		InitGame();
	}
	
	void InitGame()
	{
		enemies.Clear();
        boardScript.BoardSetup();
	}
	
	void Update()
	{
		if(playersTurn || enemiesMoving)
			return;
		
		StartCoroutine (MoveEnemies ());
	}
	
	public void GameOver()
	{
		enabled = false;
	}
	
	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		
		yield return new WaitForSeconds(turnDelay);
		
		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds(turnDelay);
		}

		playersTurn = true;

		enemiesMoving = false;
	}

    public void updateBoard(int horizantal, int vertical)
    {
        boardScript.AddToBoard(horizantal, vertical);
    }
}
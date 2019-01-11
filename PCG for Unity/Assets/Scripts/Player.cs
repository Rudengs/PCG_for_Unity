using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject
{ 
    private Animator animator;                  
    private int health;                         

    public Text healthText;                     
    public static Vector2 position;
    public int wallDamage = 1;                  
    public bool onWorldBoard;
    public bool dungeonTransition;

	
	protected override void Start ()
	{
		animator = GetComponent<Animator>();
		health = GameManager.instance.healthPoints;
		healthText.text = "Health: " + health;
        position.x = position.y = 2;
        onWorldBoard = true;
        dungeonTransition = false;

		base.Start ();
	}
	
	private void Update ()
	{
		if(!GameManager.instance.playersTurn) return;
		
		int horizontal = 0;  	//Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.

        bool canMove = false;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		if(horizontal != 0)
		{
			vertical = 0;
		}

		if(horizontal != 0 || vertical != 0)
		{
            canMove = AttemptMove<Wall> (horizontal, vertical);

            if(canMove & onWorldBoard)
            {
                position.x += horizontal;
                position.y += vertical;
                GameManager.instance.updateBoard(horizontal, vertical);
            }
        }
	}
	
    private void GoDungeonPotal()
    {
        if(onWorldBoard)
        {
            onWorldBoard = false;
            GameManager.instance.EnterDungeon();
            transform.position = DungeonManager.startPos;
        }
        else
        {
            onWorldBoard = true;
            GameManager.instance.ExitDungeon();
            transform.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            dungeonTransition = true;
            Invoke("GoDungeonPotal", 0.5f);
            Destroy(collision.gameObject);
        }
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    protected override bool AttemptMove <T> (int xDir, int yDir)
	{	
		bool hit = base.AttemptMove <T> (xDir, yDir);
		
		GameManager.instance.playersTurn = false;

		return hit;
	}
	
	protected override void OnCantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}
	
	public void LoseHealth (int loss)
	{
		animator.SetTrigger ("playerHit");
		health -= loss;
		healthText.text = "-"+ loss + " Health: " + health;
		CheckIfGameOver ();
	}
}


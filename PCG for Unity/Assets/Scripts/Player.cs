using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MovingObject
{ 
    private Animator animator;                  
    private int health;
    private Dictionary<string, Item> inventory;  

    public Text healthText;
    public Image glove;
    public Image boot;
                  
    public static Vector2 position;
    public int wallDamage = 1;
    public int attackMod = 0, defenseMod = 0;
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

        inventory = new Dictionary<string, Item>();

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
            if (dungeonTransition)
            {
                if (onWorldBoard)
                    canMove = AttemptMove<Wall>(horizontal, vertical);
                else
                    canMove = AttemptMove<Chest>(horizontal, vertical);
            }
            else
                canMove = AttemptMove<Wall>(horizontal, vertical);

            if (canMove & onWorldBoard)
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
        else if(collision.tag == "Food" || collision.tag == "Soda")
        {
            UpdateHealth(collision);
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Item")
        {
            UpdateInventory(collision);
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
        if(typeof(T) == typeof(Wall))
        {
            Wall blockingObj = component as Wall;
            blockingObj.DamageWall(wallDamage);
        }
        else if (typeof(T) == typeof(Chest))
        {
            Chest blockingObj = component as Chest;
            blockingObj.Open();
        }


        animator.SetTrigger("playerChop");
    }
	
	public void LoseHealth (int loss)
	{
		animator.SetTrigger ("playerHit");
		health -= loss;
		healthText.text = "-"+ loss + " Health: " + health;
		CheckIfGameOver ();
	}

    public void UpdateHealth(Collider2D item)
    {
        if(health < 100)
        {
            if (item.tag == "Food")
                health += Random.Range(1, 4);
            else
                health += Random.Range(4, 11);
        }
        GameManager.instance.healthPoints = health;
        healthText.text = "Health: " + health;
    }

    private void UpdateInventory(Collider2D item)
    {
        Item itemData = item.GetComponent<Item>();

        switch(itemData.type)
        {
            case itemType.glove:
                if (!inventory.ContainsKey("glove"))
                    inventory.Add("glove", itemData);
                else
                    inventory["glove"] = itemData;
                glove.color = itemData.level;
                break;
            case itemType.boot:
                if (!inventory.ContainsKey("boot"))
                    inventory.Add("boot", itemData);
                else
                    inventory["boot"] = itemData;
                boot.color = itemData.level;
                break;
        }

        attackMod = 0;
        defenseMod = 0;

        foreach(KeyValuePair<string,Item> gear in inventory)
        {
            attackMod += gear.Value.attackMod;
            defenseMod += gear.Value.defenseMod;
        }
    }
}


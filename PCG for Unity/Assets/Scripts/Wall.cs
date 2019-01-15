using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
	public Sprite dmgSprite;					
	public int hp = 3;
    public GameObject[] foodTiles;

	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public void DamageWall (int loss)
	{
		spriteRenderer.sprite = dmgSprite;
		
		hp -= loss;
		
		if(hp <= 0)
        {
            if(Random.Range(0, 5) == 1)
            {
                GameObject toInstiate = foodTiles[Random.Range(0, foodTiles.Length)];
                GameObject instance = Instantiate(toInstiate, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(transform.parent);
            }
            gameObject.SetActive(false);
        }
	}
}

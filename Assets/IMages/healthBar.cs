using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {

	public Sprite[] newSprite;
	public Image[] healthUi;

	public void healthHit (uint playerID, int imageID)
	{
		healthUi[playerID].sprite = newSprite[imageID];
	}
}

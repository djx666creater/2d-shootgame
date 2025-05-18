using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text lifeText;
    public Text bulletText;
    public Text stoneText;

    private void Awake()
    {
        LifeUpdate();
        BulletUpdate();
        StoneUpdate();
    }

    public void LifeUpdate()
    {
        lifeText.text = "X" + PlayerPrefs.GetInt("PlayerBlood").ToString();
    }

    public void BulletUpdate()
    {
        bulletText.text = "X" + PlayerPrefs.GetInt("Bullet").ToString();
    }

    public void StoneUpdate()
    {
        stoneText.text = "X" + PlayerPrefs.GetInt("PlayerStone").ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBlood : MonoBehaviour
{
    public Player player;
    public Canvas m_Canvas;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            int playerBlood = PlayerPrefs.GetInt("PlayerBlood");

            playerBlood += 10;
            if(playerBlood > 100)
            {
                playerBlood = 100;
            }

            PlayerPrefs.SetInt("PlayerBlood", playerBlood);
            player.playerBlood = playerBlood;
            m_Canvas.LifeUpdate();              //UI更新生命值
            Destroy(this.gameObject);
        }
    }
}

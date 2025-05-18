using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Canvas m_Canvas;
    private void Awake()
    {
        m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            int numStone = PlayerPrefs.GetInt("PlayerStone") + 50;
            PlayerPrefs.SetInt("PlayerStone", numStone);
            m_Canvas.StoneUpdate();
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonCollider : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            player.isJumping = false;      //玩家碰到地板后，跳跃结束，恢复可以跳跃的状态
            player.m_Animator.SetBool("Jump", false);      //玩家碰到地板后，跳跃动画结束
        }
        if(collision.tag == "MoveGround")
        {
            player.isJumping = false;      //玩家碰到地板后，跳跃结束，恢复可以跳跃的状态
            player.m_Animator.SetBool("Jump", false);      //玩家碰到地板后，跳跃动画结束
            player.transform.parent = collision.transform;  //让玩家成为地板的子物件
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "MoveGround")
        {
            player.transform.parent = null;
        }
    }
}

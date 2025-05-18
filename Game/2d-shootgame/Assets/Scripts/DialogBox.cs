using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public Text dialogText;
    public Button nextButton;

    private string[] dialogLines =
        {"欢迎来到由Unity制作的射击闯关游戏！",
         "温馨提示:E键进行挥刀攻击，F键与NPC交互，鼠标左键进行射击！",
         "现在开始你的冒险之旅吧！",
    };
    private int currentLine = 0;

    void Start()
    {
        ShowDialog();
    }
    public void ShowDialog()
    {
        Time.timeScale = 0f;
        dialogText.text = dialogLines[currentLine];
        gameObject.SetActive(true);             //显示对话框
    }

    public void NextDialogLine()
    {
        currentLine++;
        if (currentLine < dialogLines.Length)
        {
            ShowDialog();
        }
        else
        {
            EndDialog();
        }
    }

    public void EndDialog()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

}

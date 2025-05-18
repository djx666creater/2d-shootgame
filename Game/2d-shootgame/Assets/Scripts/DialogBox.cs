using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public Text dialogText;
    public Button nextButton;

    private string[] dialogLines =
        {"��ӭ������Unity���������������Ϸ��",
         "��ܰ��ʾ:E�����лӵ�������F����NPC���������������������",
         "���ڿ�ʼ���ð��֮�ðɣ�",
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
        gameObject.SetActive(true);             //��ʾ�Ի���
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

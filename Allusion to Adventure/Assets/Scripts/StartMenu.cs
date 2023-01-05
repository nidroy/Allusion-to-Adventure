using TMPro;
using UnityEngine;

/// <summary>
/// ��������� ����
/// </summary>
public class StartMenu : MonoBehaviour
{
    public Animator anim; // ��������

    public Loading loading; // ��������

    public TMP_InputField username; // ��� ������������
    public TMP_InputField password; // ������

    public TMP_Text answer; // �����


    /// <summary>
    /// �����������
    /// </summary>
    public void Authorization()
    {
        LogIn("Authorization");
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void Registration()
    {
        LogIn("Registration");
    }

    /// <summary>
    /// ������ ����� ����
    /// </summary>
    public void NewGame()
    {
        Proxy.SendMessage("NewGame");
        World.isNewGame = true;
        Inventory.isNewGame = true;

        LoadScene();
    }

    /// <summary>
    /// ���������� ����
    /// </summary>
    public void ContinueGame()
    {
        World.isNewGame = false;
        Inventory.isNewGame = false;

        LoadScene();
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// ����� � �������
    /// </summary>
    /// <param name="command">������� �������</param>
    public void LogIn(string command)
    {
        this.answer.text = "";

        Proxy.CreateConnection();
        Proxy.SendMessage(string.Format("{0}\t{1}\t{2}", command, username.text, password.text));
        string answer = Proxy.ReceiveMessage();

        if (answer == "Successful")
            anim.SetBool("isLogIn", true);
        else
            this.answer.text = answer;
    }

    /// <summary>
    /// ����� �� ��������
    /// </summary>
    public void LogOut()
    {
        username.text = "";
        password.text = "";

        anim.SetBool("isLogIn", false);
    }

    /// <summary>
    /// ��������� �����
    /// </summary>
    private void LoadScene()
    {
        loading.sceneID = 1;
        Instantiate(loading);
    }
}

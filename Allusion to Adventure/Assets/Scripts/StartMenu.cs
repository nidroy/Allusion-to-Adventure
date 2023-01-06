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
        World.isNewGame = true;
        Inventory.isNewGame = true;

        WorldStocks.sword = 2;
        WorldStocks.axe = 2;

        LoadScene();
    }

    /// <summary>
    /// ���������� ����
    /// </summary>
    public void ContinueGame()
    {
        World.isNewGame = false;
        Inventory.isNewGame = false;

        NewGame();

        //LoadScene();
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void ExitGame()
    {
        Proxy.SendMessage("LogOut");

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

        Proxy.SendMessage("LogOut");

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

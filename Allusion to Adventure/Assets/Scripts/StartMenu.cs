using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public Animator anim; // ��������

    public Loading loading; // ��������

    public TMP_InputField username; // ��� ������������
    public TMP_InputField password; // ������


    /// <summary>
    /// �����������
    /// </summary>
    public void Authorization()
    {
        LogIn();
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void Registration()
    {
        LogIn();
    }

    /// <summary>
    /// ������ ����� ����
    /// </summary>
    public void NewGame()
    {
        LoadScene();
    }

    /// <summary>
    /// ���������� ����
    /// </summary>
    public void ContinueGame()
    {
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
    public void LogIn()
    {
        anim.SetBool("isLogIn", true);
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

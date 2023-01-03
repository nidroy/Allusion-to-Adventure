using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public Animator anim; // анимации

    public Loading loading; // загрузка

    public TMP_InputField username; // имя пользователя
    public TMP_InputField password; // пароль


    /// <summary>
    /// авторизация
    /// </summary>
    public void Authorization()
    {
        LogIn();
    }

    /// <summary>
    /// регистрация
    /// </summary>
    public void Registration()
    {
        LogIn();
    }

    /// <summary>
    /// начать новую игру
    /// </summary>
    public void NewGame()
    {
        LoadScene();
    }

    /// <summary>
    /// продолжить игру
    /// </summary>
    public void ContinueGame()
    {
        LoadScene();
    }

    /// <summary>
    /// выйти из игры
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// войти в аккаунт
    /// </summary>
    public void LogIn()
    {
        anim.SetBool("isLogIn", true);
    }

    /// <summary>
    /// выйти из аккаунта
    /// </summary>
    public void LogOut()
    {
        username.text = "";
        password.text = "";

        anim.SetBool("isLogIn", false);
    }

    /// <summary>
    /// загрузить сцену
    /// </summary>
    private void LoadScene()
    {
        loading.sceneID = 1;
        Instantiate(loading);
    }
}

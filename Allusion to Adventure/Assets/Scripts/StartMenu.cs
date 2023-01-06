using TMPro;
using UnityEngine;

/// <summary>
/// начальное меню
/// </summary>
public class StartMenu : MonoBehaviour
{
    public Animator anim; // анимации

    public Loading loading; // загрузка

    public TMP_InputField username; // имя пользователя
    public TMP_InputField password; // пароль

    public TMP_Text answer; // ответ


    /// <summary>
    /// авторизация
    /// </summary>
    public void Authorization()
    {
        LogIn("Authorization");
    }

    /// <summary>
    /// регистрация
    /// </summary>
    public void Registration()
    {
        LogIn("Registration");
    }

    /// <summary>
    /// начать новую игру
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
    /// продолжить игру
    /// </summary>
    public void ContinueGame()
    {
        World.isNewGame = false;
        Inventory.isNewGame = false;

        NewGame();

        //LoadScene();
    }

    /// <summary>
    /// выйти из игры
    /// </summary>
    public void ExitGame()
    {
        Proxy.SendMessage("LogOut");

        Application.Quit();
    }

    /// <summary>
    /// войти в аккаунт
    /// </summary>
    /// <param name="command">команда серверу</param>
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
    /// выйти из аккаунта
    /// </summary>
    public void LogOut()
    {
        username.text = "";
        password.text = "";

        Proxy.SendMessage("LogOut");

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

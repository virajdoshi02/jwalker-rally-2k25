using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public static void RestartGame() {
        SceneManager.LoadScene("CarTest");
    }

    public static void QuitGame() {
        Application.Quit();
    }

    public static void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
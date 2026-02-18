using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationUI : MonoBehaviour
{
    public void LoadLevelScene()
    {
        SceneManager.LoadScene("TitleScreen");
        //SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

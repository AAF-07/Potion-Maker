using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ToPotionMaker()
    {
        SceneManager.LoadScene(2);
    }
}

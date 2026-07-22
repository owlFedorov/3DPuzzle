using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle3D
{
    public class StartScreen : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    void Update()
    {
        // Check if the current scene is "ZooScene"
        if (SceneManager.GetActiveScene().name == "Zoo")
        {
            // Check for Escape key press
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Load the "MainMenu" scene
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
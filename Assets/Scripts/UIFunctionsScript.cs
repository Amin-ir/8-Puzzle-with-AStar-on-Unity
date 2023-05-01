using UnityEngine;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFunctionsScript : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject failureMenu;

	public void letAStarPlay()
    {
        failureMenu.SetActive(false);

        boardScript game = FindObjectOfType<boardScript>();

        game.manualPlaying = false;
        
        AStar solution = new AStar(game.currentStatus);
        solution.solve(solution.initialNode);
        
        if (solution.reachedGoal)
            game.solutionToThisState = solution.solutionSteps;

    }
    public void muteMusic()
    {
        var music = FindObjectOfType<AudioSource>();
        if (music.isPlaying)
        {
            music.Pause();
        }
        else playMusic(music);
    }
    public void playMusic(AudioSource music)
    {
        music.Play();
    }
    public void play()
    {
        GetComponent<Animator>().Play("playGameAnimation");
    }
    public void showAbout()
    {
        GetComponent<Animator>().Play("goToAboutContent");
    }
    public void showMainMenu()
    {
        GetComponent<Animator>().Play("goToMainMenuOptions");
    }
    public void pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void getMoreTime()
    {
        var game = FindObjectOfType<boardScript>();
        game.gameplayTimer += 60;
        game.gameplayTimerBackground.color = Color.white;
        Time.timeScale = 1f;
        failureMenu.SetActive(false);
    }
    public void loadPlayScene()
    {
        SceneManager.LoadScene(1);
    }
    public void loadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

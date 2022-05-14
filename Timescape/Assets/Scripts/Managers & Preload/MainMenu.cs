using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    [SerializeField] private Button levelButton;
    [SerializeField] private Button quitButton;
    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventLoadLevel OnMainMenuLevelLoad;
    Coroutine musicfade;

    private void Start()
    {

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        levelButton.onClick.AddListener(HandleLoadLevel1Clicked);
        quitButton.onClick.AddListener(HandleQuitClicked);

        FadeIn();
    }

    //Selon le bouton, on envoie un event avec le nom du Level charg� (1 ou 2)
    private void HandleLoadLevel1Clicked()
    {
        if (!UIManager.Instance.AfterClick)
        {
            OnMainMenuLevelLoad.Invoke("Level1");
        }
    }

    private void HandleQuitClicked() //Le bouton Quit permet de quitter le jeu
    {
        Application.Quit();
    }

    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.PREGAME && currentstate == GameManager.GameState.RUNNING) //Si l'ancien State �tait pregame et le nouveau Running
        {
            FadeOut(); //On part du menu
        }
        if (previousstate != GameManager.GameState.PREGAME && currentstate == GameManager.GameState.PREGAME) //Si l'ancien n'�tait pas pregame et le nouveau si
        {
            if (previousstate != GameManager.GameState.WIN && previousstate != GameManager.GameState.LOSE)
            {
                GameManager.Instance.UnloadLevel(GameManager.Instance.LevelName); //On d�charge le niveau
            }
            //FadeIn(); //On entre dans le menu
        }

    }

    public void OnFadeOutComplete() //Fin de sortie du menu
    {
        OnMainMenuFadeComplete.Invoke(true); //On envoie un event pour indiquer la fin de l'anim
        gameObject.SetActive(false); //On d�sactive le menu
        UIManager.Instance.SetDummyCameraActive(false); //On d�sactive la cam�ra UIManager
    }

    public void OnFadeInComplete() //Fin d'entr�e dans le menu
    {
       OnMainMenuFadeComplete.Invoke(false); //On envoie un event pour indiquer la fin de l'anim
       musicfade = StartCoroutine(AudioManager.Instance.Play("MenuMusic", 1f, 1f)); //On joue la musique de menu
    }

    public void FadeIn() //D�but d'entr�e
    {
        Debug.Log("FadIN");
        UIManager.Instance.SetDummyCameraActive(true); //Activation de la cam�ra
        _mainMenuAnimator.Stop(); //Activation de l'animation
        _mainMenuAnimator.clip = _fadeInAnimationClip;
        _mainMenuAnimator.Play();
    }

    public void FadeOut() //D�but de sortie
    {
        StopCoroutine(musicfade);
        StartCoroutine(AudioManager.Instance.StopFadeOut("MenuMusic", .5f)); //On stoppe la musique du menu
        _mainMenuAnimator.Stop(); //Activation de l'animation
        _mainMenuAnimator.clip = _fadeOutAnimationClip;
        _mainMenuAnimator.Play();
    }
}
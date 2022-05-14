using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class WinMenu : MonoBehaviour
{
    [SerializeField] Animation _winMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button QuitButton;
    public Events.EventLoseWinFadeComplete OnWinMenuFadeComplete;
    private bool choice;
    Coroutine musicfade;

    private void Start()
    {
        MenuButton.onClick.AddListener(HandleMenuClicked);
        ReplayButton.onClick.AddListener(HandleNextLevelClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleNextLevelClicked() //On charge le prochain niveau (forc�ment niveau 2)
    {
        choice = false; //false pour le choix "nextlevel"
        FadeOut(); //d�but de sortie du menu win
    }

    private void HandleMenuClicked()
    {
        choice = true; //true pour le choix "menu"
        FadeOut(); //d�but de sortie du menu win
    }

    private void HandleQuitClicked()
    {
        Application.Quit(); //on quitte le jeu
    }

    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.WIN && currentstate != GameManager.GameState.WIN) //Si le pr�c�dent state �tait WIN et le nouveau ne l'est pas
        {
            FadeOut(); //On sort du menu Win
        }
    }

    public void OnFadeOutComplete() //Fin de sortie
    {
        OnWinMenuFadeComplete.Invoke(true, choice); //On envoie l'event avec le choix entre false -> prochain niveau et true -> menu
        gameObject.SetActive(false); //On d�sactive le menu
    }

    public void OnFadeInComplete() //Fin d'entr�e
    {
        OnWinMenuFadeComplete.Invoke(false, choice); //On envoie l'event
        musicfade = StartCoroutine(AudioManager.Instance.Play("WinMusic", .8f, 1f)); //On lance la musique de victoire
    }

    public void FadeIn() //D�but d'entr�e
    {
        Debug.Log("FadeInWin");
        gameObject.SetActive(true); //On active l'objet
        UIManager.Instance.SetDummyCameraActive(true); //On active la cam�ra
        _winMenuAnimator.Stop(); //On active l'anim
        _winMenuAnimator.clip = _fadeInAnimationClip;
        _winMenuAnimator.Play();
    }

    public void FadeOut() //D�but de sortie
    {
        StopCoroutine(musicfade);
        AudioManager.Instance.InstantStop("WinMusic");
        _winMenuAnimator.Stop(); //On active l'anim
        _winMenuAnimator.clip = _fadeOutAnimationClip;
        _winMenuAnimator.Play();
    }
}
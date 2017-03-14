using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class UIMainMenu : MonoBehaviour {
    public Image sound;
    public Sprite yesSound;
    public Sprite noSound;
    public GameObject star1;
    public GameObject star2;
    public AnimationClip clipGoTraineau;
    public AnimationClip clipGoGift;
    public Animator traineau;
    public Animator gifts;
    private bool ok = false;

    void Start()
    {
        ControlManager.GetInstance().CreateEvent("Anim");
        ControlManager.GetInstance().SubscribeToEvent("Anim", AnimOk);

        DataStorer.InitFile("tuto", false.ToString());
        GPlay.instance.onAuthenticateSucess += PostBestScore;
    }

    void PostBestScore()
    {
        GPlay.instance.PostScoreToLeaderboard(int.Parse(DataStorer.Read("prout")));
    }

    public void OnClick(RectTransform rect)
    {
        StartCoroutine(Resize(rect));
    }

    public IEnumerator Resize(RectTransform obj)
    {
        StopCoroutine("Resize");
        Vector3 size = new Vector3(1f, 1f, 1f);
        yield return Anim.AnimateValue(1f, 0.7f, 0.2f, Anim.AccelerateDeccelerateInterpolator,
            (float value, object o) =>
            {
                RectTransform rect = (RectTransform)o;
                rect.localScale = size * value;
            },null, obj);
        yield return Anim.AnimateValue(0.7f, 1f, 0.2f, Anim.AccelerateDeccelerateInterpolator,
            (float value, object o) =>
            {
                RectTransform rect = (RectTransform)o;
                rect.localScale = size * value;
            }, null, obj);
    }

    public void ClickSound()
    {
        if (sound.sprite == yesSound)
            sound.sprite = noSound;
        else
            sound.sprite = yesSound;
    }
    
    public void ClickLag()
    {
        Lagger.UpdateAll(!Lagger.enableLag);
    }

    public void ClickRate()
    {
        Application.OpenURL(UIDeathScreen.URL_ANDROID_STORE);
    }

    public void ClickLeaderBoard()
    {
        if(GPlay.instance != null)
        {
            if (!GPlay.instance.authenticated)
            {
                GPlay.instance.Authenticate((bool success) => 
                {
                    if (success)
                        GPlay.instance.ShowLeaderboard();
                });
            }
            else
            {
                GPlay.instance.ShowLeaderboard();
            }
        }
    }

    public void ClickPlay()
    {
        bool tutoDone = bool.Parse(DataStorer.Read("tuto"));
        if (tutoDone)
            SceneManager.LoadScene("Startup");
        else
        {
            StartCoroutine("GoTuto");
        }
    }
    
    IEnumerator GoTuto()
    {
        traineau.SetInteger("State", 1);
        gifts.SetInteger("State", 2);
        while(!ok)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("Transition tuto");
    }

    void AnimOk(object o)
    {
        ok = true;
    }
}

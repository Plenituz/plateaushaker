using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDeathScreen : MonoBehaviour {
    private bool capture = false;
    private float start;

    public const string URL_ANDROID_STORE = "market://details?id=com.plenituz.plateaushaker";
    public const string URL_FB = "fb://page/1";

    void Start()
    {
        DataStorer.InitFile("game", 0.ToString());

        start = Time.time;
        //check if the best score is beat, if yes post it to gplay if authenticated
        int currentBest = int.Parse(DataStorer.Read("prout"));
        int currentScore = FindObjectOfType<ScoreCounter>().score;
        if(currentScore > currentBest)
        {
            currentBest = currentScore;
            DataStorer.Store("prout", currentScore.ToString());
            GPlay gplay = FindObjectOfType<GPlay>();
            if (gplay.authenticated)
            {
                gplay.PostScoreToLeaderboard(currentScore);
            }
        }
        transform.FindChild("Best score").GetComponent<Text>().text = "Best : " + currentBest;

        int game = int.Parse(DataStorer.Read("game"));
        if(game >= 4)
        {
            DataStorer.Store("game", 0.ToString());
            StartCoroutine(AdManager.instance.ShowGoogleAd());
        }
        else
        {
            game++;
            DataStorer.Store("game", game.ToString());
        }
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
            }, null, obj);
        yield return Anim.AnimateValue(0.7f, 1f, 0.2f, Anim.AccelerateDeccelerateInterpolator,
            (float value, object o) =>
            {
                RectTransform rect = (RectTransform)o;
                rect.localScale = size * value;
            }, null, obj);
    }

    public void ClickShare()
    {
        StartCoroutine(ShareScreenshot());
    }

    public void ClickPouss()
    {
        Application.OpenURL(URL_FB);
    }

    public void ClickRate()
    {
        Application.OpenURL(URL_ANDROID_STORE);
    }

    public void ClickLeaderBoard()
    {
        GPlay gplay = GPlay.instance;
        if (gplay != null)
        {
            if (!gplay.authenticated)
            {
                gplay.Authenticate((bool success) =>
                {
                    if (success)
                        gplay.ShowLeaderboard();
                });
            }
            else
            {
                gplay.ShowLeaderboard();
            }
        }
    }

    public void ClickPlay()
    {
        if(Time.time - start > 1f)
            SceneManager.LoadScene("Menu");
    }

    public IEnumerator ShareScreenshot()
    {
        // wait for graphics to render
        int score = FindObjectOfType<ScoreCounter>().score;
        yield return new WaitForEndOfFrame();
        // create the texture
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);

        // apply
        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

        File.WriteAllBytes(destination, dataToSave);

        if (!Application.isEditor)
        {
            // block to open the file and share it ------------START
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "I saved " + score + " gifts for christmas in Plateau Shaker! "
                 + "http://play.google.com/store/apps/details?id=com.plenituz.plateaushaker");
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            // option one WITHOUT chooser:
            //currentActivity.Call("startActivity", intentObject);

            // option two WITH chooser:
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share the love !");
            currentActivity.Call("startActivity", jChooser);
            // block to open the file and share it ------------END
        }
    }
}

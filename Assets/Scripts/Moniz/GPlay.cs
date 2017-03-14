using UnityEngine;
using System.Collections;
using System;

public class GPlay : MonoBehaviour {
    public static GPlay instance;

    public bool authenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }
    public bool authenticating = false;
    public Action onAuthenticateSucess;

	void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            GooglePlayGames.PlayGamesPlatform.Activate();
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GPGConst.leaderboard_score);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Authenticate(Action<bool> onAuth)
    {
        if (!authenticated && !authenticating)
        {
            //Authenticating...
            Debug.Log("authenticating...");
            authenticating = true;
            Social.localUser.Authenticate((bool success) =>
            {
                authenticating = false;
                if(onAuth != null)
                    onAuth(success);
                if (success && onAuthenticateSucess != null)
                    onAuthenticateSucess();
            });
        }
    }

    public void SignOut()
    {
        if (Social.localUser.authenticated)
        {
            Debug.Log("Signing off");
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
        }
    }

    public void ShowLeaderboard()
    {
        if (authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }

    public void PostScoreToLeaderboard(int score)
    {
        Social.ReportScore(score, GPGConst.leaderboard_score, (bool sucess) =>
        {
            Debug.Log("score posted : " + sucess);
        });
    }
}

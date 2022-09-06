using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{

    public Text text;
    // Start is called before the first frame update

    public GameObject LoginBtn;

    public GameObject LogoutBtn;
    void Awake() // 초기화
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        //처음 인증하는 유저는 로그인 버튼을 보여준다.
        if (!Social.localUser.authenticated) LoginBtn.SetActive(true);
        else //자동 로그인
        {
            StartCoroutine(LoginPost());
            LogoutBtn.SetActive(true);
        }
    }

    public void NewLogin() // 첫 로그인
    {
        LoginBtn.SetActive(false);
        Social.localUser.Authenticate((bool success) =>
                {
                    if (success) StartCoroutine(LoginPost());
                    else text.text = "ERROR";
                });
        LogoutBtn.SetActive(true);
    }

    public void Logout()
    {
        LogoutBtn.SetActive(false);

        text.text = "Logout";
        ((PlayGamesPlatform)Social.Active).SignOut();

        LoginBtn.SetActive(true);

    }

    IEnumerator LoginPost() // DB 요청
    {
        string URL = "http://13.124.34.246/test/join/";
        WWWForm form = new WWWForm();
        form.AddField("id", Social.localUser.userName);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            text.text = www.downloadHandler.text;
        }
        else
        {
            text.text = "DBERROR";
        }
    }

}

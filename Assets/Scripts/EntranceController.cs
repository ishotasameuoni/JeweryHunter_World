using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public int doorNumber; //ドア番号
    public string sceneName; //移行したいシーン名
    public bool opened; //開閉状況

    bool isPlayerTouch; //プレイヤーとの接触状態
    bool announcement; //アナウンス中かどうか]

    GameObject worldUI; //Canvasオブジェクト
    GameObject talkPanel; //TalkPanelオブジェクト
    TextMeshProUGUI messageText;
    World_PlayerController worldPlayerCnt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldPlayerCnt = GameObject.FindGameObjectWithTag("Player").GetComponent<World_PlayerController>();
        worldUI = GameObject.FindGameObjectWithTag("WorldUI");
        talkPanel = worldUI.transform.Find("TalkPanel").gameObject;
        messageText = talkPanel.transform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();

        if (World_UIController.keyOpened != null)
        {
            opened = World_UIController.keyOpened[doorNumber];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerTouch && worldPlayerCnt.IsActionButtonPressed) //プレイヤーの接触とボタンの判定でフラグを検知
        {
            //アナウンス中じゃなければ
            if (!announcement)
            {
                Time.timeScale = 0; //ゲーム進行がストップ
                if (opened) //開錠済み
                {
                    Time.timeScale = 1; //ゲーム進行を再開
                    GameManager.currentDoorNumber = doorNumber; //該当ドア番号をGameManagerで管理する
                    SceneManager.LoadScene(sceneName);
                    return;
                }
                //未開錠の場合
                else if (GameManager.keys > 0) //鍵を持っている
                {
                    SoundManager.currentSoundManager.PlaySE(SEType.DoorOpen); //鍵を開ける音を鳴らす
                    messageText.text = "新たなステージへの扉を開けた！";
                    GameManager.keys--; //鍵を消耗する
                    opened = true; //開錠フラグを立てる
                    World_UIController.keyOpened[doorNumber] = true; //World_UIControllerが所持している開錠の帳簿(KeyOpenedディクショナリー)に開錠したという情報を記録
                    announcement = true;

                    SaveDataManager.SaveGamedata(); //開錠後はセーブする
                }
                else
                {

                    SoundManager.currentSoundManager.PlaySE(SEType.DoorClosed); //鍵を開ける音を鳴らす
                    messageText.text = "鍵が足りません！";
                    announcement = true;
                }
            }
            else //すでにアナウンス中ならannnouncement == true
            {
                Time.timeScale = 1; //ゲーム進行をも戻す
                string s = "";
                if (!opened)
                {
                    s = "(ロック)";
                }
                messageText.text = sceneName + s;
                announcement = false; //アナウンス中のフラグを解除
            }

            //連続入力にならないように一度リセット　※次にボタンが押されるまではfalse
            worldPlayerCnt.IsActionButtonPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触判定をtrueに戻してパネルを表示する
            isPlayerTouch = true;
            talkPanel.SetActive(true);
            string s = "";
            if (!opened)
            {
                s = "(ロック)";
            }
            messageText.text = sceneName + s;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触判定をfalseに戻してパネルを非表示にする
            isPlayerTouch = false;
            if (messageText != null) // NullReferenceExceptionを防ぐ
            {
                talkPanel.SetActive(false);
                Time.timeScale = 1f; // ゲーム進行を再開
            }
        }
    }

}

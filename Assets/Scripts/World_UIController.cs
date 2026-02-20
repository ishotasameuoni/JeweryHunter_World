using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class World_UIController : MonoBehaviour
{
    public static Dictionary<int, bool> keyOpened;

    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (keyOpened == null)
        {
            keyOpened = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceを全点検する
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクトが持っているEntranceControllerを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    keyOpened.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }
            }
        }

        //プレイヤーの位置
        player = GameObject.FindGameObjectWithTag("Player");
        //暫定プレイヤーの位置
        Vector2 currentPlayerPos = Vector2.zero;

        //GameManagerに記録されているcurrentDoorNumberと一致するDoorNumberを持っているEntranceを探す
        for (int i = 0; i < obj.Length; i++)
        {
            //EntranceのEntranceControllerの変数doorNumberが、GameManagerの把握しているcurrentDoorNumberと同じか銅か判別してる
            if (obj[i].GetComponent<EntranceController>().doorNumber == GameManager.currentDoorNumber)
            {
                //暫定プレイヤーの位置を一致したEntranceオブジェクトの位置に置き換える
                currentPlayerPos = obj[i].transform.position;
            }
        }
        //最終的に残ったcurrentPlayerPosの座標がPlayerの座標になる
        player.transform.position = currentPlayerPos;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentKeys != GameManager.keys)
        {
            currentKeys = GameManager.keys;
            keyText.text = currentKeys.ToString();
        }
        if (currentArrows != GameManager.arrows)
        {
            currentArrows = GameManager.arrows;
            arrowText.text = currentArrows.ToString();
        }
    }
}

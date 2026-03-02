using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    GameObject[] objects;
    GameObject player;
    public Sprite itemBoxClose;
    public Sprite itemBoxOpen;
    public static bool isRecover; //アイテム補充完了フラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FindGameObject(s)を用いることで複数ItemBoxを参照することが出来ている
        objects = GameObject.FindGameObjectsWithTag("ItemBox");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<SpriteRenderer>().sprite = itemBoxOpen;
            objects[i].GetComponent<Advent_ItemBox>().isClosed = false;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Playerが存在している、かつ矢の残数が0
        if (player != null && GameManager.arrows <= 0)
        {
            //全ItemBoxのうち、ひとつでもClose状態（アイテムが未取得)なら何もしない
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<Advent_ItemBox>().isClosed)
                {
                    return;
                }
            }

            int index = Random.Range(0, objects.Length); //０番以上　Object配列の数未満のランダム
            objects[index].GetComponent<Advent_ItemBox>().isClosed = true; //未取得の状態にする（閉じた絵にする）
            objects[index].GetComponent<SpriteRenderer>().sprite = itemBoxClose; //閉じた絵に変更
            isRecover = true; //補充済みフラグ
        }
    }
}

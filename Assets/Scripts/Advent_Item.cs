
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AdventItemType
{
    None,
    Arrrow,
    Key,
    Life
}

public class Advent_Item : MonoBehaviour
{
    public AdventItemType type = AdventItemType.None;
    public int numberOfArrow = 10;
    public int recoveryValue = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type) //自分のタイプが対象
            {
                case AdventItemType.Key:
                    GameManager.keys++;
                    //KeyGotに登録(シーン名と鍵を登録しておいたBool型の帳簿を呼び出す)
                    GameManager.keyGot[SceneManager.GetActiveScene().name] = true;
                    break;

                case AdventItemType.Arrrow:
                    GameManager.arrows += numberOfArrow;
                    //一度矢を手に入れたのでまた補充できるようにする
                    ArrowGenerator.isRecover = false;
                    break;

                case AdventItemType.Life:
                    //PlayerController.playerLife + recoveryValue;でも行けるがここではクラスからのメソッド呼び出しで対応
                    PlayerController.PlayerRecovery(recoveryValue);
                    break;

            }
            // アイテムゲット演出
            GetComponent<CircleCollider2D>().enabled = false;      // 当たりを消す
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            rbody.gravityScale = 1.0f; //重力を戻す
            rbody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); // 上に少し跳ね上げる
            Destroy(gameObject, 0.5f); // 1秒後にヒエラルキーからオブジェクトを抹消
        }
    }
}

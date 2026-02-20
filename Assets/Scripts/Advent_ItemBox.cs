using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage;
    public GameObject itemPrefab;
    public bool isClosed = true;
    public AdventItemType type = AdventItemType.Key;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == AdventItemType.Key)
        {
            //GameManagerのKeyGotディクショナリーの該当シーン記録がtrueならば
            if (GameManager.keyGot[SceneManager.GetActiveScene().name]) //Static型のディクショナリー名のKeyGotを呼び出すことで帳簿と一致させる
            {
                isClosed = false;
                //見た目をオープンに変更
                GetComponent<SpriteRenderer>().sprite = openImage;   
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClosed && collision.gameObject.tag == "Player")
        {
            //絵をオープンにして、フラグを解除
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;

            //その場に変数
            if (itemPrefab != null)
            {
                Instantiate(
                    itemPrefab,
                    transform.position,
                    Quaternion.identity);
            }

        }
    }
}

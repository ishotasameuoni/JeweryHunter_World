using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    public float deleteTime = 2; //削除時間
    public int attackPower = 1; //攻撃力


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime); //deleTime後に削除する
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision) //接触処理
    {
        transform.SetParent(collision.transform); //接触したオブジェクトを子にする
        GetComponent<CircleCollider2D>().enabled = false; //当たり判定の無効
        GetComponent<Rigidbody2D>().simulated = false; //物理シミュレーションを無効
    }
}

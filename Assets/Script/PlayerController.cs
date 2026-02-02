using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; //Rigidbody2D型の変数
    float axisH = 0.0f; //入力


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigitbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向の入力をチェックする

        axisH = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        //速度を更新する
        rbody.linearVelocity = new Vector2(axisH * 3.0f, rbody.linearVelocity.y);
    }
}

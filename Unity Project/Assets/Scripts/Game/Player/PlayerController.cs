using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*プレイヤーを操作できるかどうかのフラグ*/
    [SerializeField]
    private bool canMove;
    /*プレイヤーの移動速度*/
    [SerializeField]
    private float speed;
    /*プレイヤーのジャンプ力*/
    [SerializeField]
    private float jumpPower;
    /*地面との接触State*/
    [SerializeField]
    private bool isGrounded;
    /*ジャンプできる状態かどうか*/
    [SerializeField]
    private bool canJump;
    /*ジャンプが連続でできる回数指定*/
    [SerializeField]
    private int canJumpCount;
    /*実際にその場でできる回数*/
    private int jumpCount;
    /*壁キックの横向きの力*/
    [SerializeField]
    private float airJumpPower;
    /*空中浮遊モードの浮力*/
    [SerializeField]
    private float buoyancy;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*ジャンプできる回数がある場合*/
        if (jumpCount > 0)
        {
            /*ジャンプ可能フラグ*/
            canJump = true;
        }
        else
        {
            /*ジャンプ不可*/
            canJump = false;
        }

        /*左右のコマンド入力を取得*/
        var horizontal = Input.GetAxisRaw("Horizontal");

        if(canMove)
        {
            /*キャラクターの移動*/
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
                

        /*ジャンプ可能フラグが立っている場合*/
        if (canJump)
        {
            /*キャラクターのジャンプ*/
            if (Input.GetButtonDown("jump"))
            {
                /*ジャンプ出来る回数を1回減らす*/
                jumpCount -= 1;

                /*ノーマルジャンプ*/
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                }
                /*空中壁キック*/
                else if(!isGrounded)
                {
                    /*空中壁キック時のプレイヤーの制御コルーチンを呼び出す*/
                    StartCoroutine(CantMoveCoroutine());
                    rb.velocity = new Vector2(horizontal * airJumpPower, jumpPower);
                }
            }
        }
        
        if(!isGrounded)
        {
            if(Input.GetButton("Stay"))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / buoyancy);
            }
        }
    }

    /*空中壁キック時のキャラクターの制御*/
    IEnumerator CantMoveCoroutine()
    {
        canMove = false;
        yield return new WaitForSeconds(1.2f);
        canMove = true;
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Ground")
        {
            //ジャンプ回数の初期化
            jumpCount = canJumpCount;
        }
    }

    public void OnTriggerStay2D(Collider2D collider2D)
    {
        /*地面にプレイヤーが接触してる場合*/
        if(collider2D.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        /*プレイヤーが地面から離れた場合*/
        if (collider2D.tag == "Ground")
        {
            isGrounded = false;
        }
    }

}

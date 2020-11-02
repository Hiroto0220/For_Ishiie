using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageSceneController : MonoBehaviour
{
    /*スタート地点と各ステージの座標を管理する配列(オブジェクトと座標)*/
    [SerializeField]
    private GameObject[] stagePointObj;
    private Vector3[] stagePoint;

    /*ユーザーが操作可能な状態かを管理する変数を定義*/
    [SerializeField]
    private bool canOperation;
    /*プレイヤーが移動中かどうかの管理をする変数の定義*/
    [SerializeField]
    private bool isMove = false;

    /*プレイヤーの定義*/
    [SerializeField]
    private GameObject player;
    /*プレイヤーの移動速度の定義*/
    [SerializeField]
    private float playerSpeed;
    /*プレイヤーの位置をVec3で定義*/
    private Vector3 playerPos;
    /*プレイヤーのRigidbodyを定義*/
    private Rigidbody2D playerRb;
    /*選択しているステージを変数で定義*/
    private int stageNo;
    /*目的地の座標をVec3で定義*/
    private Vector3 choiceStagePos;

    /*プレイヤーと移動先までのベクトル*/
    private Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        /*座標の配列の要素数をオブジェクトの要素数にする*/
        stagePoint = new Vector3[stagePointObj.Length];

        /*配列の要素数だけ繰り返す*/
        for(int i = 0; i < stagePoint.Length; i++)
        {
            /*オブジェクトと座標の一致*/
            stagePoint[i] = stagePointObj[i].transform.position;
        }

        /*プレイヤーのRigidbodyを取得*/
        playerRb = player.GetComponent<Rigidbody2D>();
        /*プレイヤーの重力を消す*/
        playerRb.gravityScale = 0;
        /*プレイヤーの初期位置を設定*/
        player.transform.position = stagePoint[0];

        /*ユーザーの操作を有効にする*/
        canOperation = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*プレイヤーの移動中の処理*/
        if(isMove)
        {
            /*目的地につくまでの処理*/
            if (Vector3.Distance(player.transform.position, choiceStagePos) > 0.1 * Mathf.PI)
            {
                Debug.Log("ugoku");
                /*ユーザーの操作を無効にする*/
                canOperation = false;
                /*移動中*/
                playerRb.velocity = moveVec * playerSpeed;
            }
            else
            {
                player.transform.position = choiceStagePos;
                playerRb.velocity = Vector3.zero;
                Debug.Log(stageNo);
                /*移動を終了する*/
                isMove = false;
                /*操作可能にする*/
                canOperation = true;
            }
        }

        /*横入力を取得*/
        var horizontal = Input.GetAxisRaw("Horizontal");

        /*ユーザーの操作有効時のみ実行*/
        if (canOperation)
        {
            /*決定ボタンを押された時に呼び出される*/
            if (Input.GetButtonDown("Submit"))
            {
                /*シーン遷移コルーチンを呼び出す*/
                StartCoroutine(ChangeScene());
            }

            /*プレイヤーの位置をVec3で取得*/
            playerPos = player.transform.position;

            /*右に一定以上の力で入力されたら*/
            if (horizontal > 0.3f)
            {
                /*次にステージがある場合*/
                if(stageNo < stagePoint.Length)
                {
                    /*目的地を次のステージへ*/
                    choiceStagePos = stagePoint[stageNo + 1];
                    /*プレイヤーの位置から次のステージまでのベクトルを出す*/
                    moveVec = (choiceStagePos - playerPos).normalized;
                    /*ステージを1つ進める*/
                    stageNo += 1;
                    //プレイヤーの移動開始
                    MoveChoiceStage();
                }
                
            }
            
            /*左に一定以上の力で入力されたら*/
            else if (horizontal < -0.3f)
            {
                /*前にステージがある場合*/
                if(stageNo > 0)
                {
                    /*目的地を前のステージへ*/
                    choiceStagePos = stagePoint[stageNo - 1];
                    /*プレイヤーの位置から前のステージまでのベクトルを出す*/
                    moveVec = (choiceStagePos - playerPos).normalized;
                    /*ステージを1つ戻す*/
                    stageNo -= 1;
                    //プレイヤーの移動開始
                    MoveChoiceStage();
                }
            }

        }
    }

    /*選んだステージへ移動する関数*/
    private void MoveChoiceStage()
    {
        /*プレイヤーの移動フラグ*/
        isMove = true;
    }

    /*シーン遷移コルーチン*/
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        SceneChanger.LoadSelectingScene(SceneName.GameScene);
    }
}

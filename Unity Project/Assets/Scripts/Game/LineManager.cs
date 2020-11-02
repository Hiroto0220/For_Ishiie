using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;    //オーロラ可視化(仮)    
    [SerializeField]
    private GameObject player;        //プレイヤーオブジェクト
    private Vector3 playerPos;        //プレイヤーの位置
    private Vector3 playerPosOld;     //一つ前のオーロラ生成時のプレイヤーの座標

    [SerializeField]
    private float nextLineDistance;     //オーロラ生成の間隔

    private Vector3[] lineTra = new Vector3[100];   //プレイヤーの位置を追跡(オーロラの位置)
    private GameObject[] effectObj = new GameObject[100];   //出現させたオーロラに番号を振って格納

    private LineRenderer auroraLine;        //オーロラをつなぐライン

    private int lineNum = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがいた位置から一定の距離以上離れたときに呼び出される
        if(Vector2.Distance(lineTra[lineNum], player.transform.position) > nextLineDistance * Mathf.PI)
        {
            //配列最大まで行ったとき
            if(lineNum >= lineTra.Length - 1)
            {
                //直後+1されて0にするために-1を代入
                lineNum = -1;
            }

            lineNum += 1;   //次の配列へ
            lineTra[lineNum] = player.transform.position;   //プレイヤーの位置を格納
            effectObj[lineNum] = effectPrefab;              //effectのオブジェクトを配列に格納
            Instantiate(effectObj[lineNum], lineTra[lineNum], transform.rotation);    //effectのオブジェクトを生成
            auroraLine = effectObj[lineNum].GetComponent<LineRenderer>();       //生成したオブジェクトのラインレンダラーを取得

            if (lineNum == 0 && lineTra[0] != null)
            {
                auroraLine.SetPosition(0, lineTra[lineTra.Length - 1]);
                auroraLine.SetPosition(1, lineTra[lineNum]);
            }
            else
            {
                auroraLine.SetPosition(0, lineTra[lineNum - 1]);
                auroraLine.SetPosition(1, lineTra[lineNum]);
            }
        }
    }
}

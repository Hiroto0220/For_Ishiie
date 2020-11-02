using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]
    private float deleteSeconds;    //オーロラが消えるまでの時間

    [SerializeField]
    private bool canDelete;         //消えれる状態か判定

    // Start is called before the first frame update
    void Start()
    {
        //時間で自動消去されるコルーチンを呼び出す
        StartCoroutine(AutoDelete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //時間で自動消去されるコルーチン
    IEnumerator AutoDelete()
    {
        yield return new WaitForSeconds(deleteSeconds);
        Destroy(gameObject);
    }
}

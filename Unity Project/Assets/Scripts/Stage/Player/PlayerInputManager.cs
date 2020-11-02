using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aurorunnner.Stage.Inputs;
using Aurorunnner.Utilities;

namespace Aurorunnner.Stage.Players
{
    public class PlayerInputManager : MonoBehaviour,IInputManager
    {
        /* -- 左右入力 ----------------------------------*/
        private float moveKey = 0;
        public float MoveKey
        {
            get { return moveKey; }
        }

        /* -- ジャンプ入力 --------------------------------*/
        private int jumpKey = 0;
        public int JumpKey
        {
            get { return jumpKey; }
        }

        /* -- 浮遊入力 -----------------------------------*/
        private int floatingKey = 0;
        public int FloatingKey
        {
            get { return floatingKey; }
        }

        // コンポーネントのオンオフで替えられるように実装
        private void OnEnable()
        {
            Locator<IInputManager>.Bind(this);
        }
        private void OnDisable()
        {
            Locator<IInputManager>.Unbind(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // 移動
            moveKey = Input.GetAxisRaw("Horizontal");

            // ジャンプ
            if (Input.GetButtonDown("Jump"))
            {
                jumpKey = 1;
            }
            else if (Input.GetButton("Jump"))
            {
                jumpKey = 2;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpKey = 0;
            }

            // 浮遊
            if (Input.GetButtonDown("Floating"))
            {
                floatingKey = 1;
            }
            else if (Input.GetButton("Floating"))
            {
                floatingKey = 2;
            }
            else if (Input.GetButtonUp("Floating"))
            {
                floatingKey = 0;
            }
        }
    }
}

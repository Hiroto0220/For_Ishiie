using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aurorunnner.Stage.Inputs
{
    public interface IInputManager
    {
        /* -- 左右入力 --------------------------------------------*/
        float MoveKey { get; }

        /* -- ボタン入力 ------------------------------------------*/
        //jump
        int JumpKey { get; }

        //floating
        int FloatingKey { get; }
    }
}
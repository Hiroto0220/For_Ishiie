using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    AirKicking,
    Damaged,
}

[Serializable]
public struct PlayerParameters
{
    /// <summary>
    /// HP
    /// </summary>
    public int Health;
    /// <summary>
    /// 移動速度
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// ジャンプ中の移動速度
    /// </summary>
    public float JumpMoveSpeed;
    /// <summary>
    /// ジャンプ力
    /// </summary>
    public float JumpPower;
    /// <summary>
    /// 重力
    /// </summary>
    public float GravityRate;
}

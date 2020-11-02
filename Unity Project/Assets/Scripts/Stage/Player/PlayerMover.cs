using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aurorunnner.Utilities;
using Aurorunnner.Stage.Inputs;

namespace Aurorunnner.Stage.Players
{
    /// <summary>
    /// Playerの移動スクリプト
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        private Rigidbody2D rigidbody2d;

        // Jump用変数群
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private float jumpTime = 0.35f;
        private float jumpTimeCounter;
        private float jumpPower;

        //空中制動用の仮変数
        [SerializeField] private float airMoveAcceralation = 1;
        [SerializeField] private float floatingMagnification = 0.5f;

        // 設置判定用のエリア
        private Vector3 groundCheckA, groundCheckB;
        [SerializeField] private LayerMask platformLayer = 0;

        IInputManager inputManager;
        PlayerCore playerCore;

        // State遷移時の初期化処理（そのうちStateMachineになるかも）
        PlayerState currentState
        {
            get { return playerCore.CurrentState; }
            set {
                if(playerCore.CurrentState != value )
                {
                    playerCore.CurrentState = value;
                    switch(playerCore.CurrentState)
                    {
                        case PlayerState.Walking:
                            InitWalking();
                            break;

                        case PlayerState.Jumping:
                            InitJumping();
                            break;

                        case PlayerState.Falling:
                            InitFalling();
                            break;

                        case PlayerState.AirKicking:
                            InitAirKicking();
                            break;
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            inputManager = Locator<IInputManager>.Instance;

            playerCore = GetComponent<PlayerCore>();
            rigidbody2d = GetComponent<Rigidbody2D>();

            // Colliderの判定エリアを取得
            var collider = GetComponent<BoxCollider2D>();
            // コライダーの中心座標へずらす
            groundCheckA = collider.offset;
            groundCheckB = collider.offset;
            // コライダーのbottomへずらす
            groundCheckA.y += -collider.size.y * 0.5f;
            groundCheckB.y += -collider.size.y * 0.5f;
            // 範囲を決定
            Vector2 size = collider.size;
            size.x *= 0.75f;    // 横幅
            size.y *= 0.25f;    // 高さは4分の1
            // コライダーの横幅方向へ左右にずらす
            groundCheckA.x += -size.x * 0.5f;
            groundCheckB.x += size.x * 0.5f;
            // コライダーの高さ方向へ上下にずらす
            groundCheckA.y += -size.y * 0.5f;
            groundCheckB.y += size.y * 0.5f;

            jumpTimeCounter = jumpTime;
        }

        private void Update()
        {
            CheckIsGround();

            switch(currentState)
            {
                case PlayerState.Walking:
                    ChangeLocalScale();
                    Walking();
                    break;

                case PlayerState.Jumping:
                    ChangeLocalScale();
                    Jumping();
                    break;

                case PlayerState.AirKicking:
                    AirKicking();
                    break;

                case PlayerState.Falling:
                    ChangeLocalScale();
                    Falling();
                    break;
            }
        }

        /// <summary>
        /// 接地判定
        /// </summary>
        private void CheckIsGround()
        {
            // ワールド空間の位置へ移動
            var minPosition = groundCheckA + transform.position;
            var maxPosition = groundCheckB + transform.position;
            // minPositionとmaxPositionで指定した範囲内にコライダーが存在するかどうかを判定
            isGrounded = Physics2D.OverlapArea(minPosition, maxPosition, platformLayer);
        }

        /// <summary>
        /// Playerの向きを調整（Drawerが持つかも？）
        /// </summary>
        private void ChangeLocalScale()
        {
            var localScale = transform.localScale;
            if (inputManager.MoveKey != 0)
            {
                // 向きを変える
                if (inputManager.MoveKey > 0)
                {
                    localScale.x = 1;
                }
                else if (inputManager.MoveKey < 0)
                {
                    localScale.x = -1;
                }
                transform.localScale = localScale;
            }
        }

        /// <summary>
        /// 歩き（PlayerState.Walking）
        /// </summary>
        private void InitWalking()
        {
            remainingNumberOfAirkickTime = numberOfAirKickTime;
        }
        private void Walking()
        {
            var velocity = rigidbody2d.velocity;
            velocity.x = playerCore.CurrentParameters.MoveSpeed * inputManager.MoveKey;
            rigidbody2d.velocity = velocity;

            // 落下への移行
            if (!isGrounded)
            {
                currentState = PlayerState.Falling;
            }
            // Jumpへの移行
            else if (inputManager.JumpKey == 1)
            {
                currentState = PlayerState.Jumping;
            }
        }

        [SerializeField] private float airKickVelocity = 5.0f;
        [SerializeField] private float airKickTime = 1.0f;
        [SerializeField] private float airKickAngle = 45f;
        [SerializeField] private int numberOfAirKickTime = 1000;

        private int remainingNumberOfAirkickTime;

        private float airKickTimeCounter = 0;
        /// <summary>
        /// 空中壁キック
        /// </summary>
        private void InitAirKicking()
        {
            airKickTimeCounter = airKickTime;
            remainingNumberOfAirkickTime -= 1;

            var localScale = transform.localScale;
           // localScale.x *= -1;
            transform.localScale = localScale;
        }
        private void AirKicking()
        {
            airKickTimeCounter -= Time.deltaTime;
            Debug.Log(airKickTimeCounter);

            if (inputManager.JumpKey == 2)
            {
                var velocity = rigidbody2d.velocity;
                velocity.x = airKickVelocity * Mathf.Cos(Mathf.Deg2Rad * airKickAngle) * transform.localScale.x;
                velocity.y = airKickVelocity * Mathf.Cos(Mathf.Deg2Rad * airKickAngle);
                rigidbody2d.velocity = velocity;
            }
            // Fallingへの移行
            else if (inputManager.JumpKey == 0)
            {
                currentState = PlayerState.Falling;
            }

            if (airKickTimeCounter <= 0)
            {
                currentState = PlayerState.Falling;
                Debug.Log("Falling");
            }
        }

        /// <summary>
        /// ジャンプ(PlayerState.Jumping)
        /// </summary>
        private void InitJumping()
        {
            jumpTimeCounter = jumpTime;
            jumpPower = playerCore.CurrentParameters.JumpPower;
        }
        private void Jumping()
        {
            jumpTimeCounter -= Time.deltaTime;

            if (inputManager.JumpKey == 2)
            {
                jumpPower -= jumpTime * playerCore.CurrentParameters.JumpPower * Time.deltaTime;
                var velocity = rigidbody2d.velocity;
                velocity.x = playerCore.CurrentParameters.JumpMoveSpeed * inputManager.MoveKey;
                velocity.y = jumpPower;
                rigidbody2d.velocity = velocity;
            }
            // AirKickingへの移行
            else if(inputManager.JumpKey == 1　&& remainingNumberOfAirkickTime > 0)
            {
                currentState = PlayerState.AirKicking;
            }
            // Fallingへの移行
            else　if (inputManager.JumpKey == 0)
            {
                currentState = PlayerState.Falling;
            }

            if (jumpTimeCounter <= 0)
            {
                currentState = PlayerState.Falling;
            }
        }

        /// <summary>
        /// 落下（PlayerState.Falling）
        /// </summary>
        private void InitFalling()
        {

        }
        private void Falling()
        {
            var velocity = rigidbody2d.velocity;
            velocity.x += playerCore.CurrentParameters.JumpMoveSpeed * airMoveAcceralation * inputManager.MoveKey * Time.deltaTime;

            if (velocity.x >= playerCore.CurrentParameters.JumpMoveSpeed)
            {
                velocity.x = playerCore.CurrentParameters.JumpMoveSpeed;
            }
            else if (velocity.x <= -playerCore.CurrentParameters.JumpMoveSpeed)
            {
                velocity.x = -playerCore.CurrentParameters.JumpMoveSpeed;
            }
            velocity.y -= playerCore.CurrentParameters.GravityRate * Time.deltaTime;
            if(inputManager.FloatingKey >= 1)
            {
                velocity.y *= floatingMagnification;
            }
            rigidbody2d.velocity = velocity;

            // AirKickingへの移行
            if (inputManager.JumpKey == 1 && remainingNumberOfAirkickTime > 0)
            {
                currentState = PlayerState.AirKicking;
            }

            if (isGrounded)
            {
                currentState = PlayerState.Walking;
            }
        }
    }
}

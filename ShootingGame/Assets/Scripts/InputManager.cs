using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// プレイヤーの入力管理クラス
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;　　// PlayerInput アセット
    public PlayerInput.OnFootActions onFoot;　　// OnFoot アクションマップ

    private PlayerMotor motor;　　// 移動管理

    private PlayerLook look;　　// 視点管理
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;　　// "OnFoot" アクションマップを取得
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();  // ジャンプ入力

        onFoot.Crouch.performed += ctx => motor.Crouch();　　// しゃがみ入力

        // スプリント入力
        onFoot.Sprint.performed += ctx => motor.OnSprint(ctx);
        onFoot.Sprint.canceled += ctx => motor.OnSprint(ctx);
    }

    void FixedUpdate()
    {
        // 移動入力の取得と PlayerMotor に渡す
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        // 視点入力の取得と PlayerLook に渡す
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();　　// 入力アクションの有効化
    }
    private void OnDisable()
    {
        onFoot.Disable();　　// 入力アクションの無効化
    }
}

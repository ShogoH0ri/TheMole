using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Pathway : MonoBehaviour  // 敵キャラクターが巡回するためのポイント管理クラス
{
    public List<Transform> waypoints;　　// 巡回するポイントのリスト
    [SerializeField]
    private bool alwaysDrawPath;　　// 常にGizmosでパスを表示するか
    [SerializeField]
    private bool drawAsLoop;　　// ループとして線を描くか
    [SerializeField]
    private bool drawNumbers;　　 // ポイント番号を表示するか
    public Color debugColour = Color.white;　　//Gizmosの色

#if UNITY_EDITOR
    public void OnDrawGizmos()　　//シーンビューでGizmosを常に描画
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }
    public void DrawPath()　　//ウェイポイントを線で繋いで可視化
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();　　// ラベルのスタイル設定
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;
            if (drawNumbers)
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

            // 線を前のポイントと繋ぐ
            if (i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);

                // ループ指定なら最後と最初も繋ぐ
                if (drawAsLoop)
                    Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);

            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        if (alwaysDrawPath)
            return;
        else
            DrawPath();
    }
#endif
}
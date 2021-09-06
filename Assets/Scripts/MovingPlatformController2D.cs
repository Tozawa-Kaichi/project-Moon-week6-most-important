using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動く床を制御するコンポーネント
/// </summary>
public class MovingPlatformController2D : MonoBehaviour
{
    /// <summary>振り幅(x)</summary>
    public float m_amplitudeX = 1f;
    /// <summary>振り幅(y)</summary>
    public float m_amplitudeY = 0f;
    /// <summary>動く速さ</summary>
    public float m_speedX = 2.0f;
    /// <summary>動く速さ</summary>
    public float m_speedY = 2.0f;
    /// <summary>横移動のためのタイマー</summary>
    private float m_timerX;
    /// <summary>縦移動のためのタイマー</summary>
    private float m_timerY;
    /// <summary>初期位置</summary>
    private Vector2 m_initialPosition;

    void Start()
    {
        // 初期位置を記憶しておく
        m_initialPosition = this.transform.position;
    }

    void Update()
    {
        // 三角関数を使ってオブジェクトの位置を往復させる
        m_timerX += Time.deltaTime * m_speedX;
        m_timerY += Time.deltaTime * m_speedY;
        float posX = Mathf.Sin(m_timerX) * m_amplitudeX;
        float posY = Mathf.Sin(m_timerY) * m_amplitudeY;
        Vector2 pos = m_initialPosition;
        pos = pos + new Vector2(posX, posY);
        transform.position = pos;
    }

    // プレイヤーが上に乗った時、それを子オブジェクトとすることによりプレイヤーはオブジェクトと一緒に動く
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.transform.SetParent(transform);
    }

    // プレイヤーがオブジェクトから離れた時は、親子関係を解除する
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.transform.SetParent(null);
    }
}

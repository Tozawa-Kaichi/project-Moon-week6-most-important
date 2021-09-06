using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2D Platformer を動かすためのコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlatformerPlayerController2D : MonoBehaviour
{
    /// <summary>横移動する力</summary>
    [SerializeField] float m_runPower = 1f;
    /// <summary>ジャンプする力</summary>
    [SerializeField] float m_jumpPower = 1f;
    /// <summary>（横移動の）最大速度</summary>
    [SerializeField] float m_maxSpeed = 1f;
    /// <summary>横移動が入力されていない時の減速係数</summary>
    [SerializeField] float m_breakCoeff = 0.9f;
    /// <summary>どのレイヤーを「地面」として判定するか（ジャンプのための接地判定をするレイヤー）</summary>
    [SerializeField] LayerMask m_groundMask = 0;
    /// <summary>接地判定を自分の位置からどれくらい下まで判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1f;
    /// <summary>左右の入力値</summary>
    float h;
    /// <summary>上下の入力値</summary>
    float v;
    /// <summary>初期位置 落下した時ここに戻る</summary>
    Vector2 m_initialPosition;
    Rigidbody2D m_rb2d;
    SpriteRenderer m_sprite;
    Animator m_anim;

    void Start()
    {
        m_initialPosition = this.transform.position;
        m_rb2d = GetComponent<Rigidbody2D>();
        m_sprite = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 上下左右の入力を受け付ける
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // 左右の入力により画像を反転させる
        if (h != 0)
        {
            m_sprite.flipX = (h < 0);
        }

        // 力学的に上昇している時は接地とみなさない
        //if ()
        //{
        //    return false;
        //}

        // ジャンプを制御する
        if (Input.GetButtonDown("Jump"))
        {
            // 接地していたらジャンプする
            if ((m_rb2d.velocity.y <= 0f) && IsGrounded())  // 上昇中には接地していてもジャンプさせない
            {
                m_rb2d.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        // 入力に応じて物理挙動を制御する
        if (h == 0)
        {
            // 左右の入力がない時は減速する
            if (m_rb2d.velocity.x != 0)
            {
                Vector2 v = m_rb2d.velocity;
                v.x = v.x * m_breakCoeff;
                m_rb2d.velocity = v;
            }
        }
        else
        {
            // 入力がある時は、最大速度になるまで加速する
            if (h > 0 ? m_rb2d.velocity.x < m_maxSpeed : -1 * m_rb2d.velocity.x < m_maxSpeed)
            {
                m_rb2d.AddForce(Vector2.right * m_runPower * h, ForceMode2D.Force);
            }
        }
    }

    void LateUpdate()
    {
        // 落下した時はミスとする
        if (this.transform.position.y < -50f)
        {
            Miss();
        }

        // アニメーションを制御する
        if (m_anim)
        {
            bool isGrounded = IsGrounded();
            m_anim.SetBool("IsGrounded", isGrounded);

            if (isGrounded)
            {
                m_anim.SetFloat("RunSpeed", Mathf.Abs(m_rb2d.velocity.x));
            }
            else
            {
                m_anim.SetFloat("SpeedY", m_rb2d.velocity.y);
            }
        }
    }

    /// <summary>
    /// Raycast により接地判定する。接地している＝地面の上に立っている＝ジャンプ可能、とする。
    /// LayerMask していることに注意すること。
    /// </summary>
    /// <returns>接地している⇒true, 空中にいる⇒false</returns>
    bool IsGrounded()
    {
        bool isGrounded = false;    // isGrounded: 接地しているか

        // Ray を確認するためにシーンに線を描く
        Debug.DrawLine(this.transform.position, this.transform.position + Vector3.down * m_isGroundedLength);
        // Raycast する
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, m_isGroundedLength, m_groundMask);
        
        // Ray が地面に接触していたら true を返す
        if (hit.collider)
        {
            isGrounded = true;
        }

        return isGrounded;
    }

    /// <summary>
    /// ミスした時に呼ばれる。
    /// </summary>
    void Miss()
    {
        // 初期位置に戻す
        this.transform.position = m_initialPosition;
    }
}

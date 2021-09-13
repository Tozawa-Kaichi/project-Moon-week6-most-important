using UnityEngine;

/// <summary>
/// 攻撃を制御するコンポーネント
/// </summary>
public class PlayerAttackController : MonoBehaviour
{
    /// <summary>攻撃範囲の中心</summary>
    [SerializeField] Vector2 m_offset = Vector2.right;
    /// <summary>攻撃範囲の大きさ</summary>
    [SerializeField] Vector2 m_size = Vector2.one;
    Animator m_anim = default;
    SpriteRenderer m_sprite = default;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_anim.SetBool("Attack", true);
        }
    }

    /// <summary>
    /// 攻撃範囲の gizmo を表示する（選択時のみ）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 centerOfAttackRange = (Vector2) this.transform.position + m_offset;

        // 実行中は、左を向いている時は左に gizmo を表示する
        if (m_sprite)
        {
            centerOfAttackRange = (Vector2)this.transform.position + m_offset * (m_sprite.flipX ? -1 : 1);
        }

        Gizmos.DrawWireCube(centerOfAttackRange, m_size);
    }

    /// <summary>
    /// 攻撃する。Animation Event から呼び出すことを前提に作っている。
    /// </summary>
    void Attack()
    {
        Vector2 centerOfAttackRange = (Vector2)this.transform.position + m_offset * (m_sprite.flipX ? -1 : 1);
        var cols = Physics2D.OverlapBoxAll(centerOfAttackRange, m_size, 0);

        foreach (var c in cols)
        {
            var destruct = c.gameObject.GetComponent<Destructable>();
            
            if (destruct)
            {
                Debug.Log($"Hit {c.name}");
                destruct.Hit();
            }
        }

        m_anim.SetBool("Attack", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

/// <summary>
/// ゴールに着いたことを検知するコンポーネント
/// </summary>
public class GoalController : MonoBehaviour
{
    /// <summary>ゴールに着いた時に表示する Text</summary>
    [SerializeField] Text m_goalText = null;
    /// <summary>ゴールしたかどうかを判定するフラグ。ゴール判定を一度しかやりたくないので用意した。</summary>
    bool isFinished = false;
    /// <summary>ゴールに着いた時に演出として再生する Timeline</summary>
    [SerializeField] PlayableDirector m_goalTimeline = null;

    void Start()
    {
        // テキスト表示を消す
        if (m_goalText)
        {
            m_goalText.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが触れたらゴールの演出をする
        if (collision.tag == "Player")
        {
            if (!isFinished)
            {
                isFinished = true;

                if (m_goalTimeline)
                {
                    m_goalTimeline.gameObject.SetActive(true);
                    m_goalTimeline.Play();
                }
                else if (m_goalText)
                {
                    m_goalText.enabled = true;
                }
                else
                {
                    // 演出がない時はログを出力する
                    Debug.Log("Goal!");
                }
            }
        }
    }
}

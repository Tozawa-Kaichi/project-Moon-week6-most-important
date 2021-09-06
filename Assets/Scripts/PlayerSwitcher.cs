using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player（タグが付いたオブジェクト）の有効・無効を切り替える
/// Timeline から Signal Emitter を使って呼び出されるために作った
/// </summary>
public class PlayerSwitcher : MonoBehaviour
{
    GameObject m_player;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TurnOff()
    {
        Debug.Log("Turn player off");
        if (m_player)
        {
            m_player.SetActive(false);
        }
    }

    public void TurnOn()
    {
        Debug.Log("Turn player on");
        if (m_player)
        {
            m_player.SetActive(true);
        }
    }
}

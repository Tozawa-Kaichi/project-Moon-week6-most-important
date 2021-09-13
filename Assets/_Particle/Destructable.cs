using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] GameObject m_effect = default;

    public void Hit()
    {
        if (m_effect)
        {
            Instantiate(m_effect, this.transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}

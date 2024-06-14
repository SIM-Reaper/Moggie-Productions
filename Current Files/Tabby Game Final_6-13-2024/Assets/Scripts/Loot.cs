using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private Collider2D lootCollider;

    public void Initialize(ItemData itemData, Vector3 scale, float delay)
    {
        lootCollider = GetComponent<Collider2D>();
        if (lootCollider != null)
        {
            lootCollider.enabled = false;
        }

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemData.icon;
            spriteRenderer.sortingOrder = 10;
        }

        transform.localScale = scale;
        StartCoroutine(EnableColliderAfterDelay(delay));
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (lootCollider != null)
        {
            lootCollider.enabled = true;
        }
    }
}

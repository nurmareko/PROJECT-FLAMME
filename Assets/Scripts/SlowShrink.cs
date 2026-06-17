using UnityEngine;

public class SlowShrink : MonoBehaviour
{
    public float shrinkDuration = 12f;  // total seconds until fully gone
    public float startDelay = 0f;       // optional delay before shrinking starts

    private Vector3 startScale;
    private float elapsed;

    void Start()
    {
        startScale = transform.localScale;
        elapsed = -startDelay;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed < 0f) return;

        float t = Mathf.Clamp01(elapsed / shrinkDuration);
        transform.localScale = startScale * (1f - t);

        if (t >= 1f)
            gameObject.SetActive(false); // fully gone
    }
}
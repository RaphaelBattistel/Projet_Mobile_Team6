using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;

public class Ivy : MonoBehaviour, IWater
{
    public void DoWaterInteraction()
    {
        if (isAnimating || _sprite.size.y == scaleLimit) return;
        StartCoroutine(ScaleIvy(Mathf.Clamp(_sprite.size.y + 1, 0, scaleLimit)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController player) && collision.CompareTag("Player"))
        {
            StartCoroutine(StartElevate(player));
        }
    }

    [SerializeField] private Transform _plateformVisual;

    private IEnumerator StartElevate(CharacterController player)
    {
        player.StartMoving = false;

        float elapsed = 0f;

        float verticalOffset = player.transform.position.y - _plateformVisual.transform.position.y;

        float newPos;
        float startPos = _plateformVisual.transform.localPosition.y;
        while (elapsed < movePlayerDuration)
        {
            float ratio = elapsed / movePlayerDuration;

            newPos = Mathf.Lerp(startPos, _sprite.size.y, ratio);
            _plateformVisual.transform.localPosition = new Vector3(0, newPos, 0);
            player.transform.position = _plateformVisual.transform.position + Vector3.up * verticalOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.StartMoving = true;

        elapsed = 0f;
        yield return new WaitForSeconds(5f);
        while (elapsed < movePlayerDuration)
        {
            float ratio = elapsed / movePlayerDuration;

            newPos = Mathf.Lerp(_sprite.size.y, startPos, ratio);
            _plateformVisual.transform.localPosition = new Vector3(0, newPos, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    [SerializeField] private float scaleLimit;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _plateformEndVisual;

    [SerializeField] private float movePlayerDuration = 1f;
    [SerializeField] private float animDuration = 1f;
    bool isAnimating = false;

    private IEnumerator ScaleIvy(float goal)
    {
        isAnimating = true;

        float elapsed = 0f;

        float newScale;
        float startScale = _sprite.size.y;
        while (elapsed < animDuration)
        {
            float ratio = elapsed / animDuration;

            newScale = Mathf.Lerp(startScale, goal, ratio);
            _sprite.size = new Vector2(_sprite.size.x, newScale);
            _plateformEndVisual.localPosition = new Vector3(0, newScale, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
    }

    private void Start()
    {
        scaleLimit = Mathf.Abs(transform.position.y - Physics2D.Raycast(transform.position + transform.up / 2, transform.up, 10, GridManager.Instance.Ground).point.y);
        StartCoroutine(ScaleIvy(Mathf.Clamp(_sprite.size.y + 1, 0, scaleLimit)));
    }
}

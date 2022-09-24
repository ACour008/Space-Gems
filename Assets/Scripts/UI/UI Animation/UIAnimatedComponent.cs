using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Tweens;

public class UIAnimatedComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private EffectBuilder effectBuilder;
    private ObjectPool<UIAnimatedComponent> pool;
    private RectTransform canvasRectTransform;
    private Transform parent;

    public TextMeshProUGUI ScoreText { get => scoreText; }


    private void EffectBuilder_OnExecutionCompleted(object sender, System.EventArgs e)
    {
        transform.SetParent(parent, false);
        this.pool.Release(this);
    }

    public void Initialize(EffectBuilder effectBuilder, ObjectPool<UIAnimatedComponent> pool, Transform parent)
    {
        canvasRectTransform = GetComponent<RectTransform>();
        this.effectBuilder = effectBuilder;
        this.pool = pool;
        this.parent = parent;

        effectBuilder.OnExecutionCompleted += EffectBuilder_OnExecutionCompleted;
    }

    private void OnDestroy()
    {
        effectBuilder.OnExecutionCompleted -= EffectBuilder_OnExecutionCompleted;
    }

    public void Reset(Vector3 resetPosition, Color resetColor)
    {
        canvasRectTransform.anchoredPosition3D = resetPosition;
        scoreText.color = resetColor;
    }

    public void Run(Vector3 startingPosition, Color startingColor, string uiText)
    {
        Debug.Log("Hurry up Run!");
        canvasRectTransform.anchoredPosition3D = startingPosition;
        scoreText.color = startingColor;
        scoreText.text = uiText;

        effectBuilder.ExecuteAll();
    }
}

using UnityEngine;

namespace MiiskoWiiyaas.Core
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] protected Sprite[] sprites;
        [SerializeField] protected Sprite[] powerSprites;
        [SerializeField] protected float[] gemValues;
        [SerializeField] protected int currentCellId;
        [SerializeField] protected GemType type;
        [SerializeField] protected GemColor color;
        [SerializeField] protected SpriteRenderer powerUpRenderer;
        protected float value;

        protected GemAnimator gemAnimator;
        protected SpriteRenderer spriteRenderer;

        public bool AnimationCompleted { get => gemAnimator.Completed; }
        public int CurrentCellId { get => currentCellId; set => currentCellId = value; }
        public GemType Type { get => type; }
        public GemColor Color { get => color; }
        public float Value { get => value; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public float Disappear(GemCell currentCell, bool selfOnly = true)
        {
            return gemAnimator.Disappear(currentCell, selfOnly);
        }

        public float Move(Vector3 targetPosition)
        {
            float runTime = gemAnimator.Move(targetPosition);
            return runTime;
        }

        public void SetAnimator(GemAnimator newAnimator)
        {
            gemAnimator = newAnimator;
        }

        public void SetGemColor(GemColor newColor)
        {
            color = newColor;

            if (type == GemType.NORMAL)
            {
                spriteRenderer.sprite = sprites[(int)color];
                value = gemValues[(int)color];
                powerUpRenderer.gameObject.SetActive(false);
            }
        }

        public void SetGemType(GemType newType)
        {
            type = newType;

            if (type != GemType.NORMAL)
            {
                powerUpRenderer.sprite = powerSprites[(int)type - 1];
                value *= 2;

                powerUpRenderer.gameObject.SetActive(true);
            }
        }

        protected void SetSprite(Sprite newSprite) => spriteRenderer.sprite = newSprite;

        protected void SetValue(float value) => this.value = value;

        public override string ToString() => color.ToString();
    }
}

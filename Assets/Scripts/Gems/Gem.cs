using UnityEngine;

namespace MiiskoWiiyaas.Core
{
    /// <inheritdoc cref="GemAnimator"/>
    public class Gem : MonoBehaviour
    {
        [SerializeField] protected Sprite[] sprites;
        [SerializeField] protected Sprite[] powerSprites;
        [SerializeField] protected int[] gemValues;
        [SerializeField] protected int currentCellId;
        [SerializeField] protected GemType type;
        [SerializeField] protected GemColor color;
        [SerializeField] protected SpriteRenderer powerUpRenderer;
        protected int value;

        protected GemAnimator gemAnimator;
        protected SpriteRenderer spriteRenderer;

        public bool AnimationCompleted { get => gemAnimator.Completed; }
        public int CurrentCellId { get => currentCellId; set => currentCellId = value; }
        public GemType Type { get => type; }
        public GemColor Color { get => color; }
        public int Value { get => value; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <inheritdoc cref="GemAnimator.Disappear(GemCell, bool)"/>
        public float Disappear(GemCell currentCell, bool selfOnly = true)
        {
            return gemAnimator.Disappear(currentCell, selfOnly);
        }

        /// <inheritdoc cref="GemAnimator.Move(Vector3)"/>
        public float Move(Vector3 targetPosition)
        {
            return gemAnimator.Move(targetPosition);
        }

        /// <summary>
        /// Sets the GemAnimator object (use for when a Gem turns to a power Gem, for example).
        /// </summary>
        /// <param name="newAnimator">A new GemAnimator object.</param>
        /// <seealso cref="GemAnimator"/>
        public void SetAnimator(GemAnimator newAnimator)
        {
            gemAnimator = newAnimator;
        }

        /// <summary>
        /// Sets the color of the Gem.
        /// </summary>
        /// <param name="newColor">An enum containing all Gem color types.</param>
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

        /// <summary>
        /// Sets the type of Gem (Normal, Electric Power Gem, etc...)
        /// </summary>
        /// <param name="newType">An enum containing all Gem types</param>
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

        protected void SetValue(int value) => this.value = value;

        public override string ToString() => color.ToString();
    }
}

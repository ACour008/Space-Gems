using UnityEngine;
using MiiskoWiiyaas.Core;

namespace MiiskoWiiyaas.Builders
{
    public class GemBuilder : IBuilder<Gem, GemCell>
    {
        private GameObject prefab;
        private GemSelector gemSelector;
        private int rows;

        public GemBuilder(GameObject gemPrefab, int rows)
        {
            prefab = gemPrefab;
            this.rows = rows;
            gemSelector = new GemSelector(new System.Random());
        }

        public Gem Build(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();
            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;


            GemColor randomColor = gemSelector.GetRandomColor(id, rows, cells, 0, 6);
            newGem.SetGemColor(randomColor);
            newGem.SetGemType(GemType.NORMAL);
            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";
            
            return newGem;
        }

        public Gem BuildFromLayout(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells, GemColor[] gemColorGrid)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();

            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;
            newGem.SetGemColor(gemColorGrid[id]);
            newGem.SetGemType(GemType.NORMAL);

            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";

            return newGem;
        }

        public Gem BuildWithColor(int id, float xPosition, float yPosition, Transform parent, GemCell[] cells, GemColor color)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem newGem = instance.GetComponent<Gem>();

            newGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            newGem.CurrentCellId = id;
            newGem.SetGemColor(color);
            newGem.SetGemType(GemType.NORMAL);

            newGem.SetAnimator(new NormalGemAnimator(newGem));

            instance.name = $"{newGem.Color}";
            return newGem;
        }

        public Gem BuildPowerGem(int id, float xPosition, float yPosition, Transform parent, GemColor gemColor)
        {
            GameObject instance = GameObject.Instantiate<GameObject>(prefab, parent);
            Gem powerGem = instance.GetComponent<Gem>();

            powerGem.transform.localPosition = new Vector3(xPosition, yPosition, 0);
            powerGem.CurrentCellId = id;

            powerGem.SetGemColor(gemColor);
            powerGem.SetGemType((GemType)UnityEngine.Random.Range(1, 3));

            // Factory.
            GemAnimator animator = (powerGem.Type == GemType.BOMB) ? new BombGemAnimator(powerGem) : new ElectricGemAnimator(powerGem);

            powerGem.SetAnimator(animator);

            instance.name = $"{powerGem.Color}";

            return powerGem;
        }
    }
}

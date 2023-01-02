using UnityEngine;

/// <summary>
/// интерфейс взаимодействия с объектом
/// </summary>
public interface IInteractionWithObject
{
    /// <summary>
    /// взаимодействовать с объектом
    /// </summary>
    public void InteractWithObject();
}


/// <summary>
/// обнаружение объекта
/// </summary>
public class DetectionObject : IInteractionWithObject
{
    public Character character; // персонаж


    /// <summary>
    /// конструктор обнаружения дерева
    /// </summary>
    /// <param name="character">персонаж</param>
    public DetectionObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// взаимодействовать с объектом
    /// </summary>
    public void InteractWithObject()
    {
        if (character.characteristics.type == "Woodman")
        {
            Wood wood = DetectWood();

            if (wood == null)
                character.workObject = null;
            else
                character.workObject = wood.gameObject;
        }
        else
            character.workObject = null;
    }

    /// <summary>
    /// обнаружить дерево
    /// </summary>
    /// <returns>ближайшее дерево</returns>
    private Wood DetectWood()
    {
        Wood[] woods = Object.FindObjectsOfType<Wood>();
        woods = System.Array.FindAll(woods, wood => !wood.isStump);

        if (woods.Length > 0)
        {
            Wood detectedWood = woods[0];

            foreach (Wood wood in woods)
            {
                float detectedWoodDistance = Vector2.Distance(character.transform.position, detectedWood.transform.position);
                float woodDistance = Vector2.Distance(character.transform.position, wood.transform.position);

                if (woodDistance < detectedWoodDistance)
                    detectedWood = wood;
            }

            return detectedWood;
        }
        else
            return null;
    }
}

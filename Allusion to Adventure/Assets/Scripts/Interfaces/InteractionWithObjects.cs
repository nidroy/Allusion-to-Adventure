using UnityEngine;

/// <summary>
/// ��������� �������������� � ��������
/// </summary>
public interface IInteractionWithObject
{
    /// <summary>
    /// ����������������� � ��������
    /// </summary>
    public void InteractWithObject();
}


/// <summary>
/// ����������� �������
/// </summary>
public class DetectionObject : IInteractionWithObject
{
    public Character character; // ��������


    /// <summary>
    /// ����������� ����������� ������
    /// </summary>
    /// <param name="character">��������</param>
    public DetectionObject(Character character)
    {
        this.character = character;
    }


    /// <summary>
    /// ����������������� � ��������
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
    /// ���������� ������
    /// </summary>
    /// <returns>��������� ������</returns>
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

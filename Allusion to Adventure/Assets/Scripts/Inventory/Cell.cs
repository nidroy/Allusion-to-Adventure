using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������
/// </summary>
public class Cell : MonoBehaviour
{
    public int id; // �������������

    public Image image; // �����������

    public Item item; // �������

    public List<Sprite> sprites; // ��������� ����������� ������


    private void Update()
    {
        UpdateImage();
    }


    /// <summary>
    /// �������� �����������
    /// </summary>
    private void UpdateImage()
    {
        if (item == null)
            image.sprite = sprites[0];
        else
        {
            if (item.gameObject.activeInHierarchy)
                image.sprite = sprites[1];
            else
            {
                if (gameObject.tag == "Weapon")
                    image.sprite = sprites[2];
                else if (gameObject.tag == "Armor")
                    image.sprite = sprites[3];
                else
                    image.sprite = sprites[0];
            }
        }
    }
}

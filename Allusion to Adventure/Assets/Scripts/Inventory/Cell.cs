using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// €чейка
/// </summary>
public class Cell : MonoBehaviour
{
    public int id; // идентификатор

    public Image image; // изображение

    public Item item; // предмет

    public List<Sprite> sprites; // возможные изображени€ €чейки


    private void Update()
    {
        UpdateImage();
    }


    /// <summary>
    /// обновить изображение
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

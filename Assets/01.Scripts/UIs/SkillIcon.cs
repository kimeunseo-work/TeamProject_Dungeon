using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public Image iconImage;

    public void SetUp(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.Log("sprite is null");
            return;
        }
        iconImage.sprite = sprite;
    }
}

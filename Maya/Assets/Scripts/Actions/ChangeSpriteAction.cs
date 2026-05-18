using UnityEngine;

[CreateAssetMenu(fileName = "NewSpriteAction", menuName = "Actions/Sprite")]
public class ChangeSpriteAction : InteractableAction
{
    public override bool Execute(InteractableObject owner)
    {
        var data = owner.GetPhaseData();

        if (data == null || data.otherSprite == null) return true;

        SpriteRenderer renderer = owner.GetComponent<SpriteRenderer>();

        if (renderer.sprite != data.otherSprite)
        {
            renderer.sprite = data.otherSprite;
        }
        else
        {
            renderer.sprite = data.initialSprite;
        }

        return true;
    }
}

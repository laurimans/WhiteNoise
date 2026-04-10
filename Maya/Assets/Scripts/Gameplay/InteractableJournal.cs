using UnityEngine;

public class InteractableJournal : InteractableObject
{
    [SerializeField] private JournalUI journal;
    public override void OnObjectClicked()
    {
        base.OnObjectClicked();

        if (!this.GetPhaseData().activateOtherItem)
        {
            journal.OpenJournal();
        }
    }
}

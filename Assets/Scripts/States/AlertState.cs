using System;

public class AlertState : BaseAdditionalState
{
    [Inject]
    public AlertPopup AlertPopup { get; private set; }

    public string Text { get; set; }

    public Action<AlertResult> ResultHandler { get; set; }

    public override void Load()
    {
        base.Load();

        AlertPopup.OkClicked += OnOkClicked;
        AlertPopup.CancelClicked += OnCancelClicked;

        AlertPopup.SetText(Text);

        AlertPopup.Show();
    }

    public override void Unload()
    {
        base.Unload();

        AlertPopup.OkClicked -= OnOkClicked;
        AlertPopup.CancelClicked -= OnCancelClicked;

        AlertPopup.Hide();
    }

    protected override void OnHardwareBackPress()
    {
        OnCancelClicked();
    }

    private void OnOkClicked()
    {
        StateMachine.Unload(false);

        if (ResultHandler != null)
            ResultHandler(AlertResult.Ok);
    }

    private void OnCancelClicked()
    {
        StateMachine.Unload(false);

        if (ResultHandler != null)
            ResultHandler(AlertResult.Cancel);
    }
}
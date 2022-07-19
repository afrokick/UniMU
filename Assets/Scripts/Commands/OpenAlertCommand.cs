using System;

public class OpenAlertCommand : LoadAdditionalStateCommand<AlertState>
{
	[Inject]
	public string Text { get; private set; }
    [Inject]
    public Action<AlertResult> ResultReceived { get; private set; }

	public override void Execute ()
	{
		var inst = injectionBinder.GetInstance<AlertState>();

		inst.Text = Text;
        inst.ResultHandler = ResultReceived;

		StateMachine.Load(inst, false);
	}
}
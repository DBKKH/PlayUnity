using System;

public class VariousCodes
{
	public string CalcTime(Action action)
	{
		var sw = new System.Diagnostics.Stopwatch();

		sw.Start();
		action.Invoke();
		sw.Stop();

		var actionName = action.Method.Name + "が処理Aにかかった時間";
		TimeSpan ts = sw.Elapsed;
		var totalTime = $"　{ts}";

		return $"{actionName} \n {totalTime}";
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	Coroutine co;

	protected override void Init()
	{
        base.Init();

		SceneType = Define.Scene.Game;

		Managers.UI.ShowSceneUI<UI_Inven>();

		//co = StartCoroutine("CoExplodeAfterSeconds", 4.0f);
		//StartCoroutine("CoStopExplode", 2.0f);

		Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

		gameObject.GetOrAddComponent<CursorController>();
	}

	IEnumerator CoStopExplode(float seconds)
	{
		Debug.Log("Stop Enter");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Stop!!!!!!!!!!!!!");
		if (co != null)
		{
			StopCoroutine(co);
			co = null;
		}
	}

	IEnumerator CoExplodeAfterSeconds(float seconds)
	{
		Debug.Log("Explode Enter");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Explode!!!!!!!!!!!!!");
		co = null;
	}

	public override void Clear()
	{
		
	}
}

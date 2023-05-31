﻿
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class ESBtn_BigAwakeSystem : AwakeSystem<ESBtn_Big,Transform> 
	{
		protected override void Awake(ESBtn_Big self,Transform transform)
		{
			self.uiTransform = transform;
		}
	}


	[ObjectSystem]
	public class ESBtn_BigDestroySystem : DestroySystem<ESBtn_Big> 
	{
		protected override void Destroy(ESBtn_Big self)
		{
			self.DestroyWidget();
		}
	}
}

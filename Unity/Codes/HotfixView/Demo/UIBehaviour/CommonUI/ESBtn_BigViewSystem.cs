
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class ESBtn_BigAwakeSystem : AwakeSystem<ESBtn_Big,Transform> 
	{
		public override void Awake(ESBtn_Big self,Transform transform)
		{
			self.uiTransform = transform;
		}
	}


	[ObjectSystem]
	public class ESBtn_BigDestroySystem : DestroySystem<ESBtn_Big> 
	{
		public override void Destroy(ESBtn_Big self)
		{
			self.DestroyWidget();
		}
	}
}

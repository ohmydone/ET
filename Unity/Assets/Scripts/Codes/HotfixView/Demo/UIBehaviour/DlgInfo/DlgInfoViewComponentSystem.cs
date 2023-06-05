
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgInfoViewComponentAwakeSystem : AwakeSystem<DlgInfoViewComponent> 
	{
		protected override void Awake(DlgInfoViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgInfoViewComponentDestroySystem : DestroySystem<DlgInfoViewComponent> 
	{
		protected override void Destroy(DlgInfoViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}


using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class DlgGMViewComponentAwakeSystem : AwakeSystem<DlgGMViewComponent> 
	{
		protected override void Awake(DlgGMViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgGMViewComponentDestroySystem : DestroySystem<DlgGMViewComponent> 
	{
		protected override void Destroy(DlgGMViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}

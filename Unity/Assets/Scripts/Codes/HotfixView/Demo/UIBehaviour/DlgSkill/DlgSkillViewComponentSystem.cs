
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgSkillViewComponentAwakeSystem : AwakeSystem<DlgSkillViewComponent> 
	{
		protected override void Awake(DlgSkillViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgSkillViewComponentDestroySystem : DestroySystem<DlgSkillViewComponent> 
	{
		protected override void Destroy(DlgSkillViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}

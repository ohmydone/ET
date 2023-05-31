
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_ItemSkillDestroySystem : DestroySystem<Scroll_ItemSkill> 
	{
		protected override void Destroy( Scroll_ItemSkill self )
		{
			self.DestroyWidget();
		}
	}
}

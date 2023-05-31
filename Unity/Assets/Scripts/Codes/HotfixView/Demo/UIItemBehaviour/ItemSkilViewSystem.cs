
using ET.Client;
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_ItemSkilDestroySystem : DestroySystem<Scroll_ItemSkil> 
	{
		protected override void Destroy( Scroll_ItemSkil self )
		{
			self.DestroyWidget();
		}
	}

	public static class Scroll_ItemSkilSystem
	{
		public static void BindSkill(this Scroll_ItemSkil self,Spell spell)
		{
			self.ELab_NameText.text = spell.Config.Name;
		}
	}
}


using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_ItemGMDestroySystem : DestroySystem<Scroll_ItemGM> 
	{
		protected override void Destroy( Scroll_ItemGM self )
		{
			self.DestroyWidget();
		}
	}
}

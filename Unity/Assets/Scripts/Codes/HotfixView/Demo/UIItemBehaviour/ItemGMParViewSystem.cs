
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_ItemGMParDestroySystem : DestroySystem<Scroll_ItemGMPar> 
	{
		protected override void Destroy( Scroll_ItemGMPar self )
		{
			self.DestroyWidget();
		}
	}
	
}

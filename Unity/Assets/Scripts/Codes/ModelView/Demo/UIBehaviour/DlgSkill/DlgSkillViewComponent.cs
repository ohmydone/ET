
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgSkillViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.LoopHorizontalScrollRect ELSV_SkillLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELSV_SkillLoopHorizontalScrollRect == null )
     			{
		    		this.m_ELSV_SkillLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"Panel/ELSV_Skill");
     			}
     			return this.m_ELSV_SkillLoopHorizontalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_ELSV_SkillLoopHorizontalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.LoopHorizontalScrollRect m_ELSV_SkillLoopHorizontalScrollRect = null;
		public Transform uiTransform = null;
	}
}

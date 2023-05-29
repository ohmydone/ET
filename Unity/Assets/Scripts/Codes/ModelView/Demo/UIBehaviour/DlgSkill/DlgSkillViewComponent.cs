
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgSkillViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.LoopHorizontalScrollRect ELoopScrollList_SkillLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELoopScrollList_SkillLoopHorizontalScrollRect == null )
     			{
		    		this.m_ELoopScrollList_SkillLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"Panel/ELoopScrollList_Skill");
     			}
     			return this.m_ELoopScrollList_SkillLoopHorizontalScrollRect;
     		}
     	}

		public UnityEngine.UI.Text EKeyText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EKeyText == null )
     			{
		    		this.m_EKeyText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Panel/ELoopScrollList_Skill/Content/ItemSkil/EKey");
     			}
     			return this.m_EKeyText;
     		}
     	}

		public UnityEngine.UI.Text ENameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ENameText == null )
     			{
		    		this.m_ENameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Panel/ELoopScrollList_Skill/Content/ItemSkil/EName");
     			}
     			return this.m_ENameText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_ELoopScrollList_SkillLoopHorizontalScrollRect = null;
			this.m_EKeyText = null;
			this.m_ENameText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.LoopHorizontalScrollRect m_ELoopScrollList_SkillLoopHorizontalScrollRect = null;
		private UnityEngine.UI.Text m_EKeyText = null;
		private UnityEngine.UI.Text m_ENameText = null;
		public Transform uiTransform = null;
	}
}

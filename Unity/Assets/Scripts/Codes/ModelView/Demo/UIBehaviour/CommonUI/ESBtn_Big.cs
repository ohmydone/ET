
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[EnableMethod]
	public  class ESBtn_Big : Entity,ET.IAwake<UnityEngine.Transform>,IDestroy 
	{
		public UnityEngine.UI.Button EBtn_1Button
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBtn_1Button == null )
     			{
		    		this.m_EBtn_1Button = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EBtn_1");
     			}
     			return this.m_EBtn_1Button;
     		}
     	}

		public UnityEngine.UI.Image EBtn_1Image
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBtn_1Image == null )
     			{
		    		this.m_EBtn_1Image = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EBtn_1");
     			}
     			return this.m_EBtn_1Image;
     		}
     	}

		public UnityEngine.UI.Text ELab_1Text
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELab_1Text == null )
     			{
		    		this.m_ELab_1Text = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EBtn_1/ELab_1");
     			}
     			return this.m_ELab_1Text;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EBtn_1Button = null;
			this.m_EBtn_1Image = null;
			this.m_ELab_1Text = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EBtn_1Button = null;
		private UnityEngine.UI.Image m_EBtn_1Image = null;
		private UnityEngine.UI.Text m_ELab_1Text = null;
		public Transform uiTransform = null;
	}
}

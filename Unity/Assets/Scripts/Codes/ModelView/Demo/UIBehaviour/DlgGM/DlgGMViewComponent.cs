
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgGMViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.InputField EInp_GmInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInp_GmInputField == null )
     			{
		    		this.m_EInp_GmInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"Panel/EInp_Gm");
     			}
     			return this.m_EInp_GmInputField;
     		}
     	}

		public UnityEngine.UI.Image EInp_GmImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInp_GmImage == null )
     			{
		    		this.m_EInp_GmImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/EInp_Gm");
     			}
     			return this.m_EInp_GmImage;
     		}
     	}

		public UnityEngine.UI.Button EBtn_SureButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBtn_SureButton == null )
     			{
		    		this.m_EBtn_SureButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/EBtn_Sure");
     			}
     			return this.m_EBtn_SureButton;
     		}
     	}

		public UnityEngine.UI.Image EBtn_SureImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBtn_SureImage == null )
     			{
		    		this.m_EBtn_SureImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/EBtn_Sure");
     			}
     			return this.m_EBtn_SureImage;
     		}
     	}

		public UnityEngine.UI.Text ELab_SureText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELab_SureText == null )
     			{
		    		this.m_ELab_SureText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Panel/EBtn_Sure/ELab_Sure");
     			}
     			return this.m_ELab_SureText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EInp_GmInputField = null;
			this.m_EInp_GmImage = null;
			this.m_EBtn_SureButton = null;
			this.m_EBtn_SureImage = null;
			this.m_ELab_SureText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.InputField m_EInp_GmInputField = null;
		private UnityEngine.UI.Image m_EInp_GmImage = null;
		private UnityEngine.UI.Button m_EBtn_SureButton = null;
		private UnityEngine.UI.Image m_EBtn_SureImage = null;
		private UnityEngine.UI.Text m_ELab_SureText = null;
		public Transform uiTransform = null;
	}
}

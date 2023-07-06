using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class DlgGM: Entity, IAwake, IUILogic
    {
        public DlgGMViewComponent View
        {
            get => this.Parent.GetComponent<DlgGMViewComponent>();
        }

        public GMConfig ChooseGM;
        public List<int> Gms;
        public Dictionary<int, Scroll_ItemGM> ScrollItemGMs;
        public Dictionary<int, Scroll_ItemGMPar> ScrollItemGMPars;
    }
}
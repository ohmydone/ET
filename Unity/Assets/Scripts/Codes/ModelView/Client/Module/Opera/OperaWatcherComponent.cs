using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class OperaWatcherComponent: Entity, IAwake,ILoad,IDestroy
    {
        [StaticField]
        public static OperaWatcherComponent Instance;
        public TypeSystems typeSystems;
        public UnOrderMultiMap<object, OperaSystemAttribute> typeMapAttr;
        
        public List<Entity> OperaEntitys;
        
        public LinkedList<Tuple<object,Entity,string>> sortList;
    }
}
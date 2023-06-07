namespace ET
{
    [Invoke(TimerType.RemoveBuff)]
    public class RemoveBuff: ATimer<Buff>
    {
        protected override void Run(Buff self)
        {
            try
            {
                if(self==null||self.IsDisposed) return;
                self.GetParent<BuffComponent>().Remove(self.Id);
            }
            catch (System.Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [FriendOf(typeof(BuffComponent))]
    [ObjectSystem]
    public class BuffAwakeSystem : AwakeSystem<Buff,int,long,long>
    {
        protected override void Awake(Buff self,int id,long timestamp,long sourceId)
        {
            self.HandleAddLogic(id, timestamp, sourceId);
        }
    }
    [FriendOf(typeof(BuffComponent))]
    [ObjectSystem]
    public class BuffAwakeSystem1 : AwakeSystem<Buff,int,long,bool,long>
    {
        protected override void Awake(Buff self,int id,long timestamp,bool ignoreLogic,long sourceId)
        {
            self.HandleAddLogic(id, timestamp, sourceId, ignoreLogic);
        }
    }
    [FriendOf(typeof(BuffComponent))]
    [ObjectSystem]
    public class BuffDestroySystem : DestroySystem<Buff>
    {
        protected override void Destroy(Buff self)
        {
            TimerComponent.Instance.Remove(ref self.TimerId);
            Log.Info("移除BUFF id="+self.ConfigId);
            var buffComp = self.GetParent<BuffComponent>();
            var unit = buffComp.unit;
            for (int i = 0; i < self.Config.Type.Count; i++)
            {
                if (self.Config.Type[i] == BuffSubType.Attribute) //结束后是否移除加成（0:是）
                {
                    if(self.AttrConfig.IsRemove == 0)
                        self.RemoveBuffAttrValue(unit);
                }
                else if (self.Config.Type[i] == BuffSubType.ActionControl)
                {
                    self.RemoveBuffActionControl(unit);
                }
                else if (self.Config.Type[i] == BuffSubType.Bleed)
                {
                    self.RemoveComponent<BuffBleedComponent>();
                }
                else if (self.Config.Type[i] == BuffSubType.Chant)
                {
#if SERVER//单机去掉
                    self.Parent.GetParent<CombatUnitComponent>().GetComponent<SpellComponent>().Interrupt();
#endif
                }
            }
        }
    }

    [FriendOf(typeof(Buff))]
    [FriendOf(typeof(BuffComponent))]
    public static class BuffSystem
    {
        /// <summary>
        /// 刷新时间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="timestamp"></param>
        public static void RefreshTime(this Buff self,long timestamp)
        {
            if(timestamp<=self.Timestamp) return;
            if (self.Timestamp >= 0)
            {
                TimerComponent.Instance.Remove(ref self.Timestamp);
            }
            self.Timestamp = timestamp;
            self.TimerId = TimerComponent.Instance.NewOnceTimer(self.Timestamp, TimerType.RemoveBuff, self);
        }
        /// <summary>
        /// 处理添加buff
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <param name="sourceId"></param>
        /// <param name="ignoreLogic"></param>
        public static void HandleAddLogic(this Buff self,int id,long timestamp,long sourceId,bool ignoreLogic=false)
        {
            Log.Info("添加BUFF id="+id);
            self.ConfigId = id;
            self.Timestamp = timestamp;
            self.FromUnitId = sourceId;
            var buffComp = self.GetParent<BuffComponent>();
            var unit = buffComp.unit;
            if (!ignoreLogic)//忽略逻辑处理
            {
                for (int i = 0; i < self.Config.Type.Count; i++)
                {
                    if (self.Config.Type[i] == BuffSubType.Attribute)
                    {
                        self.AddBuffAttrValue(unit);
                    }
                    else if (self.Config.Type[i] == BuffSubType.ActionControl)
                    {
                        self.AddBuffActionControl(unit);
                    }
                    else if (self.Config.Type[i] == BuffSubType.Bleed)
                    {
                        self.AddComponent<BuffBleedComponent,int>(self.ConfigId);
                    }
                    else if (self.Config.Type[i] == BuffSubType.Chant)
                    {
                        // if(self.BuffChantConfig.MoveInterrupt == 1)
                        //     self.GetParent<BuffComponent>().unit.Stop(0);
                    }
                }
            }

            if(timestamp>=0)
                self.TimerId = TimerComponent.Instance.NewOnceTimer(timestamp, TimerType.RemoveBuff, self);
        }
        
        /// <summary>
        /// 添加BUFF属性加成
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit"></param>
        public static void AddBuffAttrValue(this Buff self, Unit unit)
        {
            if (self.AttrConfig.AttributeType != null)
            {
                var numc = unit.GetComponent<NumericComponent>();
                if (numc != null)
                {
                    for (int i = 0; i < self.AttrConfig.AttributeType.Count; i++)
                    {
                        if (NumericType.Map.TryGetValue(self.AttrConfig.AttributeType[i], out var attr))
                        {
                            if (self.AttrConfig.AttributeAdd != null && self.AttrConfig.AttributeAdd.Count > i)
                                numc.Set(attr * 10 + 2, numc.GetAsInt(attr * 10 + 2) + self.AttrConfig.AttributeAdd[i]);
                            if (self.AttrConfig.AttributePct != null && self.AttrConfig.AttributePct.Count > i)
                                numc.Set(attr * 10 + 3, numc.GetAsInt(attr * 10 + 3) + self.AttrConfig.AttributePct[i]);
                            if (self.AttrConfig.AttributeFinalAdd != null && self.AttrConfig.AttributeFinalAdd.Count > i)
                                numc.Set(attr * 10 + 4,
                                    numc.GetAsInt(attr * 10 + 4) + self.AttrConfig.AttributeFinalAdd[i]);
                            if (self.AttrConfig.AttributeFinalPct != null && self.AttrConfig.AttributeFinalPct.Count > i)
                                numc.Set(attr * 10 + 5,
                                    numc.GetAsInt(attr * 10 + 5) + self.AttrConfig.AttributeFinalPct[i]);
                        }
                        else
                        {
                            Log.Info("BuffConfig属性没找到 【" + self.AttrConfig.AttributeType[i]+"】");
                        }
                    }
                }
                else
                {
                    Log.Error("添加BUFF id= " + unit.Id + " 时没找到 NumericComponent 组件");
                }
            }
        }
        /// <summary>
        /// 移除BUFF属性加成
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit"></param>
        public static void RemoveBuffAttrValue(this Buff self,Unit unit)
        {
            if (self.AttrConfig.AttributeType != null)
            {
                var numc = unit.GetComponent<NumericComponent>();
                if (numc != null)
                {
                    for (int i = 0; i < self.AttrConfig.AttributeType.Count; i++)
                    {
                        if (NumericType.Map.TryGetValue(self.AttrConfig.AttributeType[i], out var attr))
                        {
                            if (self.AttrConfig.AttributeAdd != null && self.AttrConfig.AttributeAdd.Count > i)
                                numc.Set(attr * 10 + 2, numc.GetAsInt(attr * 10 + 2) - self.AttrConfig.AttributeAdd[i]);
                            if (self.AttrConfig.AttributePct != null && self.AttrConfig.AttributePct.Count > i)
                                numc.Set(attr * 10 + 3, numc.GetAsInt(attr * 10 + 3) - self.AttrConfig.AttributePct[i]);
                            if (self.AttrConfig.AttributeFinalAdd != null && self.AttrConfig.AttributeFinalAdd.Count > i)
                                numc.Set(attr * 10 + 4,
                                    numc.GetAsInt(attr * 10 + 4) - self.AttrConfig.AttributeFinalAdd[i]);
                            if (self.AttrConfig.AttributeFinalPct != null && self.AttrConfig.AttributeFinalPct.Count > i)
                                numc.Set(attr * 10 + 5,
                                    numc.GetAsInt(attr * 10 + 5) - self.AttrConfig.AttributeFinalPct[i]);
                        }
                        else
                        {
                            Log.Info("BuffConfig属性没找到 【" + self.AttrConfig.AttributeType[i]+"】");
                        }
                    }
                }
                else
                {
                    Log.Error("移除BUFF id= " + self.ConfigId + " 时没找到 NumericComponent 组件");
                }
                
            }
        }


        /// <summary>
        /// 添加行为禁制
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit"></param>
        public static void AddBuffActionControl(this Buff self, Unit unit)
        {
            var buffComp = self.GetParent<BuffComponent>();
            if (self.ActionControlConfig.ActionControl != null)
            {
                for (int i = 0; i < self.ActionControlConfig.ActionControl.Count; i++)
                {
                    var type = self.ActionControlConfig.ActionControl[i];
                    if (!buffComp.ActionControls.ContainsKey(type)||buffComp.ActionControls[type]==0)
                    {
                        buffComp.ActionControls[type] = 1;
                        // Log.Info("BuffWatcherComponent");
                        BuffWatcherComponent.Instance.SetActionControlActive(type,true,unit);
                    }
                    else
                    {
                        buffComp.ActionControls[type]++;
                    }
                }
            }
        }

        /// <summary>
        /// 移除行为禁制
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit"></param>
        public static void RemoveBuffActionControl(this Buff self, Unit unit)
        {
            var buffComp = self.GetParent<BuffComponent>();
            if (self.ActionControlConfig.ActionControl != null)
            {
                for (int i = 0; i < self.ActionControlConfig.ActionControl.Count; i++)
                {
                    var type = self.ActionControlConfig.ActionControl[i];
                    if (buffComp.ActionControls.ContainsKey(type)&&buffComp.ActionControls[type]>0)
                    {
                        buffComp.ActionControls[type]--;
                        if (buffComp.ActionControls[type] == 0)
                        {
                            // Log.Info("BuffWatcherComponent");
                            BuffWatcherComponent.Instance.SetActionControlActive(type,false,unit);
                        }
                    }
                }
            }
        }
    }
}
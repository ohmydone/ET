﻿using Unity.Mathematics;

namespace ET
{
    [FriendOf(typeof(ZhuiZhuAimComponent))]
    public class AI_ZhuiZhu: AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            Unit myUnit = aiComponent.GetParent<Unit>();
            Log.Info("Check");
            if (myUnit == null)
            {
                Log.Info("myUnit == null");
                return 1;
            }
            ZhuiZhuAimComponent zhuiZhuAimPathComponent = myUnit.GetComponent<ZhuiZhuAimComponent>();
            if (zhuiZhuAimPathComponent == null||zhuiZhuAimPathComponent.Aim==null)
            {
                Log.Info("zhuiZhuAimPathComponent == null||zhuiZhuAimPathComponent.Aim==null");
                return 2;
            }

            return 0;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit myUnit = aiComponent.GetParent<Unit>();
            if (myUnit == null)
            {
                return;
            }
            
            Log.Info("开始追逐");
            
            ZhuiZhuAimComponent zhuiZhuAimPathComponent = myUnit.GetComponent<ZhuiZhuAimComponent>();
            while (zhuiZhuAimPathComponent?.Aim!=null)
            {
                float3 nextTarget = zhuiZhuAimPathComponent.Aim.Position;
#if DOTNET //纯客户端单机游戏自己在客户端接入Recast库后去掉
                myUnit.FindPathMoveToAsync(nextTarget,cancellationToken).Coroutine(); 
                await TimerComponent.Instance.WaitAsync(100,cancellationToken);
#else
                //myUnit.MoveToAsync(nextTarget, cancellationToken).Coroutine(); 
                await TimerComponent.Instance.WaitAsync(10,cancellationToken);
#endif
               
                if(math.length(nextTarget-myUnit.Position)<0.1f) 
                    zhuiZhuAimPathComponent.Arrived();
            }
        }
    }
}
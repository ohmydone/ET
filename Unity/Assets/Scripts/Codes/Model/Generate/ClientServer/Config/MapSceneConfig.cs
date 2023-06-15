//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET
{

public sealed partial class MapSceneConfig: Bright.Config.BeanBase
{
    public MapSceneConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        Name = _buf.ReadString();
        Area = _buf.ReadString();
        Recast = _buf.ReadString();
        PostInit();
    }

    public static MapSceneConfig DeserializeMapSceneConfig(ByteBuf _buf)
    {
        return new MapSceneConfig(_buf);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// UnityScene名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 加载的Area数据表名（不需要做无缝则不填）
    /// </summary>
    public string Area { get; private set; }
    /// <summary>
    /// 寻路文件名
    /// </summary>
    public string Recast { get; private set; }

    public const int __ID__ = -455406574;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, IConfigSingleton> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Area:" + Area + ","
        + "Recast:" + Recast + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
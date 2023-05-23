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
   
[Config]
public partial class StartSceneConfigCategory: ConfigSingleton<StartSceneConfigCategory>
{
    private readonly Dictionary<int, StartSceneConfig> _dataMap;
    private readonly List<StartSceneConfig> _dataList;
    
    public StartSceneConfigCategory(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, StartSceneConfig>();
        _dataList = new List<StartSceneConfig>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            StartSceneConfig _v;
            _v = StartSceneConfig.DeserializeStartSceneConfig(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }
    
    public bool Contain(int id)
    {
        return _dataMap.ContainsKey(id);
    }

    public Dictionary<int, StartSceneConfig> GetAll()
    {
        return _dataMap;
    }
    
    public List<StartSceneConfig> DataList => _dataList;

    public StartSceneConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public StartSceneConfig Get(int key) => _dataMap[key];
    public StartSceneConfig this[int key] => _dataMap[key];

    public override void Resolve(Dictionary<string, IConfigSingleton> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public override void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    public override void TrimExcess()
    {
        _dataMap.TrimExcess();
        _dataList.TrimExcess();
    }
    
    
    public override string ConfigName()
    {
        return typeof(StartSceneConfig).Name;
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
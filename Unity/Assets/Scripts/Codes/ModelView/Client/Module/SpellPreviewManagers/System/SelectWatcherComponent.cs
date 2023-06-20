using System;
using System.Collections.Generic;
namespace ET.Client
{
	using OneTypeSystems = UnOrderMultiMap<Type, object>;

	[ComponentOf(typeof(Scene))]
	public sealed class SelectWatcherComponent:Entity,IAwake,ILoad
	{
		[StaticField]
		public static SelectWatcherComponent Instance;
		public TypeSystems typeSystems;
	}
}
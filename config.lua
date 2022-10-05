return {
	DefaultSettings = 
	{
		[1] = 
		{
			Description = "开启",
			DisplayName = "后端修复",
			Key = "On",
			DefaultValue = true,
			SettingType = "Toggle"
		}
	},
	Source = 1,
	Title = "个人临时Bug修复",
	BackendPlugins = 
	{
		[1] = "BackendFix1.dll"
	},
	Author = "xyzkljl1",
	Description = "v1.0.4,对应游戏版本v0.0.25.1\n个人用bug修复，内容很少，只会修复我力所能及的部分\n如果官方修复了bug则会移除对应内容\n\n修复：\n1.功法书读到最后一页不显示效率，技艺书读完仍然显示效率的bug\n2.修复练功房不加读书效率bug",
	FrontendPlugins = 
	{
		[1] = "FrontendFix1.dll"
	},
	FileId = 2871708452,
	Version = "1.0.4.2022.10.6"
}
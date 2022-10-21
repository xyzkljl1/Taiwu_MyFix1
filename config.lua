return {
	DefaultSettings = 
	{
		[1] = 
		{
			Description = "开启",
			DisplayName = "后端修复(All)",
			Key = "BackOn",
			DefaultValue = true,
			SettingType = "Toggle"
		}
		,
		[2] = 
		{
			Description = "开启",
			DisplayName = "前端修复(暂无)",
			Key = "FrontOn",
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
	Description = "v1.1.2,对应游戏版本v0.0.32\n个人用bug修复，内容很少，只会修复我力所能及的部分\n如果官方修复了bug则会移除对应内容\n代码https://github.com/xyzkljl1/Taiwu_MyFix1\n提建议或问题请去讨论区！！在评论区没法单独回复你\n\n修复：\n4.门派支持的正逆练读书效率加成不正确的bug\n",
	FrontendPlugins = 
	{
		[1] = "FrontendFix1.dll"
	},
	FileId = 2871708452,
	Version = "1.1.2"
}
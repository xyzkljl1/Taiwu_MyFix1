return {
	Title = "个人Bug修复合集",
	Version = "1.0.2.2022.10.1",
	Author = "xyzkljl1",	
	FrontendPlugins = { 
		[1] = "FrontendFix1.dll"
	},
	Description = "仅用于v0.0.19,用于绕过(而非修复)特定bug",
	["DefaultSettings"] = {
		[1] = {
			["Key"] = "FrontendFix1On",
			["SettingType"] = "Toggle",
			["DisplayName"] = "修复属性界面",
			["Description"] = "进入属性界面切换，退出到主菜单再进入属性界面吃食物触发红字修复",
			["DefaultValue"] = true,
			},
	},
}
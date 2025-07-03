# TicTacToe Unity 项目

## 项目简介

本项目是一个基于 Unity 引擎开发的井字棋（TicTacToe）游戏，支持人机对战（AI）、~~玩家对战~~等多种模式。项目采用模块化设计，包含棋盘管理、游戏逻辑、AI（极大极小算法）、UI 管理等核心模块，适合学习 Unity 游戏开发、AI 算法实现与 UI 交互。

---

## 主要特性

- 支持玩家对战与人机对战（AI 可配置强度）
- AI 使用极大极小（Minimax）算法，支持 Alpha-Beta 剪枝
- 直观的 UI 界面，启动即可开玩，支持鼠标操作、回合指示、胜负提示
- 代码结构清晰，易于扩展和维护

---

## 目录结构

```text
.
+-Assets/ 
|  +--Scripts/
|  |  +--Core/   # 核心数据结构与算法（棋盘、AI、规则等）
|  |  +--Game/   # 游戏管理、棋盘管理、棋子逻辑 
|  |  +--UI/     # UI 管理、拖拽、设置面板等
|  +--Prefabs/   # 预制体资源
...
```



---

## 主要模块说明

- **GameManager**：游戏主控，负责游戏流程、回合切换、AI 调用等。
- **BoardManager**：棋盘管理，负责棋子生成、位置管理、棋盘清理等。
- **Minimax**：AI 算法实现，支持可配置强度的极大极小搜索。
- **UIManager**：UI 控制，负责回合指示、胜负提示、设置面板等。
- **Rules**：游戏规则判定，判断胜负、连线等。
- **GameBlock**：棋子逻辑，响应鼠标事件、显示高亮等。

---

## 快速开始

### 1. 克隆项目

```shell
git clone https://github.com/yuriyoung/tictactoe.git
```

### 2. 用 Unity 打开项目

- 推荐 Unity 版本：2021.3 或更高
- 打开 `Assets/Scenes/` 下的主场景

### 3. 运行游戏

- 点击 Unity 编辑器顶部的 `Play` 按钮即可开始游戏

### 4. 打包发布

1. 打开 `File > Build Settings`
2. 选择 `PC, Mac & Linux Standalone`，Target Platform 选 `Windows`
3. 添加主场景到 `Scenes In Build`
4. 点击 `Build`，选择输出目录，生成 exe 和 Data 文件夹

---

## 主要操作说明

- [x] **开始新游戏**：点击 UI 按钮或调用 `UIManager.StartNewGame()`
- [x] **退出游戏**：点击 UI 按钮或调用 `UIManager.ExitGame()`
- [x] **游戏操作**：鼠标左键点击棋盘的方块
- [x] **切换 AI 强度**：通过设置面板的 CheckBox 控制，调用 `UIManager.SetSupperAIEnabled(bool)`
- [ ] ~~**拖拽 UI**：支持鼠标拖拽设置面板等 UI 元素~~

---

## 依赖与注意事项

- 开发平台Unity 2022.3.51f1
- 依赖 [TextMeshPro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest) 组件
- 需将所有用到的场景添加到 Build Settings
- 资源需放在 `Assets/` 目录下并被引用
- 若打包后出现黑屏/闪退，请检查主场景设置和资源引用

---

## 代码风格与扩展建议

- 采用 C# 9.0，.NET Framework 4.7.1
- 推荐使用 MonoBehaviourSingleton 模式管理全局单例
- AI、规则、UI 等均为可扩展模块，便于二次开发

---

## 联系与反馈

如有建议或 bug 反馈，请通过 Issue 或 Pull Request 提交。

---

**祝你游戏愉快！**
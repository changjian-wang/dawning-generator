# Dawning Generator

> 🚀 类似 ABP.io 的项目脚手架生成器，基于 Dawning SDK 快速创建 .NET 微服务项目。

## ✨ 功能特性

- 📦 **多种架构模式**: 支持分层架构和简单架构
- 🔧 **灵活配置**: 可选择数据库类型、SDK 模块和可选功能
- 💾 **历史记录**: 登录后可保存项目生成历史
- ⭐ **模板收藏**: 保存常用配置为模板快速复用
- 🎨 **现代 UI**: 基于 Vue 3 + Arco Design 的前端界面

## 🛠️ 技术栈

### 后端
- .NET 8.0 / ASP.NET Core
- Scriban 模板引擎
- Dapper + MySQL
- Dawning SDK (Identity, Core, Logging)

### 前端
- Vue 3 + TypeScript
- Arco Design Vue
- Pinia + Vue Router

## 🚀 快速开始

### 环境要求

- .NET SDK 8.0+
- Node.js 18+
- MySQL 8.0+

### 后端运行

```bash
# 初始化数据库
mysql -u root -p < scripts/init.sql

# 运行 API
cd src/Dawning.Generator.Api
dotnet run
```

API 地址: http://localhost:5000

### 前端运行

```bash
cd web
npm install
npm run dev
```

前端地址: http://localhost:5173

## 📁 项目结构

```
dawning-generator/
├── src/
│   ├── Dawning.Generator.Api/           # Web API
│   ├── Dawning.Generator.Application/   # 应用层
│   ├── Dawning.Generator.Domain/        # 领域层
│   └── Dawning.Generator.Infrastructure/ # 基础设施层
├── templates/                            # 项目模板
│   ├── layered/                         # 分层架构模板
│   ├── simple/                          # 简单架构模板
│   └── shared/                          # 共享模板片段
├── web/                                  # Vue 前端
├── scripts/                              # 脚本
│   └── init.sql                         # 数据库初始化
└── tests/                                # 测试
```

## 📖 API 文档

启动后访问 Swagger: http://localhost:5000/swagger

### 主要端点

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | /api/generator/generate | 生成项目 |
| GET | /api/generator/options | 获取配置选项 |
| GET | /api/history | 获取生成历史 |
| POST | /api/favorites | 创建收藏模板 |

## 📄 License

MIT

---

*Powered by Dawning SDK*

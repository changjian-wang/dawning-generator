// API 相关类型

export interface GenerateProjectRequest {
  projectName: string
  namespacePrefix?: string
  architectureType: ArchitectureType
  dotNetVersion: DotNetVersion
  databaseType: DatabaseType
  selectedModules: string[]
  includeDocker: boolean
  includeGitHubActions: boolean
  includeUnitTests: boolean
}

export interface GenerateProjectResponse {
  projectId: string
  projectName: string
  downloadUrl: string
  fileSize: number
  generatedFiles: string[]
  generatedAt: string
}

export interface ProjectHistory {
  id: string
  userId: string
  projectName: string
  namespacePrefix?: string
  architectureType: string
  dotNetVersion: string
  databaseType: string
  selectedModules: string[]
  includeDocker: boolean
  includeGitHubActions: boolean
  includeUnitTests: boolean
  downloadCount: number
  createdAt: string
  updatedAt: string
}

export interface TemplateFavorite {
  id: string
  userId: string
  name: string
  description?: string
  config: GenerateProjectRequest
  isDefault: boolean
  createdAt: string
  updatedAt: string
}

// 枚举类型
export type ArchitectureType = 'Layered' | 'Clean' | 'Simple'
export type DotNetVersion = 'Net8' | 'Net9'
export type DatabaseType = 'MySQL' | 'PostgreSQL' | 'SqlServer' | 'SQLite'

// 模块选项
export const MODULE_OPTIONS = [
  { value: 'Authentication', label: '认证授权', icon: 'icon-lock' },
  { value: 'Logging', label: '日志记录', icon: 'icon-file' },
  { value: 'Caching', label: '缓存', icon: 'icon-storage' },
  { value: 'Swagger', label: 'Swagger', icon: 'icon-code' },
  { value: 'HealthCheck', label: '健康检查', icon: 'icon-heart' },
  { value: 'AutoMapper', label: 'AutoMapper', icon: 'icon-swap' }
] as const

// 架构类型选项
export const ARCHITECTURE_OPTIONS = [
  { value: 'Layered', label: '分层架构', description: '传统三层/四层架构，适合中小型项目' },
  { value: 'Clean', label: '整洁架构', description: '洋葱架构，适合大型企业级项目' },
  { value: 'Simple', label: '简单架构', description: '单项目结构，适合微服务或小工具' }
] as const

// .NET 版本选项
export const DOTNET_OPTIONS = [
  { value: 'Net8', label: '.NET 8 (LTS)' },
  { value: 'Net9', label: '.NET 9' }
] as const

// 数据库选项
export const DATABASE_OPTIONS = [
  { value: 'MySQL', label: 'MySQL' },
  { value: 'PostgreSQL', label: 'PostgreSQL' },
  { value: 'SqlServer', label: 'SQL Server' },
  { value: 'SQLite', label: 'SQLite' }
] as const

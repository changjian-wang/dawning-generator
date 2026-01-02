import request from './request'

export type ProjectType = 'Backend' | 'Frontend' | 'Fullstack'
export type FrontendFramework = 'VueArco' | 'VueElement' | 'ReactAntd'

export interface GenerateProjectRequest {
  projectName: string
  namespacePrefix?: string
  projectType: ProjectType
  architectureType: 'Layered' | 'Clean' | 'Simple'
  frontendFramework: FrontendFramework
  dotNetVersion: 'Net8' | 'Net9'
  databaseType: 'MySQL' | 'PostgreSQL' | 'SqlServer' | 'SQLite'
  selectedModules: string[]
  includeDocker: boolean
  includeGitHubActions: boolean
  includeUnitTests: boolean
  useRedis: boolean
  useSignalR: boolean
  useECharts: boolean
  servicePort: number
  frontendPort: number
}

// 后端 DTO 格式
interface BackendGenerateRequest {
  projectName: string
  namespacePrefix?: string
  projectType: string
  architectureType: string
  frontendFramework: string
  dotNetVersion: string
  database: {
    type: string
  }
  modules: string[]
  features: {
    includeTests: boolean
    includeDocker: boolean
    includeGitHubActions: boolean
    includeSwagger: boolean
    includeHealthChecks: boolean
    useRedis: boolean
    useSignalR: boolean
    useOpenIddict: boolean
  }
  frontend: {
    useECharts: boolean
    apiBaseUrl: string
    useSignalR: boolean
    appTitle: string
  }
  servicePort: number
  frontendPort: number
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
  projectName: string
  architectureType: string
  dotNetVersion: string
  databaseType: string
  selectedModules: string[]
  downloadCount: number
  createdAt: string
}

// 转换前端请求格式为后端格式
function transformRequest(data: GenerateProjectRequest): BackendGenerateRequest {
  return {
    projectName: data.projectName,
    namespacePrefix: data.namespacePrefix,
    projectType: data.projectType,
    architectureType: data.architectureType,
    frontendFramework: data.frontendFramework,
    dotNetVersion: data.dotNetVersion,
    database: {
      type: data.databaseType
    },
    modules: data.selectedModules.map(m => m.toLowerCase()),
    features: {
      includeTests: data.includeUnitTests,
      includeDocker: data.includeDocker,
      includeGitHubActions: data.includeGitHubActions,
      includeSwagger: data.selectedModules.includes('Swagger'),
      includeHealthChecks: data.selectedModules.includes('HealthCheck'),
      useRedis: data.useRedis,
      useSignalR: data.useSignalR,
      useOpenIddict: false
    },
    frontend: {
      useECharts: data.useECharts,
      apiBaseUrl: '/api',
      useSignalR: data.useSignalR,
      appTitle: data.projectName
    },
    servicePort: data.servicePort,
    frontendPort: data.frontendPort
  }
}

// 生成项目
export function generateProject(data: GenerateProjectRequest): Promise<Blob> {
  return request.post('/generator/generate', transformRequest(data), {
    responseType: 'blob'
  })
}

// 获取历史记录
export function getHistory(): Promise<ProjectHistory[]> {
  return request.get('/history')
}

// 获取历史详情
export function getHistoryById(id: string): Promise<ProjectHistory> {
  return request.get(`/history/${id}`)
}

// 删除历史记录
export function deleteHistory(id: string): Promise<void> {
  return request.delete(`/history/${id}`)
}

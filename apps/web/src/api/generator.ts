import request from './request'

export interface GenerateProjectRequest {
  projectName: string
  namespacePrefix?: string
  architectureType: 'Layered' | 'Clean' | 'Simple'
  dotNetVersion: 'Net8' | 'Net9'
  databaseType: 'MySQL' | 'PostgreSQL' | 'SqlServer' | 'SQLite'
  selectedModules: string[]
  includeDocker: boolean
  includeGitHubActions: boolean
  includeUnitTests: boolean
}

// 后端 DTO 格式
interface BackendGenerateRequest {
  projectName: string
  namespacePrefix?: string
  architectureType: string
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
  }
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
    architectureType: data.architectureType,
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
      includeHealthChecks: data.selectedModules.includes('HealthCheck')
    }
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

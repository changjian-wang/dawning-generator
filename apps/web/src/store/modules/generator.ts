import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { GenerateProjectRequest, GenerateProjectResponse } from '@/api/generator'

export const useGeneratorStore = defineStore('generator', () => {
  const currentConfig = ref<GenerateProjectRequest>({
    projectName: '',
    namespacePrefix: '',
    projectType: 'Backend',
    architectureType: 'Layered',
    frontendFramework: 'VueArco',
    dotNetVersion: 'Net8',
    databaseType: 'MySQL',
    selectedModules: ['Authentication', 'Logging'],
    includeDocker: true,
    includeGitHubActions: false,
    includeUnitTests: true,
    useRedis: false,
    useSignalR: false,
    useECharts: true,
    servicePort: 5000,
    frontendPort: 3000
  })

  const lastResult = ref<GenerateProjectResponse | null>(null)
  const isGenerating = ref(false)

  function setConfig(config: Partial<GenerateProjectRequest>) {
    currentConfig.value = { ...currentConfig.value, ...config }
  }

  function setResult(result: GenerateProjectResponse) {
    lastResult.value = result
  }

  function resetConfig() {
    currentConfig.value = {
      projectName: '',
      namespacePrefix: '',
      projectType: 'Backend',
      architectureType: 'Layered',
      frontendFramework: 'VueArco',
      dotNetVersion: 'Net8',
      databaseType: 'MySQL',
      selectedModules: ['Authentication', 'Logging'],
      includeDocker: true,
      includeGitHubActions: false,
      includeUnitTests: true,
      useRedis: false,
      useSignalR: false,
      useECharts: true,
      servicePort: 5000,
      frontendPort: 3000
    }
  }

  return {
    currentConfig,
    lastResult,
    isGenerating,
    setConfig,
    setResult,
    resetConfig
  }
})

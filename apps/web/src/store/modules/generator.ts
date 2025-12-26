import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { GenerateProjectRequest, GenerateProjectResponse } from '@/api/generator'

export const useGeneratorStore = defineStore('generator', () => {
  const currentConfig = ref<GenerateProjectRequest>({
    projectName: '',
    namespacePrefix: '',
    architectureType: 'Layered',
    dotNetVersion: 'Net8',
    databaseType: 'MySQL',
    selectedModules: ['Authentication', 'Logging'],
    includeDocker: true,
    includeGitHubActions: false,
    includeUnitTests: true
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
      architectureType: 'Layered',
      dotNetVersion: 'Net8',
      databaseType: 'MySQL',
      selectedModules: ['Authentication', 'Logging'],
      includeDocker: true,
      includeGitHubActions: false,
      includeUnitTests: true
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

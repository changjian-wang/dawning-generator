<template>
  <div class="generator-page">
    <a-form :model="formData" :rules="rules" layout="vertical" @submit="handleSubmit">
      <a-row :gutter="24">
        <!-- 左侧：基本配置 -->
        <a-col :xs="24" :lg="12">
          <a-card class="config-card" :bordered="false">
            <template #title>
              <div class="card-header">
                <icon-settings class="card-icon" />
                <span>{{ $t('generator.basicConfig') }}</span>
              </div>
            </template>
            
            <a-form-item field="projectName" :label="$t('generator.projectName')" required>
              <a-input 
                v-model="formData.projectName" 
                :placeholder="$t('generator.projectNamePlaceholder')"
                :max-length="50"
                allow-clear
              >
                <template #prefix>
                  <icon-folder />
                </template>
              </a-input>
            </a-form-item>
            
            <a-form-item field="namespacePrefix" :label="$t('generator.namespacePrefix')">
              <a-input 
                v-model="formData.namespacePrefix" 
                :placeholder="$t('generator.namespacePrefixPlaceholder')"
                allow-clear
              >
                <template #prefix>
                  <icon-code-block />
                </template>
              </a-input>
            </a-form-item>
            
            <a-form-item field="projectType" :label="$t('generator.projectType')" required>
              <a-radio-group v-model="formData.projectType" type="button" class="project-type-group">
                <a-radio value="Backend">
                  <icon-code /> {{ $t('generator.projectType.backend') }}
                </a-radio>
                <a-radio value="Frontend">
                  <icon-desktop /> {{ $t('generator.projectType.frontend') }}
                </a-radio>
                <a-radio value="Fullstack">
                  <icon-apps /> {{ $t('generator.projectType.fullstack') }}
                </a-radio>
              </a-radio-group>
            </a-form-item>
            
            <a-form-item 
              v-if="formData.projectType !== 'Frontend'"
              field="architectureType" 
              :label="$t('generator.architectureType')" 
              required
            >
              <a-radio-group v-model="formData.architectureType" direction="vertical" class="arch-radio-group">
                <a-radio value="Clean">
                  <div class="arch-radio-content">
                    <span class="arch-label">{{ $t('generator.architectureType.clean') }}</span>
                    <span class="arch-desc">{{ $t('generator.architectureType.cleanDesc') }}</span>
                  </div>
                </a-radio>
                <a-radio value="Layered">
                  <div class="arch-radio-content">
                    <span class="arch-label">{{ $t('generator.architectureType.layered') }}</span>
                    <span class="arch-desc">{{ $t('generator.architectureType.layeredDesc') }}</span>
                  </div>
                </a-radio>
                <a-radio value="Simple">
                  <div class="arch-radio-content">
                    <span class="arch-label">{{ $t('generator.architectureType.simple') }}</span>
                    <span class="arch-desc">{{ $t('generator.architectureType.simpleDesc') }}</span>
                  </div>
                </a-radio>
              </a-radio-group>
            </a-form-item>
            
            <a-form-item 
              v-if="formData.projectType !== 'Backend'"
              field="frontendFramework" 
              :label="$t('generator.frontendFramework')" 
              required
            >
              <a-radio-group v-model="formData.frontendFramework" direction="vertical" class="arch-radio-group">
                <a-radio value="VueArco">
                  <div class="arch-radio-content">
                    <span class="arch-label">Vue 3 + Arco Design</span>
                    <span class="arch-desc">{{ $t('generator.frontendFramework.vueArcoDesc') }}</span>
                  </div>
                </a-radio>
                <a-radio value="VueElement" disabled>
                  <div class="arch-radio-content">
                    <span class="arch-label">Vue 3 + Element Plus</span>
                    <span class="arch-desc">{{ $t('generator.frontendFramework.comingSoon') }}</span>
                  </div>
                </a-radio>
                <a-radio value="ReactAntd" disabled>
                  <div class="arch-radio-content">
                    <span class="arch-label">React + Ant Design</span>
                    <span class="arch-desc">{{ $t('generator.frontendFramework.comingSoon') }}</span>
                  </div>
                </a-radio>
              </a-radio-group>
            </a-form-item>
          </a-card>
        </a-col>
        
        <!-- 右侧：技术选项 -->
        <a-col :xs="24" :lg="12">
          <a-card class="config-card" :bordered="false">
            <template #title>
              <div class="card-header">
                <icon-tool class="card-icon" />
                <span>{{ $t('generator.techOptions') }}</span>
              </div>
            </template>
            
            <a-form-item 
              v-if="formData.projectType !== 'Frontend'"
              field="dotNetVersion" 
              :label="$t('generator.dotNetVersion')" 
              required
            >
              <a-radio-group v-model="formData.dotNetVersion" type="button">
                <a-radio value="Net8">.NET 8 (LTS)</a-radio>
                <a-radio value="Net9">.NET 9</a-radio>
              </a-radio-group>
            </a-form-item>
            
            <a-form-item 
              v-if="formData.projectType !== 'Frontend'"
              field="databaseType" 
              :label="$t('generator.databaseType')" 
              required
            >
              <a-select v-model="formData.databaseType">
                <a-option value="MySQL">MySQL</a-option>
                <a-option value="PostgreSQL">PostgreSQL</a-option>
                <a-option value="SqlServer">SQL Server</a-option>
                <a-option value="SQLite">SQLite</a-option>
              </a-select>
            </a-form-item>
            
            <a-form-item 
              v-if="formData.projectType !== 'Frontend'"
              field="selectedModules" 
              :label="$t('generator.modules')"
            >
              <a-checkbox-group v-model="formData.selectedModules" class="module-checkbox-group">
                <a-row :gutter="[8, 8]">
                  <a-col :span="12">
                    <a-checkbox value="Authentication">{{ $t('generator.modules.authentication') }}</a-checkbox>
                  </a-col>
                  <a-col :span="12">
                    <a-checkbox value="Logging">{{ $t('generator.modules.logging') }}</a-checkbox>
                  </a-col>
                  <a-col :span="12">
                    <a-checkbox value="Caching">{{ $t('generator.modules.caching') }}</a-checkbox>
                  </a-col>
                  <a-col :span="12">
                    <a-checkbox value="Swagger">{{ $t('generator.modules.swagger') }}</a-checkbox>
                  </a-col>
                  <a-col :span="12">
                    <a-checkbox value="HealthCheck">{{ $t('generator.modules.healthCheck') }}</a-checkbox>
                  </a-col>
                  <a-col :span="12">
                    <a-checkbox value="Dapper">Dapper ORM</a-checkbox>
                  </a-col>
                </a-row>
              </a-checkbox-group>
            </a-form-item>
            
            <a-divider />
            
            <a-form-item :label="$t('generator.options')">
              <a-space direction="vertical" fill>
                <a-checkbox v-model="formData.includeDocker">
                  <a-space>
                    <icon-cloud />
                    {{ $t('generator.includeDocker') }}
                  </a-space>
                </a-checkbox>
                <a-checkbox v-model="formData.includeGitHubActions">
                  <a-space>
                    <icon-github />
                    {{ $t('generator.includeGitHubActions') }}
                  </a-space>
                </a-checkbox>
                <a-checkbox 
                  v-if="formData.projectType !== 'Frontend'"
                  v-model="formData.includeUnitTests"
                >
                  <a-space>
                    <icon-bug />
                    {{ $t('generator.includeUnitTests') }}
                  </a-space>
                </a-checkbox>
                <a-checkbox 
                  v-if="formData.projectType !== 'Frontend'"
                  v-model="formData.useRedis"
                >
                  <a-space>
                    <icon-storage />
                    {{ $t('generator.useRedis') }}
                  </a-space>
                </a-checkbox>
                <a-checkbox v-model="formData.useSignalR">
                  <a-space>
                    <icon-sync />
                    {{ $t('generator.useSignalR') }}
                  </a-space>
                </a-checkbox>
                <a-checkbox 
                  v-if="formData.projectType !== 'Backend'"
                  v-model="formData.useECharts"
                >
                  <a-space>
                    <icon-bar-chart />
                    {{ $t('generator.useECharts') }}
                  </a-space>
                </a-checkbox>
              </a-space>
            </a-form-item>
          </a-card>
        </a-col>
      </a-row>
      
      <!-- 提交按钮 -->
      <div class="form-actions">
        <a-space size="medium">
          <a-button size="large" @click="handleReset">
            <template #icon><icon-refresh /></template>
            {{ $t('common.reset') }}
          </a-button>
          <a-button type="primary" html-type="submit" :loading="loading" size="large">
            <template #icon><icon-download /></template>
            {{ loading ? $t('generator.generating') : $t('generator.generate') }}
          </a-button>
        </a-space>
      </div>
    </a-form>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive } from 'vue'
import { Message } from '@arco-design/web-vue'
import { generateProject } from '@/api/generator'
import type { GenerateProjectRequest } from '@/api/generator'

const loading = ref(false)

const formData = reactive<GenerateProjectRequest>({
  projectName: '',
  namespacePrefix: '',
  projectType: 'Backend',
  architectureType: 'Clean',
  frontendFramework: 'VueArco',
  dotNetVersion: 'Net8',
  databaseType: 'MySQL',
  selectedModules: ['Authentication', 'Logging', 'Swagger'],
  includeDocker: true,
  includeGitHubActions: false,
  includeUnitTests: true,
  useRedis: false,
  useSignalR: false,
  useECharts: true,
  servicePort: 5000,
  frontendPort: 3000
})

const rules = {
  projectName: [
    { required: true, message: '请输入项目名称' },
    { match: /^[a-zA-Z][a-zA-Z0-9.]*[a-zA-Z0-9]$|^[a-zA-Z]$/, message: '项目名称只能包含字母、数字和点号，且以字母开头和字母/数字结尾' }
  ]
}

const handleSubmit = async () => {
  if (!formData.projectName) {
    Message.warning('请输入项目名称')
    return
  }
  
  loading.value = true
  try {
    const blob = await generateProject(formData) as unknown as Blob
    
    // 下载文件
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `${formData.projectName}.zip`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    
    Message.success('项目生成成功！')
  } catch (error) {
    console.error('生成失败:', error)
  } finally {
    loading.value = false
  }
}

const handleReset = () => {
  formData.projectName = ''
  formData.namespacePrefix = ''
  formData.projectType = 'Backend'
  formData.architectureType = 'Clean'
  formData.frontendFramework = 'VueArco'
  formData.dotNetVersion = 'Net8'
  formData.databaseType = 'MySQL'
  formData.selectedModules = ['Authentication', 'Logging', 'Swagger']
  formData.includeDocker = true
  formData.includeGitHubActions = false
  formData.includeUnitTests = true
  formData.useRedis = false
  formData.useSignalR = false
  formData.useECharts = true
  formData.servicePort = 5000
  formData.frontendPort = 3000
}
</script>

<style scoped>
.generator-page {
  max-width: 1200px;
  margin: 0 auto;
}

.config-card {
  border-radius: 8px;
  margin-bottom: 16px;
}

.config-card :deep(.arco-card-body) {
  padding: 16px 20px;
}

.config-card :deep(.arco-form-item) {
  margin-bottom: 12px;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
}

.card-icon {
  color: rgb(var(--primary-6));
}

/* 项目类型选择样式 */
.project-type-group {
  width: 100%;
}

.project-type-group :deep(.arco-radio-button) {
  flex: 1;
  text-align: center;
}

/* 架构选项样式 */
.arch-radio-group {
  width: 100%;
}

.arch-radio-group :deep(.arco-radio) {
  padding: 10px 12px;
  margin-bottom: 6px;
  border: 1px solid var(--color-border-2);
  border-radius: 6px;
  width: 100%;
  transition: all 0.2s;
}

.arch-radio-group :deep(.arco-radio:hover) {
  border-color: rgb(var(--primary-6));
}

.arch-radio-group :deep(.arco-radio-checked) {
  border-color: rgb(var(--primary-6));
  background: var(--color-primary-light-1);
}

.arch-radio-group :deep(.arco-radio-disabled) {
  opacity: 0.6;
}

.arch-radio-content {
  display: flex;
  flex-direction: column;
  margin-left: 4px;
}

.arch-label {
  font-weight: 500;
  color: var(--color-text-1);
}

.arch-desc {
  font-size: 12px;
  color: var(--color-text-3);
  margin-top: 2px;
}

/* 模块复选框样式 */
.module-checkbox-group {
  width: 100%;
}

/* 表单操作按钮 */
.form-actions {
  display: flex;
  justify-content: center;
  padding: 16px 0 8px;
}
</style>

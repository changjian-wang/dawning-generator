<template>
  <div class="history-page">
    <a-card :bordered="false">
      <template #title>
        <div class="card-header">
          <icon-history class="card-icon" />
          <span>{{ $t('history.title') }}</span>
        </div>
      </template>
      <template #extra>
        <a-button type="primary" @click="$router.push('/generator')">
          <template #icon><icon-plus /></template>
          {{ $t('history.createNew') || '创建新项目' }}
        </a-button>
      </template>
      
      <a-table 
        :columns="columns" 
        :data="historyList" 
        :loading="loading"
        :pagination="{ pageSize: 10, showTotal: true }"
        :bordered="false"
      >
        <template #architectureType="{ record }">
          <a-tag :color="getArchColor(record.architectureType)" size="small">
            {{ getArchLabel(record.architectureType) }}
          </a-tag>
        </template>
        
        <template #dotNetVersion="{ record }">
          <a-tag color="purple" size="small">{{ record.dotNetVersion }}</a-tag>
        </template>
        
        <template #databaseType="{ record }">
          <a-tag color="cyan" size="small">{{ record.databaseType }}</a-tag>
        </template>
        
        <template #selectedModules="{ record }">
          <a-space wrap size="mini">
            <a-tag v-for="m in record.selectedModules?.slice(0, 3)" :key="m" size="small">
              {{ m }}
            </a-tag>
            <a-tag v-if="record.selectedModules?.length > 3" size="small" color="gray">
              +{{ record.selectedModules.length - 3 }}
            </a-tag>
          </a-space>
        </template>
        
        <template #createdAt="{ record }">
          {{ formatDate(record.createdAt) }}
        </template>
        
        <template #actions="{ record }">
          <a-space>
            <a-tooltip content="重新下载">
              <a-button type="text" size="small" @click="handleDownload(record)">
                <icon-download />
              </a-button>
            </a-tooltip>
            <a-popconfirm content="确定删除此记录吗？" @ok="handleDelete(record.id)">
              <a-tooltip content="删除">
                <a-button type="text" size="small" status="danger">
                  <icon-delete />
                </a-button>
              </a-tooltip>
            </a-popconfirm>
          </a-space>
        </template>
      </a-table>
    </a-card>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { Message } from '@arco-design/web-vue'
import { getHistory, deleteHistory } from '@/api/generator'
import type { ProjectHistory } from '@/api/generator'

const loading = ref(false)
const historyList = ref<ProjectHistory[]>([])

const columns = [
  { title: '项目名称', dataIndex: 'projectName', width: 180 },
  { title: '架构', slotName: 'architectureType', width: 100 },
  { title: '.NET', slotName: 'dotNetVersion', width: 90 },
  { title: '数据库', slotName: 'databaseType', width: 100 },
  { title: '模块', slotName: 'selectedModules', width: 200 },
  { title: '下载次数', dataIndex: 'downloadCount', width: 90 },
  { title: '创建时间', slotName: 'createdAt', width: 160 },
  { title: '操作', slotName: 'actions', width: 100, fixed: 'right' as const }
]

const getArchColor = (type: string) => {
  const map: Record<string, string> = {
    'Layered': 'blue',
    'Clean': 'green',
    'Simple': 'orange'
  }
  return map[type] || 'gray'
}

const getArchLabel = (type: string) => {
  const map: Record<string, string> = {
    'Layered': '分层架构',
    'Clean': '整洁架构',
    'Simple': '简单架构'
  }
  return map[type] || type
}

const formatDate = (date: string) => {
  return new Date(date).toLocaleString('zh-CN')
}

const fetchHistory = async () => {
  loading.value = true
  try {
    historyList.value = await getHistory()
  } catch (error) {
    console.error('获取历史失败:', error)
  } finally {
    loading.value = false
  }
}

const handleDownload = (record: ProjectHistory) => {
  Message.info(`重新下载 ${record.projectName}...`)
  // TODO: 实现重新下载
}

const handleDelete = async (id: string) => {
  try {
    await deleteHistory(id)
    Message.success('删除成功')
    fetchHistory()
  } catch (error) {
    console.error('删除失败:', error)
  }
}

onMounted(() => {
  fetchHistory()
})
</script>

<style scoped>
.history-page {
  max-width: 1400px;
  margin: 0 auto;
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
</style>

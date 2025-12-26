<template>
  <div class="favorites-page">
    <a-card :bordered="false">
      <template #title>
        <div class="card-header">
          <icon-star class="card-icon" />
          <span>{{ $t('favorites.title') }}</span>
        </div>
      </template>
      <template #extra>
        <a-button type="primary" @click="showAddModal = true">
          <template #icon><icon-plus /></template>
          {{ $t('favorites.saveConfig') || '保存当前配置' }}
        </a-button>
      </template>
      
      <a-empty v-if="favorites.length === 0" :description="$t('favorites.empty') || '暂无收藏模板'">
        <template #image>
          <icon-star-fill :size="48" style="color: var(--color-fill-3)" />
        </template>
        <a-button type="primary" @click="$router.push('/generator')">
          {{ $t('favorites.goCreate') || '去创建项目' }}
        </a-button>
      </a-empty>
      
      <a-row v-else :gutter="16">
        <a-col v-for="item in favorites" :key="item.id" :xs="24" :sm="12" :lg="8">
          <a-card class="favorite-card" hoverable>
            <template #title>
              <div class="favorite-title">
                <span>{{ item.name }}</span>
                <a-tag v-if="item.isDefault" color="arcoblue" size="small">默认</a-tag>
              </div>
            </template>
            <template #extra>
              <a-dropdown>
                <a-button type="text" size="small">
                  <icon-more />
                </a-button>
                <template #content>
                  <a-doption @click="handleSetDefault(item.id)">
                    <icon-star /> 设为默认
                  </a-doption>
                  <a-doption @click="handleApply(item)">
                    <icon-import /> 应用配置
                  </a-doption>
                  <a-doption class="danger-option" @click="handleDelete(item.id)">
                    <icon-delete /> 删除
                  </a-doption>
                </template>
              </a-dropdown>
            </template>
            
            <p class="favorite-desc">{{ item.description || '无描述' }}</p>
            
            <div class="favorite-tags">
              <a-tag size="small">{{ item.config?.architectureType || 'Layered' }}</a-tag>
              <a-tag size="small">{{ item.config?.dotNetVersion || 'Net8' }}</a-tag>
              <a-tag size="small">{{ item.config?.databaseType || 'MySQL' }}</a-tag>
            </div>
            
            <template #actions>
              <a-button type="primary" long @click="handleApply(item)">
                <template #icon><icon-thunderbolt /></template>
                使用此模板
              </a-button>
            </template>
          </a-card>
        </a-col>
      </a-row>
    </a-card>
    
    <!-- 添加收藏弹窗 -->
    <a-modal v-model:visible="showAddModal" title="保存模板" @ok="handleSave" :width="480">
      <a-form :model="newFavorite" layout="vertical">
        <a-form-item label="模板名称" required>
          <a-input v-model="newFavorite.name" placeholder="给模板起个名字" />
        </a-form-item>
        <a-form-item label="描述">
          <a-textarea 
            v-model="newFavorite.description" 
            placeholder="描述一下这个模板的用途"
            :auto-size="{ minRows: 2, maxRows: 4 }"
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { Message } from '@arco-design/web-vue'
import { useGeneratorStore } from '@/store'

interface Favorite {
  id: string
  name: string
  description?: string
  isDefault: boolean
  config: Record<string, unknown>
}

const router = useRouter()
const generatorStore = useGeneratorStore()

const favorites = ref<Favorite[]>([])
const showAddModal = ref(false)
const newFavorite = reactive({
  name: '',
  description: ''
})

const handleSetDefault = (id: string) => {
  favorites.value.forEach(f => {
    f.isDefault = f.id === id
  })
  Message.success('已设为默认模板')
}

const handleApply = (item: Favorite) => {
  generatorStore.setConfig(item.config as any)
  router.push('/generator')
  Message.success(`已应用模板: ${item.name}`)
}

const handleDelete = (id: string) => {
  favorites.value = favorites.value.filter(f => f.id !== id)
  Message.success('删除成功')
}

const handleSave = () => {
  if (!newFavorite.name) {
    Message.warning('请输入模板名称')
    return
  }
  
  favorites.value.push({
    id: Date.now().toString(),
    name: newFavorite.name,
    description: newFavorite.description,
    isDefault: false,
    config: { ...generatorStore.currentConfig }
  })
  
  showAddModal.value = false
  newFavorite.name = ''
  newFavorite.description = ''
  Message.success('保存成功')
}
</script>

<style scoped>
.favorites-page {
  max-width: 1200px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
}

.card-icon {
  color: rgb(var(--warning-6));
}

.favorite-card {
  margin-bottom: 16px;
}

.favorite-title {
  display: flex;
  align-items: center;
  gap: 8px;
}

.favorite-desc {
  color: var(--color-text-3);
  font-size: 13px;
  margin-bottom: 12px;
}

.favorite-tags {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.danger-option {
  color: rgb(var(--danger-6)) !important;
}
</style>

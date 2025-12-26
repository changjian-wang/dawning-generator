<template>
  <div class="result-page">
    <a-result status="success" title="项目生成成功">
      <template #subtitle>
        您的项目已成功生成并下载
      </template>
      <template #extra>
        <a-space>
          <a-button type="primary" @click="$router.push('/generator')">
            创建新项目
          </a-button>
          <a-button @click="$router.push('/history')">
            查看历史
          </a-button>
        </a-space>
      </template>
    </a-result>
  </div>
</template>

<script lang="ts" setup>
</script>

<style scoped>
.result-page {
  max-width: 800px;
  margin: 100px auto;
}
</style>

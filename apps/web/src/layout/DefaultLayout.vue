<template>
  <a-layout class="layout">
    <a-layout-header class="header">
      <div class="header-content">
        <div class="left-side" @click="$router.push('/')">
          <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg" class="logo-svg">
            <defs>
              <linearGradient id="arrowGradient" x1="16" y1="2" x2="16" y2="30" gradientUnits="userSpaceOnUse">
                <stop offset="0%" stop-color="#165DFF"/>
                <stop offset="100%" stop-color="#6AA1FF"/>
              </linearGradient>
            </defs>
            <path d="M16 2 L28 28 L20 28 L16 18 L12 28 L4 28 Z" fill="url(#arrowGradient)"/>
            <path d="M16 12 L13 20 L16 20 L19 20 Z" fill="white"/>
          </svg>
          <span class="brand-title">Dawning Generator</span>
        </div>

        <div class="center-side">
          <a-menu 
            mode="horizontal" 
            :selected-keys="[currentRoute]" 
            @menu-item-click="handleMenuClick"
            class="nav-menu"
          >
            <a-menu-item key="generator">
              <template #icon><icon-code /></template>
              {{ $t('menu.generator') }}
            </a-menu-item>
            <a-menu-item key="history">
              <template #icon><icon-history /></template>
              {{ $t('menu.history') }}
            </a-menu-item>
            <a-menu-item key="favorites">
              <template #icon><icon-star /></template>
              {{ $t('menu.favorites') }}
            </a-menu-item>
          </a-menu>
        </div>

        <div class="right-side">
          <a-tooltip :content="$t('common.switchLanguage') || '切换语言'">
            <a-dropdown trigger="click">
              <a-button type="text" class="nav-btn">
                <icon-language :size="18" />
              </a-button>
              <template #content>
                <a-doption 
                  v-for="item in LOCALE_OPTIONS" 
                  :key="item.value"
                  @click="changeLocale(item.value)"
                >
                  <a-space>
                    <span>{{ item.label }}</span>
                    <icon-check v-if="locale === item.value" style="color: rgb(var(--primary-6))" />
                  </a-space>
                </a-doption>
              </template>
            </a-dropdown>
          </a-tooltip>
          
          <a-tooltip content="GitHub">
            <a-button type="text" class="nav-btn" @click="openGithub">
              <icon-github :size="18" />
            </a-button>
          </a-tooltip>
          
          <a-tooltip :content="$t('common.docs') || '文档'">
            <a-button type="text" class="nav-btn" @click="openDocs">
              <icon-book :size="18" />
            </a-button>
          </a-tooltip>
        </div>
      </div>
    </a-layout-header>

    <a-layout-content class="main-content">
      <div class="content-wrapper">
        <router-view v-slot="{ Component }">
          <transition name="fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </div>
    </a-layout-content>

    <a-layout-footer class="footer">
      <div class="footer-content">
        <span>© {{ new Date().getFullYear() }} Dawning Generator</span>
        <a-divider direction="vertical" />
        <span>{{ $t('generator.description') }}</span>
        <a-divider direction="vertical" />
        <a-link href="https://github.com/changjian-wang/dawning" target="_blank" :hoverable="false">
          <icon-github :size="14" style="margin-right: 4px" />
          Dawning SDK
        </a-link>
      </div>
    </a-layout-footer>
  </a-layout>
</template>

<script lang="ts" setup>
import { computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { LOCALE_OPTIONS } from '@/locale'

const router = useRouter()
const route = useRoute()
const { locale } = useI18n()

const currentRoute = computed(() => {
  const path = route.path
  if (path.startsWith('/generator')) return 'generator'
  if (path.startsWith('/history')) return 'history'
  if (path.startsWith('/favorites')) return 'favorites'
  return 'generator'
})

const handleMenuClick = (key: string) => {
  router.push(`/${key}`)
}

const changeLocale = (value: string) => {
  locale.value = value
  localStorage.setItem('locale', value)
}

const openGithub = () => {
  window.open('https://github.com/changjian-wang/dawning-generator', '_blank')
}

const openDocs = () => {
  window.open('https://github.com/changjian-wang/dawning-generator#readme', '_blank')
}
</script>

<style scoped>
.layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: var(--color-fill-2);
  overflow-x: hidden;
}

.header {
  background: var(--color-bg-2);
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  position: sticky;
  top: 0;
  z-index: 100;
  padding: 0;
  border-bottom: 1px solid var(--color-border);
  overflow: hidden;
  flex-shrink: 0;
}

.header-content {
  max-width: 1400px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  height: 60px;
}

.left-side {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  transition: opacity 0.2s;
}

.left-side:hover {
  opacity: 0.85;
}

.logo-svg {
  flex-shrink: 0;
}

.brand-title {
  font-size: 20px;
  font-weight: 600;
  letter-spacing: 0.5px;
  font-family: 'Poppins', 'Segoe UI', -apple-system, BlinkMacSystemFont, sans-serif;
  background: linear-gradient(135deg, #165DFF 0%, #4080FF 50%, #6AA1FF 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  white-space: nowrap;
}

.center-side {
  flex: 1;
  display: flex;
  justify-content: center;
  overflow: hidden;
}

.nav-menu {
  background: transparent;
  border: none;
  overflow: hidden;
}

.nav-menu :deep(.arco-menu-inner) {
  padding: 0;
  overflow: hidden;
}

.nav-menu :deep(.arco-menu-overflow-wrap) {
  overflow: hidden;
}

.nav-menu :deep(.arco-menu-item) {
  padding: 0 20px;
  line-height: 58px;
  font-weight: 500;
  color: var(--color-text-2);
  transition: all 0.2s;
}

.nav-menu :deep(.arco-menu-item:hover) {
  color: rgb(var(--primary-6));
  background: transparent;
}

.nav-menu :deep(.arco-menu-selected) {
  color: rgb(var(--primary-6));
  background: transparent;
}

.nav-menu :deep(.arco-menu-selected::after) {
  content: '';
  position: absolute;
  bottom: 0;
  left: 20px;
  right: 20px;
  height: 2px;
  background: linear-gradient(90deg, #165DFF 0%, #6AA1FF 100%);
  border-radius: 1px;
}

.right-side {
  display: flex;
  align-items: center;
  gap: 4px;
}

.nav-btn {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  color: var(--color-text-2);
  transition: all 0.2s;
}

.nav-btn:hover {
  color: rgb(var(--primary-6));
  background: var(--color-fill-2);
}

.main-content {
  flex: 1;
  padding: 20px 24px;
  overflow-y: auto;
}

.content-wrapper {
  max-width: 1400px;
  margin: 0 auto;
}

.footer {
  background: var(--color-bg-2);
  border-top: 1px solid var(--color-border);
  padding: 0;
  flex-shrink: 0;
}

.footer-content {
  max-width: 1400px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  height: 52px;
  color: var(--color-text-3);
  font-size: 13px;
}

.footer-content :deep(.arco-link) {
  font-size: 13px;
  color: var(--color-text-3);
}

.footer-content :deep(.arco-link:hover) {
  color: rgb(var(--primary-6));
}

/* Page transition */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Responsive */
@media (max-width: 768px) {
  .header-content {
    padding: 0 16px;
  }

  .brand-title {
    font-size: 16px;
  }

  .center-side {
    display: none;
  }

  .main-content {
    padding: 16px;
  }
}
</style>

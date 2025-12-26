import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { DefaultLayout } from '@/layout'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: DefaultLayout,
    redirect: '/generator',
    children: [
      {
        path: 'generator',
        name: 'Generator',
        component: () => import('@/views/generator/index.vue'),
        meta: { title: '项目生成器' }
      },
      {
        path: 'generator/result',
        name: 'GeneratorResult',
        component: () => import('@/views/generator/result.vue'),
        meta: { title: '生成结果' }
      },
      {
        path: 'history',
        name: 'History',
        component: () => import('@/views/history/index.vue'),
        meta: { title: '项目历史' }
      },
      {
        path: 'favorites',
        name: 'Favorites',
        component: () => import('@/views/favorites/index.vue'),
        meta: { title: '收藏模板' }
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, _from, next) => {
  document.title = `${to.meta.title || 'Dawning Generator'} - Dawning Generator`
  next()
})

export default router

import { mergeConfig } from 'vite'
import baseConfig from './vite.config.base'

export default mergeConfig(baseConfig, {
  mode: 'production',
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          arco: ['@arco-design/web-vue'],
          vue: ['vue', 'vue-router', 'pinia'],
        },
      },
    },
    chunkSizeWarningLimit: 2000,
  },
})

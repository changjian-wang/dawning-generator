import { mergeConfig } from 'vite'
import baseConfig from './vite.config.base'

export default mergeConfig(baseConfig, {
  mode: 'development',
  server: {
    open: true,
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5215',
        changeOrigin: true,
      },
    },
  },
})

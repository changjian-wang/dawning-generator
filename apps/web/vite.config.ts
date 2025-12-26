import { mergeConfig } from 'vite'

// 根据环境加载不同配置
const isDev = process.env.NODE_ENV !== 'production'

export default async () => {
  const configModule = isDev 
    ? await import('./config/vite.config.dev')
    : await import('./config/vite.config.prod')
  return configModule.default
}

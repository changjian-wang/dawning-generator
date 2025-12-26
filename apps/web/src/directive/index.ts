import { App } from 'vue'

// 权限指令
const permission = {
  mounted(el: HTMLElement, binding: { value: string[] }) {
    const { value } = binding
    // 这里可以添加权限校验逻辑
    if (value && value.length > 0) {
      // 暂时不做权限校验，后续可扩展
    }
  },
}

export default {
  install(app: App) {
    app.directive('permission', permission)
  },
}

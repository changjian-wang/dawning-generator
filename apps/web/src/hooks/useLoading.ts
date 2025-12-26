import { ref, Ref } from 'vue'

/**
 * 加载状态 Hook
 */
export function useLoading(initialValue = false): [Ref<boolean>, <T>(fn: () => Promise<T>) => Promise<T>] {
  const loading = ref(initialValue)

  const withLoading = async <T>(fn: () => Promise<T>): Promise<T> => {
    loading.value = true
    try {
      return await fn()
    } finally {
      loading.value = false
    }
  }

  return [loading, withLoading]
}

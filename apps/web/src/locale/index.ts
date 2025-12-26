import { createI18n } from 'vue-i18n'
import zhCN from './zh-CN'
import enUS from './en-US'

export const LOCALE_OPTIONS = [
  { label: '中文', value: 'zh-CN' },
  { label: 'English', value: 'en-US' },
]

const defaultLocale = localStorage.getItem('locale') || 'zh-CN'

const i18n = createI18n({
  locale: defaultLocale,
  fallbackLocale: 'zh-CN',
  legacy: false,
  allowComposition: true,
  messages: {
    'zh-CN': zhCN,
    'en-US': enUS,
  },
})

export default i18n

import localeGenerator from '@/views/generator/locale/zh-CN'
import localeHistory from '@/views/history/locale/zh-CN'
import localeFavorites from '@/views/favorites/locale/zh-CN'

export default {
  // 通用
  'common.search': '搜索',
  'common.reset': '重置',
  'common.confirm': '确认',
  'common.cancel': '取消',
  'common.save': '保存',
  'common.delete': '删除',
  'common.edit': '编辑',
  'common.add': '新增',
  'common.actions': '操作',
  'common.createdAt': '创建时间',
  'common.updatedAt': '更新时间',
  'common.loading': '加载中...',
  'common.noData': '暂无数据',
  'common.operationSuccess': '操作成功',
  'common.operationFailed': '操作失败',
  'common.deleteConfirm': '确定要删除吗？',
  'common.deleteSuccess': '删除成功',
  'common.deleteFailed': '删除失败',

  // 菜单
  'menu.generator': '项目生成器',
  'menu.history': '历史记录',
  'menu.favorites': '收藏模板',

  // 页面合并
  ...localeGenerator,
  ...localeHistory,
  ...localeFavorites,
}

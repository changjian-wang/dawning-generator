import localeGenerator from '@/views/generator/locale/en-US'
import localeHistory from '@/views/history/locale/en-US'
import localeFavorites from '@/views/favorites/locale/en-US'

export default {
  // Common
  'common.search': 'Search',
  'common.reset': 'Reset',
  'common.confirm': 'Confirm',
  'common.cancel': 'Cancel',
  'common.save': 'Save',
  'common.delete': 'Delete',
  'common.edit': 'Edit',
  'common.add': 'Add',
  'common.actions': 'Actions',
  'common.createdAt': 'Created At',
  'common.updatedAt': 'Updated At',
  'common.loading': 'Loading...',
  'common.noData': 'No Data',
  'common.operationSuccess': 'Operation Success',
  'common.operationFailed': 'Operation Failed',
  'common.deleteConfirm': 'Are you sure to delete?',
  'common.deleteSuccess': 'Deleted successfully',
  'common.deleteFailed': 'Failed to delete',

  // Menu
  'menu.generator': 'Project Generator',
  'menu.history': 'History',
  'menu.favorites': 'Favorites',

  // Merge page locales
  ...localeGenerator,
  ...localeHistory,
  ...localeFavorites,
}

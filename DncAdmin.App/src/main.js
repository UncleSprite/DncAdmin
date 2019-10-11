import Vue from 'vue'

import 'normalize.css/normalize.css' // A modern alternative to CSS resets

import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import locale from 'element-ui/lib/locale/lang/zh-CN' // lang i18n
import moment from 'moment'

import '@/styles/index.scss' // global css

import App from './App'
import router from './router'
import store from './store'

import '@/icons' // icon
import '@/permission' // permission control

Vue.use(ElementUI, { locale, size: 'small' })

Vue.config.productionTip = false

Vue.filter('dateFmt', (input, fmt) => {
  return moment(input).format(fmt)
})

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})

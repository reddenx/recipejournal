import Vue from 'vue'
import VueRouter from 'vue-router'
import HomePage from '../views/HomePage.vue'
import RecipePage from '../views/RecipePage'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomePage
  },
  {
    path: '/recipes',
    name: 'recipe list',
  },
  {
    path: '/recipes/:recipeId',
    name: 'recipe',
    component: RecipePage,
    props: true
  }
]

const router = new VueRouter({
  routes
})

export default router

import Vue from 'vue'
import VueRouter from 'vue-router'
import HomePage from '../views/HomePage.vue'
import RecipePage from '../views/RecipePage'
import CreateRecipePage from '../views/CreateRecipePage'
import EditRecipePage from '../views/EditRecipePage'

import CookingPage from '../views/CookingPage';

Vue.use(VueRouter)

const routes = [
  {
    path: '/cook/:recipeId',
    component: CookingPage,
    props: true
  },
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
    path: '/recipes/new',
    name: 'create recipe',
    component: CreateRecipePage
  },
  {
    path: '/recipes/:recipeId',
    name: 'recipe',
    component: RecipePage,
    props: true
  },
  {
    path: '/recipes/:recipeId/edit',
    name: 'edit recipe',
    component: EditRecipePage,
    props: true
  },
]

const router = new VueRouter({
  routes
})

export default router

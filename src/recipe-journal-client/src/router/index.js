import Vue from 'vue'
import VueRouter from 'vue-router'
import HomePage from '../views/HomePage.vue'
// import RecipePage from '../views/RecipePage'
// import CreateRecipePage from '../views/CreateRecipePage'
// import EditRecipePage from '../views/EditRecipePage'

// import MockPage from '../views/MockPage';
import CookingPage from '../views/CookingPage';
import CookingListPage from "../views/CookingListPage";
import RecipeCmsPage from "../views/RecipeCmsPage";
import GroceriesPage from "../views/GroceriesPage.vue";

Vue.use(VueRouter)

const routes = [
  {
    path: '/cook/:recipeId',
    component: CookingPage,
    props: true
  },
  {
    path: '/cook',
    component: CookingListPage,
  },
  {
    path: '/cms/:id?',
    component: RecipeCmsPage,
    props: true,
  },
  {
    path: '/',
    name: 'home',
    component: HomePage
  },
  {
    path:'/groceries',
    component: GroceriesPage
  }
  // {
  //   path: '/recipes',
  //   name: 'recipe list',
  // },
  // {
  //   path: '/recipes/new',
  //   name: 'create recipe',
  //   component: CreateRecipePage
  // },
  // {
  //   path: '/recipes/:recipeId',
  //   name: 'recipe',
  //   component: RecipePage,
  //   props: true
  // },
  // {
  //   path: '/recipes/:recipeId/edit',
  //   name: 'edit recipe',
  //   component: EditRecipePage,
  //   props: true
  // },
  // {
  //   path:'/mock',
  //   name: 'mock',
  //   component: MockPage
  // },
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router

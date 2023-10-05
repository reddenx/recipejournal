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
import JournalPage from "../views/JournalPage.vue";
import GoalsPage from "../views/GoalsPage";
import AccountPage from "../views/AccountPage";
import RecipeJournalPage from "../views/RecipeJournalPage.vue";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomePage
  },
  {
    path: '/cook',
    component: CookingListPage,
  },
  {
    path: '/cook/:recipeId',
    component: CookingPage,
    props: true
  },
  {
    path: '/cms/:id?',
    component: RecipeCmsPage,
    props: true,
  },
  {
    path:'/groceries',
    component: GroceriesPage
  },
  {
    path: '/journal',
    component: JournalPage
  },
  {
    path: '/journal/:recipeId',
    component: RecipeJournalPage,
    props: true
  },
  {
    path: '/goals',
    component: GoalsPage
  },
  {
    path: '/account',
    component: AccountPage
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

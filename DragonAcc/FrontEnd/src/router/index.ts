import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import PurchasedAccount from '@/views/PurchasedAccount.vue'
import ChatBoxView from '@/views/ChatBoxView.vue'
import AboutView from '../views/AboutView.vue'
import ProfileView from '@/views/ProfileView.vue'
import NotificationsView from '@/views/NotificationsView.vue'
import AuctionListView from '@/views/AuctionListView.vue'
import AuctionDetailView from '@/views/AuctionDetailView.vue'
import LoginView from '@/views/LoginView.vue'
import LuckyWheelView from '@/views/LuckyWheelView.vue'
import AIEsportView from '@/views/AIEsportView.vue'
import { userStore } from '../stores/auth'
import AdminManageView from '@/views/AdminManageView.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      component: AboutView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      meta: { requiresAuth: true }
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminManageView,
      meta: { requiresAuth: true }
    },
    {
      path: '/aiesport',
      name: 'aiesport',
      component: AIEsportView,
    },
    {
      path: '/chatbot',
      name: 'chatbot',
      component: ChatBoxView,
    },  
    {
      path: '/luckywheel',
      name: 'luckywheel',
      component: LuckyWheelView,
    }, 
    {
      path: '/auctionlist',
      name: 'auctionlist',
      component: AuctionListView,
    }, 
    {
      path: '/auctiondetail',
      name: 'auctiondetail',
      component: AuctionDetailView,
    }, 
    {
      path: '/profileview',
      name: 'profileview',
      component: ProfileView,
    }, 
    {
      path: '/notification',
      name: 'notification',
      component: NotificationsView,
    }, 
    {
      path: '/purchasedaccount',
      name: 'purchasedaccount',
      component: PurchasedAccount,
    }, 
    
  ]
})

router.beforeEach((to, from, next) => {
  const authStore = userStore();
  if (to.meta.requiresAuth && !authStore.user) {
    next('/login');
    alert("Không vào được đâu!");
  } else {
    next();
  }
});
export default router

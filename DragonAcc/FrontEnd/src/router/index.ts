import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ChatBoxView from '@/views/ChatBoxView.vue'
import AboutView from '../views/AboutView.vue'
import PurchasedGame from '@/views/PurchasedGame.vue'
import ProfileView from '@/views/ProfileView.vue'
import NotificationsView from '@/views/NotiView.vue'
import AuctionListView from '@/views/AuctionListView.vue'
import AuctionDetailView from '@/views/AuctionDetailView.vue'
import LoginView from '@/views/LoginView.vue'
import LuckyWheelView from '@/views/LuckyWheelView.vue'
import AIEsportView from '@/views/AIEsportView.vue'
import RegisterView from '@/views/RegisterView.vue'
import AdminManageDeposit from '@/views/AdminManageDeposit.vue'
import DepositView from '@/views/DepositView.vue'
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
      path: '/auction/:id',
      name: 'auctionDetail',
      component: AuctionDetailView,
      props: true, 
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
      path: '/purchased',
      name: 'purchased',
      component: PurchasedGame,
    }, 
    {
      path: '/deposit',
      name: 'deposit',
      component: DepositView,
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
    },
    {
      path: '/adminmanagedeposit',
      name: 'adminmanagedeposit',
      component: AdminManageDeposit,
    },
    
    
  ]
})

export default router

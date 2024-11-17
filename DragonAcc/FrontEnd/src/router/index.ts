import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import AboutView from '../views/AboutView.vue';
import PurchasedGame from '@/views/PurchasedGame.vue';
import ProfileView from '@/views/ProfileView.vue';
import NotificationsView from '@/views/NotiView.vue';
import AuctionListView from '@/views/AuctionListView.vue';
import AuctionDetailView from '@/views/AuctionDetailView.vue';
import StatisticalView from '@/views/StatisticalView.vue';
import LoginView from '@/views/LoginView.vue';
import WithDrawMoneyView from '@/views/WithDrawMoneyView.vue';
import LuckyWheelView from '@/views/LuckyWheelView.vue';
import AdminManageWithDrawView from '@/views/AdminManageWithDrawView.vue';
import AIEsportView from '@/views/AIEsportView.vue';
import RegisterView from '@/views/RegisterView.vue';
import SlotGameView from '@/views/SlotGameView.vue';
import AdminManageDeposit from '@/views/AdminManageDeposit.vue';
import DepositView from '@/views/DepositView.vue';
import AdminManageView from '@/views/AdminManageView.vue';

// Import the user store or authentication API for role checking
import { userStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { path: '/about', name: 'about', component: AboutView },
    { path: '/login', name: 'login', component: LoginView },
    { path: '/admin', name: 'admin', component: AdminManageView, meta: { requiresAuth: true } },
    { path: '/aiesport', name: 'aiesport', component: AIEsportView },
    { path: '/luckywheel', name: 'luckywheel', component: LuckyWheelView },
    { path: '/auctionlist', name: 'auctionlist', component: AuctionListView },
    { path: '/auction/:id', name: 'auctionDetail', component: AuctionDetailView, props: true },
    { path: '/profileview', name: 'profileview', component: ProfileView },
    { path: '/notification', name: 'notification', component: NotificationsView },
    { path: '/purchased', name: 'purchased', component: PurchasedGame },
    { path: '/deposit', name: 'deposit', component: DepositView },
    { path: '/register', name: 'register', component: RegisterView },
    { path: '/adminmanagedeposit', name: 'adminmanagedeposit', component: AdminManageDeposit },
    { path: '/statistical', name: 'statistical', component: StatisticalView },
    { path: '/withdrawmoney', name: 'withdrawmoney', component: WithDrawMoneyView },
    { path: '/adminmanagewithdraw', name: 'adminmanagewithdraw', component: AdminManageWithDrawView },
    { path: '/slotgame', name: 'slotgame', component: SlotGameView },
  ]
});

router.beforeEach((to, from, next) => {
  const store = userStore();
  console.log("User info:", store.user);
  const isAdmin = store.user?.userName === 'Admin';
  if (to.path.startsWith('/admin')) {
if (!isAdmin) {
  alert("Tao đố mày vào được luôn");
      return next({ name: 'home' });
    }
  }
  next();
});



export default router;

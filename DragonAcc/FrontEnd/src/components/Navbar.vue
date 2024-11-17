<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-white border-bottom">
    <div class="container d-flex align-items-center">
      <a class="navbar-brand" href="/">
        <img src="../assets/LogoWebsite.png" alt="" class="logo-img">
      </a>

      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="navbarNav">
        <div class="search-container d-flex align-items-center mx-auto my-2 my-lg-0">
          <form class="input-group">
            <span class="input-group-text bg-light border-0">
              <i class="fas fa-search"></i>
            </span>
            <input type="search" class="form-control border-0" placeholder="Tìm kiếm" aria-label="Search" />
          </form>
        </div>

        <ul class="navbar-nav ms-auto d-flex align-items-center">
          <li class="nav-item">
            <a class="nav-link text-center" href="/">
              <i class="fas fa-home"></i>
              <div class="nav-text">Trang chủ</div>
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link text-center" href="/auctionlist">
              <i class="fas fa-gavel"></i>
              <div class="nav-text">Đấu giá</div>
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link text-center" href="/luckywheel">
              <i class="fas fa-gift"></i>
              <div class="nav-text">Thử vận may</div>
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link text-center" href="/notification">
              <i class="fas fa-bell"></i>
              <div class="nav-text">Thông báo</div>
            </a>
          </li>

          <!-- Admin Menu: Visible Only to Admin Users -->
          <li v-if="isAdmin" class="nav-item dropdown">
            <a class="nav-link text-center admin-dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
              <i class="fas fa-tools"></i>
              <div class="nav-text">Quản trị</div>
            </a>
            <ul class="dropdown-menu admin-dropdown-menu" aria-labelledby="adminDropdown">
              <li><a class="dropdown-item admin-dropdown-item" href="/admin">Quản lý tài khoản</a></li>
              <li><a class="dropdown-item admin-dropdown-item" href="/adminmanagedeposit">Quản lý nạp thẻ</a></li>
              <li><a class="dropdown-item admin-dropdown-item" href="/adminmanagewithdraw">Quản lý rút tiền</a></li>
            </ul>
          </li>                           

          <li class="nav-item">
            <a class="nav-link text-center" href="/aiesport">
              <i class="fas fa-brain"></i>
              <div class="nav-text">Dự đoán</div>
            </a>
          </li>

          <!-- User Profile Dropdown -->
          <li v-if="isLoggedIn" class="nav-item dropdown">
            <a class="nav-link dropdown-toggle d-flex align-items-center" href="#" id="profileDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
              <img src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg" class="rounded-circle" height="30" alt="Profile" />
            </a>
            <div class="dropdown-menu dropdown-menu-end profile-dropdown" aria-labelledby="profileDropdown">
              <!-- Profile Header -->
              <div class="dropdown-header d-flex align-items-center">
                <img src="https://tintuc.dienthoaigiakho.vn/wp-content/uploads/2024/01/avatar-nam-nu-trang-2.jpg" class="rounded-circle me-2" height="50" alt="Profile" />
                <div>
                  <div class="profile-name">{{ fullname }}</div>
                  <div class="profile-status">Vip 1</div>
                  <a href="profileview">
                    <button class="btn btn-outline-primary btn-sm mt-2">Xem hồ sơ</button>
                  </a>
                </div>
              </div>
              <hr class="dropdown-divider" />
              <!-- Account Section -->
              <div class="dropdown-section">
                <div class="dropdown-item"><strong>Tài khoản</strong></div>
                <div class="dropdown-item"><span class="badge bg-warning text-dark">Dùng thử 1 tháng Premium</span></div>
                <a class="dropdown-item" href="/purchased">Lịch sử giao dịch</a>
                <a class="dropdown-item" href="/deposit">Nạp tiền</a>
                <a class="dropdown-item" href="/withdrawmoney">Rút tiền</a>
                <li class="nav-item">
                  <a class="nav-link text-center" href="#">
                    <i class="fas fa-globe"></i>
                    <div class="nav-text">Ngôn ngữ</div>
                  </a>
                </li>
              </div>
              <hr class="dropdown-divider" />
              <!-- Logout -->
              <a class="dropdown-item logout" style="cursor: pointer;" @click.prevent="logout">Đăng xuất</a>
            </div>
          </li>
          
          <!-- Display Login Icon if Not Logged In -->
          <li v-else class="nav-item">
            <a class="nav-link text-center" href="/login">
              <i class="fas fa-user"></i>
              <div class="nav-text">Đăng nhập</div>
            </a>
          </li>            
        </ul>
      </div>
    </div>
    <!-- Container wrapper -->
  </nav>
</template>

<script lang="ts">
import { computed, onMounted, ref } from 'vue';
import { userStore } from '@/stores/auth';
import router from '@/router';
import authenticateApi from '@/api/authenticate.api';

export default {
  setup() {
    const store = userStore();
    const isLoggedIn = ref(false);
    const isLanguageOpen = ref(false);
    const showDropdown = ref(false);
    const userFullName = ref('');
    const isAdmin = ref(false);
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    onMounted(async () => {
  store.init();
  const token = localStorage.getItem('token');
  if (token) {
    isLoggedIn.value = true;
    userFullName.value = localStorage.getItem('fullName') || '';

    try {
      const response = await authenticateApi.getRoleById(userId);
      const roles = response.data.result;
      isAdmin.value = roles.some((role: string) => role === 'Admin');
    } catch (error) {
      console.error("Failed to fetch user roles:", error);
    }
  }
});


    const fullname = computed(() => store.user?.fullName || 'Guest');

    const toggleLanguage = () => {
      isLanguageOpen.value = !isLanguageOpen.value;
    };

    const logout = async () => {
      try {
        await authenticateApi.logout();
        localStorage.removeItem('token');
        localStorage.removeItem('fullName');
        store.clearUser();
        isLoggedIn.value = false;
        showDropdown.value = false;
        router.push('/login');
      } catch (error) {
        console.error("Logout failed:", error);
      }
    };

    return {
      fullname,
      logout,
      isLoggedIn,
      isLanguageOpen,
      toggleLanguage,
      isAdmin,
    };
  },
};
</script>

<style scoped>
.logo-img {
  height: 39px;
  width: auto;
}
.navbar {
  background-color: #ffffff;
  border-bottom: 1px solid #e6e6e6;
  padding: 0.5rem 1rem;
  position: sticky;
  top: 0px;
  z-index: 1000;
}

.navbar-brand {
  font-size: 1.5rem;
}

.search-container {
  flex-grow: 1;
  margin-left: 20px;
}

.input-group {
  width: 100%;
  max-width: 300px;
  border: 1px solid #dfe3e8;
  border-radius: 4px;
  overflow: hidden;
}

.input-group .form-control {
  border: none;
  box-shadow: none;
}

.input-group-text {
  background-color: #f3f6f8;
  border: none;
}

.nav-link {
  color: #333;
  font-size: 0.875rem;
  text-decoration: none;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0 10px;
}

.nav-link:hover {
  color: #0073b1;
}

.nav-text {
  font-size: 0.75rem;
  color: #6c757d;
}

.profile-dropdown {
  width: 300px;
  padding: 0;
}

.dropdown-header {
  padding: 15px;
  display: flex;
  align-items: center;
}

.profile-name {
  font-weight: bold;
  font-size: 1rem;
}

.profile-status {
  font-size: 0.875rem;
  color: #6c757d;
}

.btn-outline-primary {
  border-color: #0073b1;
  color: #0073b1;
}

.dropdown-section {
  padding: 10px 15px;
}

.dropdown-item {
  font-size: 0.875rem;
  color: #000;
  text-decoration: none;
}

.dropdown-item:hover {
  color: #0073b1;
  background-color: #f3f6f8;
}

.badge {
  font-size: 0.75rem;
  padding: 3px 6px;
  border-radius: 5px;
}

hr.dropdown-divider {
  margin: 0;
}
.language-sidebar {
  position: fixed;
  top: 0;
  right: 0;
  width: 300px;
  height: 100%;
  background-color: #ffffff;
  border-left: 1px solid #e6e6e6;
  padding: 20px;
  box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
  z-index: 1001;
  overflow-y: auto;
}

.language-sidebar .close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  font-size: 1.5rem;
  cursor: pointer;
}
.dropdown-item.logout {
  text-align: center;
  display: block;
  padding: 10px 0;
  margin: 0;
}
.admin-dropdown-toggle {
  color: #333;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.admin-dropdown-toggle:hover {
  color: #0073b1;
}

.admin-dropdown-menu {
  min-width: 220px;
  background-color: #ffffff;
  border: 1px solid #e1e9ee;
  border-radius: 8px;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.15);
  padding: 0;
}

.admin-dropdown-item {
  font-size: 14px;
  color: #333333;
  padding: 10px 15px;
  transition: background-color 0.2s ease, color 0.2s ease;
}

.admin-dropdown-item:hover {
  background-color: #f3f6f8;
  color: #0073b1;
}

.admin-dropdown-item:active {
  background-color: #e1ecf4;
  color: #005582;
}

.admin-dropdown-item i {
  margin-right: 8px;
  color: #0073b1;
}

@media (max-width: 768px) {
  .search-container {
    margin-left: 0;
    margin-bottom: 10px;
    width: 100%;
  }

  .nav-link {
    font-size: 0.8rem;
  }

  .navbar-nav {
    flex-direction: column;
    align-items: flex-start;
  }

  .dropdown-item {
    font-size: 0.8rem;
  }
}
</style>

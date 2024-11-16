<template>
  <LoadingSpinner :isLoading="loading" />
  <div v-if="!loading" class="container">
    <!-- Sidebar -->
    <div class="sidebar">
      <h3>Chọn game</h3>
      <ul class="game-list">
        <li
          v-for="game in gameOptions"
          :key="game"
          :class="{ active: selectedGameFilter === game }"
          @click="filterPostsByGame(game)"
        >
          <i class="fas fa-gamepad"></i> {{ game }}
        </li>
      </ul>

      <h3 class="mt-3">Trạng thái</h3>
      <ul class="status-list">
        <li
          :class="{ active: selectedStatusFilter === 'All' }"
          @click="filterPostsByStatus('All')"
        >
          <i class="fas fa-list"></i> Tất cả
        </li>
        <li
          :class="{ active: selectedStatusFilter === 'Đang bán' }"
          @click="filterPostsByStatus('Đang bán')"
        >
          <i class="fas fa-tag"></i> Đang bán
        </li>
        <li
          :class="{ active: selectedStatusFilter === 'Đã bán' }"
          @click="filterPostsByStatus('Đã bán')"
        >
          <i class="fas fa-check"></i> Đã bán
        </li>
      </ul>

      <h3 class="mt-3">Chủ sở hữu</h3>
      <ul class="owner-list">
        <li
          :class="{ active: selectedOwnerFilter === 'All' }"
          @click="filterPostsByOwner('All')"
        >
          <i class="fas fa-users"></i> Tất cả
        </li>
        <li
          :class="{ active: selectedOwnerFilter === 'Mine' }"
          @click="filterPostsByOwner('Mine')"
        >
          <i class="fas fa-user"></i> Tài khoản của tôi
        </li>
      </ul>
    </div>

    <!-- Content Container -->
    <div class="content-container">
      <div class="left-column">
        <!-- Carousel -->
        <div class="banner-carousel">
          <div
            class="carousel-slide"
            v-for="(slide, index) in slides"
            :key="index"
            :class="{ active: currentSlide === index }"
          >
            <img :src="slide.image" :alt="slide.alt" />
            <div class="carousel-caption">
              <h3>{{ slide.title }}</h3>
              <p>{{ slide.description }}</p>
            </div>
          </div>
          <button class="carousel-button prev" @click="prevSlide">&#10094;</button>
          <button class="carousel-button next" @click="nextSlide">&#10095;</button>
        </div>

        <h2 class="d-flex justify-content-center mt-3 mb-3">Các tài khoản hiện có</h2>
        <!-- Post Album Grid -->
        <div class="post-grid">
          <div
            class="post-album-container"
            v-for="(post, index) in paginatedPosts"
            :key="post.id || index"
          >
            <div class="user-info">
              <div class="user-details">
                <span class="post-type">{{ post.gameName }}</span>
              </div>
              <div
                :class="[
                  'status-badge',
                  post.status === 'Đang bán' ? 'status-selling' : 'status-sold',
                ]"
              >
                <i v-if="post.status === 'Đang bán'" class="fas fa-tag"></i>
                <i v-else class="fas fa-check"></i>
                {{ post.status }}
              </div>
            </div>
            <div class="post-images" v-if="post.images.length > 0">
              <img :src="getFullImageUrl(post.images[0])" alt="Product image" class="image-item" />
            </div>
            <div class="game-attributes">
              <div
                v-if="
                  post.gameName === 'Liên minh huyền thoại' ||
                  post.gameName === 'Tốc chiến'
                "
              >
                <p>Số Lượng Tướng: {{ post.championCount }}</p>
                <p>Số Lượng Trang Phục: {{ post.skinCount }}</p>
                <p>Hạng: {{ post.rank }}</p>
                <p class="price-item">{{ formatBalance(post.price) }}</p>
              </div>
              <div
                v-else-if="post.gameName === 'Pubg' || post.gameName === 'Valorant'"
              >
                <p>Số Lượng Skin Súng: {{ post.gunSkinCount }}</p>
                <p v-if="post.gameName === 'Pubg'">
                  Số Lượng Skin Nhân Vật: {{ post.humanSkinCount }}
                </p>
                <p v-if="post.gameName === 'Valorant'">
                  Số Lượng Nhân Vật: {{ post.championCount }}
                </p>
                <p>Xếp hạng: {{ post.rank }}</p>
                <p class="price-item">{{ formatBalance(post.price) }}</p>
              </div>
              <div v-else-if="post.gameName === 'Ngọc rồng online'">
                <p>Máy Chủ: {{ post.server }}</p>
                <p>Hành Tinh: {{ post.planet }}</p>
                <p class="price-item">{{ formatBalance(post.price) }}</p>
              </div>
            </div>
            <div class="post-footer">
              <div class="reactions">
                <button
                  class="reaction-button"
                  v-if="post.userId !== store.user?.id"
                  @click="openPurchaseModal(post)"
                >
                  <i class="fas fa-shopping-cart"></i>Mua
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Pagination Controls -->
        <div class="pagination" v-if="totalPages > 1">
          <button
            class="pagination-button"
            :disabled="currentPage === 1"
            @click="goToPage(currentPage - 1)"
          >
            &laquo; Trước
          </button>

          <button
            class="pagination-button"
            v-for="(page, idx) in paginationRange"
            :key="`page-${page}-${idx}`"
            :class="{ active: currentPage === page }"
            @click="goToPage(page)"
          >
            {{ page }}
          </button>

          <button
            class="pagination-button"
            :disabled="currentPage === totalPages"
            @click="goToPage(currentPage + 1)"
          >
            Tiếp &raquo;
          </button>
        </div>
      </div>

      <!-- Right Column -->
      <div class="right-column">
        <div class="page-container">
          <div class="profile-header">
            <img src="../assets/avatar.jpg" alt="User Avatar" />
            <h2>{{ userModel.fullName || fullname }}</h2>
            <div class="user-stats">
              <div class="stat-card1">
                <i class="fas fa-wallet"></i>
                <span>{{ formatBalance(userModel.balance) }}</span>
                <a href="/deposit" class="deposit-link"><i class="fas fa-plus"></i></a>
              </div>
              <div class="stat-card">
                <i class="fas fa-coins"></i>
                <span>{{ userModel.coin }}</span>
              </div>
              <div class="stat-card1">
                <i class="fas fa-shopping-cart"></i>
                <span>Tài khoản:  {{ mySellingAccounts }}</span>
                <a class="deposit-link" @click.prevent="openAddAccountModal" style="cursor: pointer;"><i class="fas fa-plus"></i></a>
              </div>
              <div class="stats-button-container">
                <a href="/statistical" class="stats-button">
                  <i class="fas fa-chart-line"></i> Thống kê
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Purchase Confirmation Modal -->
      <div class="modal-overlay" v-if="purchaseModalVisible">
        <div class="modal-content">
          <h2>Xác nhận mua tài khoản</h2>
          <p>Bạn có chắc chắn muốn mua tài khoản <strong>{{ selectedPost?.gameName }}</strong> của <strong>{{ selectedPost?.fullName }}</strong>?</p>
          <div class="modal-buttons">
            <button type="button" @click="confirmPurchase" class="confirm-button">Xác nhận</button>
            <button type="button" @click="closePurchaseModal" class="cancel-button">Hủy</button>
          </div>
        </div>
      </div>

      <!-- Error Modal -->
      <div class="modal-overlay error-modal" v-if="errorModalVisible">
        <div class="modal-content">
          <i class="fas fa-exclamation-triangle error-icon"></i>
          <h2>Lỗi</h2>
          <p>{{ errorModalMessage }}</p>
          <div class="modal-buttons">
            <button type="button" @click="closeErrorModal" class="close-button">
              Đóng
            </button>
          </div>
        </div>
      </div>

      <!-- Success Modal -->
      <div class="modal-overlay success-modal" v-if="successModalVisible">
        <div class="modal-content">
          <i class="fas fa-check-circle success-icon"></i>
          <h2>{{ successModalTitle }}</h2>
          <p>{{ successModalMessage }}</p>
          <div class="modal-buttons">
            <button type="button" @click="closeSuccessModal" class="close-button">
              Đóng
            </button>
          </div>
        </div>
      </div>

      <!-- Insufficient Funds Modal -->
      <div class="modal-overlay" v-if="insufficientFundsModalVisible">
        <div class="modal-content">
          <i class="fas fa-exclamation-triangle error-icon"></i>
          <h2>Không đủ tiền!</h2>
          <p>Bạn không đủ số dư để mua tài khoản này.</p>
          <div class="modal-buttons">
            <button type="button" @click="closeInsufficientFundsModal" class="close-button">
              Đóng
            </button>
            <a href="/deposit" class="deposit-link">
              <button type="button" class="deposit-button">Nạp tiền</button>
            </a>
          </div>
        </div>
      </div>

      <!-- Add Account Modal -->
      <div class="new-modal-overlay" v-if="addAccountModalVisible">
        <div class="new-modal-content">
          <h2>Thêm Tài Khoản Mới</h2>
          <form @submit.prevent="submitAddAccount">
            <!-- Lựa chọn game -->
            <div class="new-form-group">
              <label for="game">Chọn Game:</label>
              <select v-model="newAccount.gameName" id="game" required>
                <option disabled value="">Vui lòng chọn game</option>
                <option v-for="game in gameOptions" :key="game" :value="game">
                  {{ game }}
                </option>
              </select>
            </div>

            <!-- Tên tài khoản -->
            <div class="new-form-group">
              <label for="accountName">Tên Tài Khoản:</label>
              <input type="text" id="accountName" v-model="newAccount.AccountName" required />
            </div>

            <!-- Mật khẩu -->
            <div class="new-form-group">
              <label for="password">Mật Khẩu:</label>
              <input type="password" id="password" v-model="newAccount.Password" required />
            </div>

            <!-- Giá tài khoản -->
            <div class="new-form-group">
              <label for="price">Giá:</label>
              <input type="number" id="price" v-model.number="newAccount.Price" required min="0" />
            </div>

            <!-- Upload hình ảnh -->
            <label class="new-label-image-upload" for="image-upload">
              <i class="fas fa-image"></i> Chọn hình ảnh
            </label>
            <input
              type="file"
              id="image-upload"
              class="new-image-input"
              @change="handleImageUpload"
              multiple
            />

            <!-- Image Preview Section -->
            <div class="selected-images" v-if="images.length > 0">
              <div v-for="(img, idx) in images" :key="idx" class="selected-image">
                <img :src="img" alt="Selected Image" />
                <button type="button" @click="removeImage(idx)" class="remove-image-button">×</button>
              </div>
            </div>

            <!-- Các trường thông tin khác theo game -->
            <div v-if="newAccount.gameName === 'Liên minh huyền thoại' || newAccount.gameName === 'Tốc chiến'">
              <div class="new-form-group">
                <label for="championCount">Số Lượng Tướng:</label>
                <input type="number" id="championCount" v-model.number="newAccount.championCount" required min="0" />
              </div>
              <div class="new-form-group">
                <label for="skinCount">Số Lượng Trang Phục:</label>
                <input type="number" id="skinCount" v-model.number="newAccount.skinCount" required min="0" />
              </div>
              <div class="new-form-group">
                <label for="rank">Hạng:</label>
                <input type="text" id="rank" v-model="newAccount.rank" required />
              </div>
            </div>

            <div v-else-if="newAccount.gameName === 'Pubg' || newAccount.gameName === 'Valorant'">
              <div class="new-form-group">
                <label for="gunSkinCount">Số Lượng Skin Súng:</label>
                <input type="number" id="gunSkinCount" v-model.number="newAccount.gunSkinCount" required min="0" />
              </div>
              <div v-if="newAccount.gameName === 'Pubg'" class="new-form-group">
                <label for="humanSkinCount">Số Lượng Skin Nhân Vật:</label>
                <input type="number" id="humanSkinCount" v-model.number="newAccount.humanSkinCount" required min="0" />
              </div>
              <div v-if="newAccount.gameName === 'Valorant'" class="new-form-group">
                <label for="championCount">Số Lượng Nhân Vật:</label>
                <input type="number" id="championCount" v-model.number="newAccount.championCount" required min="0" />
              </div>
              <div class="new-form-group">
                <label for="rank">Xếp hạng:</label>
                <input type="text" id="rank" v-model="newAccount.rank" required />
              </div>
            </div>

            <div v-else-if="newAccount.gameName === 'Ngọc rồng online'">
              <div class="new-form-group">
                <label for="server">Máy Chủ:</label>
                <input type="text" id="server" v-model="newAccount.server" required />
              </div>
              <div class="new-form-group">
                <label for="planet">Hành Tinh:</label>
                <input type="text" id="planet" v-model="newAccount.planet" required />
              </div>
            </div>

            <!-- Nút xác nhận và hủy -->
            <div class="new-modal-buttons">
              <button type="submit" class="new-confirm-button">Thêm Tài Khoản</button>
              <button type="button" @click="closeAddAccountModal" class="new-cancel-button">Hủy</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import { ref, onMounted, computed, onUnmounted } from 'vue';
import PurchasedgameaccountAPI from '@/api/purchased.api';
import { userStore } from '@/stores/auth';
import getallgameaccountAPI from '@/api/getallgameaccount.api';
import getfullname from '@/api/getfullname.api';
import profile from '@/api/profile.api';
import * as addGameAccountAPI from '@/api/addgameaccount.api'; 

// Initialize user store
const store = userStore();

// Modal Visibility States
const approvalModalVisible = ref(false);
const successModalVisible = ref(false);

// Success Modal Titles and Messages
const successModalTitle = ref<string>('');
const successModalMessage = ref<string>('');


// Modal Control Methods
const openErrorModal = (message: string): void => {
  successModalVisible.value = false; // Close success modal if open
  errorModalMessage.value = message;
  errorModalVisible.value = true;
};

const closeErrorModal = (): void => {
  errorModalVisible.value = false;
  errorModalMessage.value = '';
};

const openSuccessModal = (title: string, message: string): void => {
  errorModalVisible.value = false; // Close error modal if open
  successModalTitle.value = title;
  successModalMessage.value = message;
  successModalVisible.value = true;
};

const closeSuccessModal = (): void => {
  successModalVisible.value = false;
  successModalTitle.value = '';
  successModalMessage.value = '';
};

// Error Modal Control
const errorModalVisible = ref<boolean>(false);
const errorModalMessage = ref<string>('');

// Reactive variables
const loading = ref(true);
const selectedGameFilter = ref<string>('All');
const gameOptions = ref<string[]>([
  'All',
  'Liên minh huyền thoại',
  'Pubg',
  'Ngọc rồng online',
  'Valorant',
  'Tốc chiến',
]);
const selectedStatusFilter = ref<string>('All');
const selectedOwnerFilter = ref<string>('All');
const posts = ref<any[]>([]);
const purchaseModalVisible = ref(false); 
const selectedPost = ref<any | null>(null);
const insufficientFundsModalVisible = ref<boolean>(false);
const addAccountModalVisible = ref<boolean>(false);
const imageFiles = ref<File[]>([]);
const images = ref<string[]>([]);

// New account form data
const newAccount = ref({
  gameName: '',
  AccountName: '',
  Password: '',
  Price: 0,
  Image: '',
  // Game-specific fields
  championCount: 0,
  skinCount: 0,
  rank: '',
  gunSkinCount: 0,
  humanSkinCount: 0,
  server: '',
  planet: '',
});

// Pagination variables
const currentPage = ref<number>(1);
const itemsPerPage: number = 21;

// Modal control methods
const closePurchaseModal = (): void => {
  purchaseModalVisible.value = false;
};

const closeInsufficientFundsModal = (): void => {
  insufficientFundsModalVisible.value = false;
};

const openAddAccountModal = (): void => {
  addAccountModalVisible.value = true;
};

const closeAddAccountModal = (): void => {
  addAccountModalVisible.value = false;
  resetAddAccountForm();
};

// Reset the add account form
const resetAddAccountForm = (): void => {
  newAccount.value = {
    gameName: '',
    AccountName: '',
    Password: '',
    Price: 0,
    Image: '',
    championCount: 0,
    skinCount: 0,
    rank: '',
    gunSkinCount: 0,
    humanSkinCount: 0,
    server: '',
    planet: '',
  };
  imageFiles.value = [];
  images.value = [];
};

// Handle image upload
const handleImageUpload = (event: Event): void => {
  const files = (event.target as HTMLInputElement).files;
  if (files) {
    Array.from(files).forEach((file: File) => {
      imageFiles.value.push(file);
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target && e.target.result) {
          images.value.push(e.target.result as string);
        }
      };
      reader.readAsDataURL(file);
    });
  }
};

// Remove selected image
const removeImage = (index: number): void => {
  imageFiles.value.splice(index, 1);
  images.value.splice(index, 1);
};

// User information
const userModel = ref({
  fullName: '',
  email: '',
  balance: '0',
  coin: 0,
  createdDate: null,
});

// Fetch user profile
const fetchUserProfile = async (shouldHandleError: boolean = true) => {
  try {
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;
    if (userId) {
      const response = await profile.getByIdProfile(userId);
      if (response && response.data && response.data.result.isSuccess) {
        const userData = response.data.result.data;
        userModel.value = {
          fullName: userData.fullName || '',
          email: userData.email || '',
          balance: userData.balance || '0',
          coin: userData.coin || 0,
          createdDate: userData.createdDate || '',
        };
      }
    } else {
      console.error('User ID not found.');
      if (shouldHandleError) {
        openErrorModal('User ID not found.');
      }
    }
  } catch (error) {
    console.error('Error fetching user profile:', error);
    if (shouldHandleError) {
      openErrorModal('Đã xảy ra lỗi khi lấy thông tin người dùng.');
    }
  }
};

// Full name of the user
const fullname = computed<string>(() => store.user?.fullName || 'Chưa đăng nhập');

// Filtering methods
const filterPostsByStatus = (status: string) => {
  selectedStatusFilter.value = status;
  currentPage.value = 1;
};

const filterPostsByGame = (game: string) => {
  selectedGameFilter.value = game;
  currentPage.value = 1;
};

const filterPostsByOwner = (owner: string) => {
  selectedOwnerFilter.value = owner;
  currentPage.value = 1;
};

// Format balance
const formatBalance = (balanceString: string): string => {
  const balance = parseFloat(balanceString);
  if (isNaN(balance)) return '0 VNĐ';
  return balance.toLocaleString('vi-VN') + ' VNĐ';
};

// Open Purchase Modal
const openPurchaseModal = (post: any): void => {
  selectedPost.value = post;
  purchaseModalVisible.value = true;
};

// Confirm Purchase
const confirmPurchase = async (): Promise<void> => {
  if (!selectedPost.value) return;
  purchaseModalVisible.value = false;

  // Check if user has enough balance
  const userBalance = parseFloat(userModel.value.balance);
  const postPrice = parseFloat(selectedPost.value.price);
  if (isNaN(userBalance) || isNaN(postPrice)) {
    openErrorModal('Lỗi dữ liệu.');
    return;
  }

  if (userBalance < postPrice) {
    insufficientFundsModalVisible.value = true;
    return;
  }

  try {
    const gameAccountId = selectedPost.value.id;
    const userId = store.user?.id;
    if (!userId || !gameAccountId) {
      openErrorModal('Lỗi dữ liệu.');
      return;
    }

    const gameName = selectedPost.value.gameName;
    let response: any;

    switch (gameName) {
      case 'Liên minh huyền thoại':
        response = await PurchasedgameaccountAPI.purchasedLOL(gameAccountId, userId);
        break;
      case 'Pubg':
        response = await PurchasedgameaccountAPI.purchasedPubg(gameAccountId, userId);
        break;
      case 'Ngọc rồng online':
        response = await PurchasedgameaccountAPI.purchasedNgocRong(gameAccountId, userId);
        break;
      case 'Tốc chiến':
        response = await PurchasedgameaccountAPI.purchasedTocChien(gameAccountId, userId);
        break;
      case 'Valorant':
        response = await PurchasedgameaccountAPI.purchasedValorant(gameAccountId, userId);
        break;
      default:
        return openErrorModal('Game không hợp lệ cho việc mua hàng.');
    }

    if (response && response.data.result.isSuccess) {
      openSuccessModal('Mua tài khoản thành công!', 'Bạn đã mua tài khoản thành công.');
      await fetchAllPosts(false);
      await fetchUserProfile(false);
    } else {
      openErrorModal(response.data.result.message || 'Mua hàng thất bại.');
    }
  } catch (error) {
    console.error('Lỗi khi mua hàng:', error);
    openErrorModal('Vui lòng đăng nhập!.');
  }
};

// Submit Add Account
const submitAddAccount = async (): Promise<void> => {
  try {
    // Kiểm tra các trường bắt buộc
    if (
      !newAccount.value.gameName ||
      !newAccount.value.AccountName ||
      !newAccount.value.Password ||
      !newAccount.value.Price ||
      imageFiles.value.length === 0
    ) {
      openErrorModal('Vui lòng điền đầy đủ các trường bắt buộc.');
      return;
    }

    // Tạo FormData
    const formDataToSend = new FormData();
    formDataToSend.append('GameName', newAccount.value.gameName);
    formDataToSend.append('AccountName', newAccount.value.AccountName);
    formDataToSend.append('Password', newAccount.value.Password);
    formDataToSend.append('Price', newAccount.value.Price.toString());

    // Đính kèm hình ảnh
    imageFiles.value.forEach((file) => {
      formDataToSend.append('Files', file);
    });

    // Đính kèm các trường thông tin cụ thể theo game
    switch (newAccount.value.gameName) {
      case 'Liên minh huyền thoại':
        formDataToSend.append('ChampionCount', newAccount.value.championCount.toString());
        formDataToSend.append('SkinCount', newAccount.value.skinCount.toString());
        formDataToSend.append('Rank', newAccount.value.rank);
        await addGameAccountAPI.addLol_Game(formDataToSend);
        break;

      case 'Tốc chiến':
        formDataToSend.append('ChampionCount', newAccount.value.championCount.toString());
        formDataToSend.append('SkinCount', newAccount.value.skinCount.toString());
        formDataToSend.append('Rank', newAccount.value.rank);
        await addGameAccountAPI.addTocChien_Game(formDataToSend);
        break;

      case 'Pubg':
        formDataToSend.append('GunSkinCount', newAccount.value.gunSkinCount.toString());
        formDataToSend.append('HumanSkinCount', newAccount.value.humanSkinCount.toString());
        formDataToSend.append('Rank', newAccount.value.rank);
        await addGameAccountAPI.addPubg_Game(formDataToSend);
        break;

      case 'Valorant':
        formDataToSend.append('GunSkinCount', newAccount.value.gunSkinCount.toString());
        formDataToSend.append('ChampionCount', newAccount.value.championCount.toString());
        formDataToSend.append('Rank', newAccount.value.rank);
        await addGameAccountAPI.addValorant_Game(formDataToSend);
        break;

      case 'Ngọc rồng online':
        formDataToSend.append('Server', newAccount.value.server);
        formDataToSend.append('Planet', newAccount.value.planet);
        await addGameAccountAPI.addNgocRong_Game(formDataToSend);
        break;

      default:
        openErrorModal('Game không hợp lệ.');
        return;
    }
    closeAddAccountModal;
    openSuccessModal(
      'Thêm tài khoản thành công!',
      'Tài khoản của bạn đã được thêm vào và đang chờ admin duyệt.'
    );
    await fetchAllPosts(false);
    await fetchUserProfile(false);
  } catch (error) {
    console.error('Lỗi khi gọi API:', error);
    openErrorModal('Có lỗi xảy ra khi thêm tài khoản.');
  }
};


const fetchData = async (): Promise<void> => {
  loading.value = true;
  await new Promise((resolve) => setTimeout(resolve, 2000));
  loading.value = false;
};

const getFullImageUrl = (imageString: string | null): string => {
  if (!imageString) {
    return 'https://via.placeholder.com/550x500';
  }
  const baseUrl = 'https://localhost:7071/';
  const firstImage = imageString.split(';')[0];
  return `${baseUrl}${firstImage}`;
};

const filteredPosts = computed(() => {
  let postsFiltered = posts.value.filter(
    (item) => item.status !== 'Đang chờ duyệt'
  );

  if (selectedGameFilter.value !== 'All') {
    postsFiltered = postsFiltered.filter(
      (post) => post.gameName === selectedGameFilter.value
    );
  }

  if (selectedStatusFilter.value !== 'All') {
    postsFiltered = postsFiltered.filter(
      (post) => post.status === selectedStatusFilter.value
    );
  }

  if (selectedOwnerFilter.value === 'Mine') {
    postsFiltered = postsFiltered.filter(
      (post) => post.userId === store.user?.id
    );
  }

  postsFiltered.sort((a, b) => {
    if (a.status === 'Đang bán' && b.status !== 'Đang bán') {
      return -1;
    }
    if (a.status !== 'Đang bán' && b.status === 'Đang bán') {
      return 1;
    }
    return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
  });

  return postsFiltered;
});

const totalPages = computed<number>(() => {
  return Math.ceil(filteredPosts.value.length / itemsPerPage);
});

const paginatedPosts = computed<any[]>(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filteredPosts.value.slice(start, end);
});

const paginationRange = computed<Array<number | string>>(() => {
  const range: Array<number | string> = [];
  const total = totalPages.value;
  const current = currentPage.value;
  const delta = 2;

  for (let i = 1; i <= total; i++) {
    if (
      i === 1 ||
      i === total ||
      (i >= current - delta && i <= current + delta)
    ) {
      range.push(i);
    } else if (
      i === current - delta - 1 ||
      i === current + delta + 1
    ) {
      range.push('...');
    }
  }

  const uniqueRange: Array<number | string> = [];
  range.forEach((page) => {
    if (page === '...' && uniqueRange[uniqueRange.length - 1] === '...') {
      return;
    }
    uniqueRange.push(page);
  });

  return uniqueRange;
});

const goToPage = (page: number | string): void => {
  if (page === '...') return;

  if (typeof page === 'number') {
    if (page < 1) {
      currentPage.value = 1;
    } else if (page > totalPages.value) {
      currentPage.value = totalPages.value;
    } else {
      currentPage.value = page;
    }
  }
};

const fetchAllPosts = async (shouldHandleError: boolean = true): Promise<void> => {
  try {
    const [
      lolResponse,
      ngocRongResponse,
      pubgResponse,
      tocChienResponse,
      valorantResponse,
    ] = await Promise.all([
      getallgameaccountAPI.getAllLOL(),
      getallgameaccountAPI.getAllNgocRong(),
      getallgameaccountAPI.getAllPubg(),
      getallgameaccountAPI.getAllTocChien(),
      getallgameaccountAPI.getAllValorant(),
    ]);

    const rawPosts = [
      ...(lolResponse.data.result.data || []),
      ...(ngocRongResponse.data.result.data || []),
      ...(pubgResponse.data.result.data || []),
      ...(tocChienResponse.data.result.data || []),
      ...(valorantResponse.data.result.data || []),
    ];

    posts.value = await Promise.all(
      rawPosts.map(async (post) => {
        let fullName = '';
        try {
          switch (post.gameName) {
            case 'Liên minh huyền thoại':
              fullName = (await getfullname.getfullnameLol(post.id)).data.result.data;
              break;
            case 'Ngọc rồng online':
              fullName = (await getfullname.getfullnameNgocRong(post.id)).data.result.data;
              break;
            case 'Pubg':
              fullName = (await getfullname.getfullnamePubg(post.id)).data.result.data;
              break;
            case 'Tốc chiến':
              fullName = (await getfullname.getfullnameTocChien(post.id)).data.result.data;
              break;
            case 'Valorant':
              fullName = (await getfullname.getfullnameValorant(post.id)).data.result.data;
              break;
            default:
              console.warn('Unknown game type:', post.gameName);
          }
        } catch (error) {
          console.error(`Error fetching full name for post ${post.id}:`, error);
        }
        return {
          ...post,
          fullName,
          images: post.image ? post.image.split(';') : [],
          createdAt: post.createdDate,
          updatedAt: post.updatedDate,
        };
      })
    );

  } catch (error) {
    console.error('Error fetching posts:', error);
    if (shouldHandleError) {
      openErrorModal('Đã xảy ra lỗi khi lấy dữ liệu bài đăng.');
    }
  }
};

const slides = ref([
  {
    image: 'https://cmsassets.rgpub.io/sanity/images/dsfx7636/game_data/031444bd205c751c4986f5f4bf0902993ccaeec8-1280x720.jpg',
    alt: 'Promotion 1',
    title: 'Khuyến mãi Đặc Biệt',
    description: 'Giảm giá lên đến 50% cho các tài khoản mới!',
  },
  {
    image: 'https://preview.redd.it/empyrean-akali-splash-art-v0-vn1gq9j6kwuc1.jpeg?width=1080&crop=smart&auto=webp&s=ae2153820c5fc71d49a990df26ad75311ca0261b',
    alt: 'Promotion 2',
    title: 'Mua Sớm Nhận Thêm Quà',
    description: 'Nhận thêm coin khi mua tài khoản trong tuần này.',
  },
  {
    image: 'https://i.vietgiaitri.com/2024/9/18/them-mot-bang-chung-chi-mang-cho-thay-aatrox-dang-la-vi-tuong-overrated-nhat-cua-riot-76d-7270064.webp',
    alt: 'Promotion 3',
    title: 'Tài Khoản Độc Quyền',
    description: 'Mua ngay các tài khoản game hiếm có.',
  },
]);

const currentSlide = ref(0);
const nextSlide = () => {
  currentSlide.value = (currentSlide.value + 1) % slides.value.length;
};

const prevSlide = () => {
  currentSlide.value = (currentSlide.value - 1 + slides.value.length) % slides.value.length;
};

let slideInterval: number;
const startSlideShow = () => {
  slideInterval = window.setInterval(() => {
    nextSlide();
  }, 5000);
};

const stopSlideShow = () => {
  clearInterval(slideInterval);
};

const mySellingAccounts = computed<number>(() => {
  return posts.value.filter(post => post.userId === store.user?.id).length;
});

onMounted(async () => {
  await fetchAllPosts();
  await fetchUserProfile();
  store.init();
  startSlideShow();
  fetchData();
});
onUnmounted(() => {
  stopSlideShow();
});
</script>

<style scoped>
.container {
  display: flex;
  align-items: flex-start;
  justify-content: center; 
  max-width: 1700px;
  margin: 0 auto; 
  padding: 20px;
}

.sidebar {
  background-color: #ffffff;
  border-radius: 12px;
  width: 250px;
  padding: 15px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  position: fixed;
  top: 68px; 
  left: 20px;
  height: calc(100vh - 88px);
  overflow-y: auto;
  z-index: 100;
}

.sidebar h3 {
  font-size: 18px;
  font-weight: bold;
  color: #333;
  margin-bottom: 10px;
}

.game-list,
.status-list,
.owner-list { /* Include .owner-list with existing lists */
  list-style: none;
  padding: 0;
  margin: 0;
}

.game-list li,
.status-list li,
.owner-list li { /* Include .owner-list li with existing lis */
  font-size: 14px;
  color: #0073b1;
  cursor: pointer;
  padding: 10px 15px;
  border-radius: 8px;
  transition: background-color 0.3s, color 0.3s; /* Smooth Transition */
  margin-bottom: 5px;
}

.game-list li:hover,
.status-list li:hover,
.owner-list li:hover { /* Add hover for .owner-list li */
  background-color: #e1f5fe;
}

.active {
  background-color: #9dddff;
  color: white;
}

/* Right Column Styles */
.right-column {
  width: 250px;
  position: fixed; /* Fixed position */
  top: 68px; /* Distance from top (assuming navbar height is 68px) */
  right: 20px; /* Distance from right */
  height: calc(100vh - 88px); /* Height to prevent overflow */
  overflow-y: auto; /* Vertical scroll if content is long */
  z-index: 100; /* Ensure right-column is above other elements */
  background-color: #f9f9f9; /* Light background color */
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

.page-container {
  background-color: white;
  border-radius: 10px;
  padding: 20px;
  text-align: center;
}

.profile-header {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.profile-header img {
  width: 80px; /* Increase avatar size */
  height: 80px;
  border-radius: 50%;
  margin-bottom: 15px;
  object-fit: cover;
  border: 2px solid #0073b1; /* Border around avatar */
  transition: transform 0.3s, box-shadow 0.3s; /* Add transition for effects */
}

.profile-header img:hover {
  transform: scale(1.05); /* Slight zoom on hover */
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); /* Add shadow on hover */
}

.profile-header h2 {
  margin: 10px 0;
  font-size: 15px; /* Increase font size */
  font-weight: bold;
  color: #333;
}

.user-stats {
  display: flex;
  flex-direction: column;
  gap: 10px;
  width: 100%;
  margin-top: 15px;
}

.stat-card,
.stat-card1 {
  display: flex;
  align-items: center;
  background-color: #eef6fb; /* Light background for cards */
  padding: 5px 10px;
  border-radius: 8px;
  transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s;
}

.stat-card1 {
  justify-content: space-between; /* Space between elements */
}

.stat-card i,
.stat-card1 i {
  font-size: 15px;
  color: #0073b1;
  margin-right: 10px;
}

.stat-card span,
.stat-card1 span {
  font-size: 12px;
  color: #333;
  font-weight: 500;
}

.stat-card:hover,
.stat-card1:hover {
  background-color: #dceefc;
  transform: translateY(-3px);
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.deposit-link {
  color: #0073b1;
  text-decoration: none;
  font-size: 15px;
}

.deposit-link:hover {
  color: #005a8c;
}

.stats-button-container {
  margin-top: 10px;
}

.stats-button {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #0073b1;
  color: #ffffff;
  padding: 8px 12px;
  border-radius: 6px;
  text-decoration: none;
  font-size: 14px;
  transition: background-color 0.3s, transform 0.2s;
}

.stats-button:hover {
  background-color: #005a8c;
  transform: translateY(-2px);
}

.stats-button i {
  margin-right: 5px;
}

/* Content Container Styles */
.content-container {
  flex-grow: 1;
  margin: 0 340px; /* Left and right: 250px width + 20px spacing */
  display: flex;
  flex-direction: column;
  max-width: 1200px; /* Adjust as needed */
}

.left-column {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.left-column h2 {
  font-size: 29px;
  color: #333;
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* Slightly darker background */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background-color: #ffffff;
  padding: 30px;
  width: 500px; /* Adjust width to be consistent */
  border-radius: 8px; /* Slightly less rounded corners */
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  position: relative;
  animation: fadeIn 0.3s ease-out;
}

/* LinkedIn-like Typography */
.modal-content h2 {
  margin-bottom: 20px;
  font-size: 20px;
  color: #0073b1;
  text-align: center;
  font-weight: 600;
}

.modal-content p {
  font-size: 14px;
  color: #333;
  text-align: center;
  margin-bottom: 20px;
}

.modal-buttons {
  display: flex;
  justify-content: center;
  gap: 15px;
}

.modal-buttons button {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.confirm-button {
  background-color: #0073b1;
  color: #ffffff;
}

.confirm-button:hover {
  background-color: #005580;
}

.cancel-button {
  background-color: #e0e0e0;
  color: #333333;
}

.cancel-button:hover {
  background-color: #c0c0c0;
}

/* Success Modal */
.success-modal .modal-content {
  text-align: center;
}

.success-modal .success-icon {
  font-size: 40px;
  color: #28a745;
  margin-bottom: 10px;
}

/* Error Modal */
.error-modal .modal-content {
  text-align: center;
}

.error-modal .error-icon {
  font-size: 40px;
  color: #dc3545;
  margin-bottom: 10px;
}

/* Insufficient Funds Modal */
.deposit-button {
  background-color: #0073b1;
  color: #ffffff;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.deposit-button:hover {
  background-color: #005580;
}

/* Add Account Modal */
.new-modal-overlay { 
  position: fixed; 
  top: 0; 
  left: 0; 
  width: 100%; 
  height: 100%; 
  background-color: rgba(0, 0, 0, 0.5); /* Slightly darker background */ 
  display: flex; 
  justify-content: center; 
  align-items: center; 
  z-index: 1000; 
  overflow-y: auto; /* Enable vertical scrolling */ 
} 

/* Modal content styling */ 
.new-modal-content { 
  background-color: #ffffff; 
  padding: 30px; 
  max-width: 700px; /* Larger width for modal */ 
  width: 90%; /* Responsive width */ 
  max-height: 80vh; /* Limit modal height */ 
  overflow-y: auto; /* Scroll within modal if content exceeds height */ 
  border-radius: 8px; 
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2); 
  animation: fadeIn 0.3s ease-out; 
  display: flex; 
  flex-direction: column; 
} 

/* Modal header styling */ 
.new-modal-content h2 { 
  margin-bottom: 20px; 
  font-size: 20px; 
  color: #0073b1; 
  text-align: center; 
  font-weight: 600;
} 

/* Form styling */ 
.new-form-group { 
  display: flex; 
  flex-direction: column; 
  margin-bottom: 15px;
} 

/* Label styling */ 
.new-form-group label { 
  font-weight: 600; 
  margin-bottom: 5px; 
  color: #333; 
} 

/* Input and select styling */ 
.new-form-group input, .new-form-group select { 
  padding: 10px 12px; 
  font-size: 14px; 
  border: 1px solid #ccc; 
  border-radius: 4px; 
  transition: border-color 0.3s; 
} 

.new-form-group input:focus, .new-form-group select:focus { 
  border-color: #0073b1; /* Highlight on focus */ 
  outline: none;
} 

/* Image upload button styling */ 
.new-image-input { 
  display: none; 
} 

.new-label-image-upload { 
  display: inline-flex; 
  align-items: center; 
  color: #0073b1; 
  cursor: pointer; 
  margin-top: 10px; 
  font-size: 14px;
} 

.new-label-image-upload i { 
  margin-right: 5px; 
  font-size: 18px; 
}

/* Image Preview Styles */
.selected-images {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-top: 10px;
}

.selected-image {
  position: relative;
  width: 80px;
  height: 80px;
}

.selected-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 4px;
}

.remove-image-button {
  position: absolute;
  top: -5px;
  right: -5px;
  background-color: #dc3545;
  border: none;
  color: #ffffff;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  cursor: pointer;
  font-weight: bold;
}

.remove-image-button:hover {
  background-color: #c82333;
}

/* Modal buttons styling */
.new-modal-buttons {
  display: flex;
  justify-content: center;
  gap: 15px;
  margin-top: 20px;
}

/* Confirm button styling */
.new-confirm-button {
  padding: 10px 25px;
  background-color: #0073b1;
  color: #ffffff;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.new-confirm-button:hover {
  background-color: #005580;
}

/* Cancel button styling */
.new-cancel-button {
  padding: 10px 25px;
  background-color: #dc3545;
  color: #ffffff;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.new-cancel-button:hover {
  background-color: #c82333;
}

/* Post Grid Styles */
.post-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.post-album-container {
  position: relative;
  background-color: #ffffff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 12px;
  height: 330px;
  width: 250px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  overflow: hidden;
  transition: transform 0.3s, box-shadow 0.3s;
}

.post-album-container:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
}

.status-badge {
  position: absolute;
  top: 7px;
  right: 7px;
  padding: 6px 12px;
  border-radius: 3px;
  font-size: 7px;
  font-weight: bold;
  color: #ffffff;
  background-color: #28a745;
  z-index: 1;
  opacity: 0.95;
  transition: transform 0.3s ease, opacity 0.3s ease;
  display: flex;
  align-items: center;
}

.status-selling {
  background-color: #0073b1;
}

.status-sold {
  background-color: #dc3545;
}

.status-badge:hover {
  transform: scale(1.05);
  opacity: 1;
}

.status-badge i {
  font-size: 12px;
  margin-right: 3px;
}

.user-info {
  display: flex;
  align-items: center;
}

.user-details {
  font-size: 12px;
  color: #0073b1;
  line-height: 1.2;
  font-weight: 700;
  display: flex;
}

.post-type {
  font-weight: bold;
}

.post-images {
  margin-top: 8px;
  position: relative;
}

.image-item {
  width: 100%;
  height: 120px;
  object-fit: cover;
  border-radius: 4px;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.image-item:hover {
  transform: scale(1.05);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3);
}

.game-attributes {
  font-size: 12px;
  color: #555;
  margin-top: 8px;
}

.game-attributes p {
  margin: 2px 0;
}

.price-item {
  color: #0073b1;
  font-weight: 700;
  text-align: end;
  font-size: 14px;
}

.post-footer {
  display: flex;
  justify-content: end;
  align-items: center;
  padding-top: 8px;
}

.reactions {
  display: flex;
  justify-content: center;
}

.reaction-button {
  display: flex;
  align-items: center;
  padding: 8px 12px;
  background-color: #0073b1;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 8px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.2s;
}

.reaction-button:hover {
  background-color: #005580;
}

.reaction-button:active {
  transform: scale(0.98); /* Press effect */
}

.reaction-button i {
  font-size: 14px;
  margin-right: 5px;
}

/* Pagination Styles */
.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
  gap: 5px;
  flex-wrap: wrap;
}

.pagination-button {
  padding: 8px 12px;
  background-color: #ffffff;
  border: 1px solid #0073b1;
  color: #0073b1;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}

.pagination-button:hover {
  background-color: #0073b1;
  color: #ffffff;
}

.pagination-button:disabled {
  background-color: #e0e0e0;
  color: #aaa;
  cursor: not-allowed;
}

.pagination-button.active {
  background-color: #0073b1;
  color: #ffffff;
}

/* Animations */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .post-album-container {
    width: calc(50% - 10px);
  }

  .container {
    flex-direction: column;
    align-items: center;
    padding: 10px; /* Change padding for small screens */
  }

  .sidebar,
  .right-column {
    width: 100%;
    max-width: 600px;
    margin-bottom: 20px;
    position: static; /* Remove fixed on small screens */
    height: auto;
    overflow-y: visible;
  }

  .content-container {
    max-width: 100%;
    margin: 0; /* No left and right margin */
  }

  .modal-content {
    width: 90%;
    padding: 20px;
  }

  .modal-content h2 {
    font-size: 18px;
  }

  .modal-buttons button {
    font-size: 12px;
  }

  /* Adjust status-badge for smaller screens */
  .status-badge {
    top: 8px;
    right: 8px;
    padding: 4px 8px;
    font-size: 10px;
    border-radius: 10px;
  }

  .status-badge i {
    font-size: 10px;
    margin-right: 3px;
  }

  /* Add styles for owner filter on small screens */
  .owner-list li {
    font-size: 12px;
    padding: 8px 12px;
  }

  /* Adjust profile section on small screens */
  .profile-header img {
    width: 60px;
    height: 60px;
  }

  .profile-header h2 {
    font-size: 18px;
  }

  .stat-card span,
  .stat-card1 span {
    font-size: 14px;
  }

  .stats-button {
    font-size: 14px;
    padding: 8px 12px;
  }

  /* Carousel adjustments */
  .banner-carousel {
    height: 200px;
  }

  .carousel-caption h3 {
    font-size: 1.2em;
  }

  .carousel-caption p {
    font-size: 0.9em;
  }
}

.banner-carousel {
  position: relative;
  width: 100%;
  max-width: 1200px;
  height: 300px;
  margin: 0 auto 20px auto;
  overflow: hidden;
  border-radius: 10px;
}

.carousel-slide {
  position: absolute;
  width: 100%;
  height: 100%;
  opacity: 0;
  transition: opacity 1s ease-in-out;
}

.carousel-slide.active {
  opacity: 1;
}

.carousel-slide img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.carousel-caption {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  color: #fff;
  background: rgba(0, 0, 0, 0.5);
  padding: 10px 20px;
  border-radius: 5px;
  text-align: center;
}

.carousel-caption h3 {
  margin: 0;
  font-size: 1.5em;
}

.carousel-caption p {
  margin: 5px 0 0 0;
  font-size: 1em;
}

.carousel-button {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background-color: rgba(0,0,0,0.5);
  border: none;
  color: #fff;
  padding: 10px;
  cursor: pointer;
  border-radius: 50%;
  font-size: 1.2em;
  transition: background-color 0.3s;
}

.carousel-button:hover {
  background-color: rgba(0,0,0,0.8);
}

.carousel-button.prev {
  left: 10px;
}

.carousel-button.next {
  right: 10px;
}

/* Responsive adjustments for carousel */
@media (max-width: 768px) {
  .banner-carousel {
    height: 200px;
  }

  .carousel-caption h3 {
    font-size: 1.2em;
  }

  .carousel-caption p {
    font-size: 0.9em;
  }
}
</style>

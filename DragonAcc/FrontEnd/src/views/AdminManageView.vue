<template>
  <LoadingSpinner :isLoading="loading" />
  <div v-if="!loading" class="container">
    <h2 class="title">Quản lý tài khoản</h2>
    
    <div class="filter-section">
      <label for="status-filter">Lọc theo trạng thái:</label>
      <select id="status-filter" v-model="selectedStatus">
        <option value="All">Tất cả</option>
        <option value="Đang chờ duyệt">Đang chờ duyệt</option>
        <option value="Đang bán">Đang bán</option>
        <option value="Đã bán">Đã bán</option>
      </select>
    </div>

    <div class="tabs">
      <button
        v-for="game in gameTabs"
        :key="game.key"
        :class="['tab-button', { active: activeTab === game.key }]"
        @click="activeTab = game.key"
      >
        {{ game.name }}
      </button>
    </div>

    <div class="tab-content">
      <!-- League of Legends Accounts -->
      <div v-if="activeTab === 'lol'" class="account-section">
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Price</th>
              <th>Champion Count</th>
              <th>Skin Count</th>
              <th>Rank</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="account in filteredLolAccounts" :key="account.id">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ formatBalance(account.price) }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="openApprovalModal('lol', account)"
                >
                  Duyệt
                </button>
                <button
                  class="btn-view-images"
                  @click="openViewImagesModal(account)"
                >
                  Xem Hình Ảnh
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Ngọc Rồng Online Accounts -->
      <div v-if="activeTab === 'ngocRong'" class="account-section">
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Price</th>
              <th>Server</th>
              <th>Planet</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="account in filteredNgocRongAccounts" :key="account.id">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ formatBalance(account.price) }}</td>
              <td>{{ account.server }}</td>
              <td>{{ account.planet }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="openApprovalModal('ngocRong', account)"
                >
                  Duyệt
                </button>
                <button
                  class="btn-view-images"
                  @click="openViewImagesModal(account)"
                >
                  Xem Hình Ảnh
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pubg Accounts -->
      <div v-if="activeTab === 'pubg'" class="account-section">
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Price</th>
              <th>Human Skin Count</th>
              <th>Gun Skin Count</th>
              <th>Rank</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="account in filteredPubgAccounts" :key="account.id">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ formatBalance(account.price) }}</td>
              <td>{{ account.humanSkinCount }}</td>
              <td>{{ account.gunSkinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="openApprovalModal('pubg', account)"
                >
                  Duyệt
                </button>
                <button
                  class="btn-view-images"
                  @click="openViewImagesModal(account)"
                >
                  Xem Hình Ảnh
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Valorant Accounts -->
      <div v-if="activeTab === 'valorant'" class="account-section">
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Price</th>
              <th>Gun Skin Count</th>
              <th>Champion Count</th>
              <th>Rank</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="account in filteredValorantAccounts" :key="account.id">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ formatBalance(account.price) }}</td>
              <td>{{ account.gunSkinCount }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="openApprovalModal('valorant', account)"
                >
                  Duyệt
                </button>
                <button
                  class="btn-view-images"
                  @click="openViewImagesModal(account)"
                >
                  Xem Hình Ảnh
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Tốc Chiến Accounts -->
      <div v-if="activeTab === 'tocChien'" class="account-section">
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Price</th>
              <th>Champion Count</th>
              <th>Skin Count</th>
              <th>Rank</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="account in filteredTocChienAccounts" :key="account.id">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ formatBalance(account.price) }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="openApprovalModal('tocChien', account)"
                >
                  Duyệt
                </button>
                <button
                  class="btn-view-images"
                  @click="openViewImagesModal(account)"
                >
                  Xem Hình Ảnh
                </button>
              </td>              
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>
    </div>
    <div class="modal-overlay" v-if="approvalModalVisible">
      <div class="modal-content">
        <h2>Xác nhận duyệt tài khoản</h2>
        <p>
          Bạn có chắc chắn muốn duyệt tài khoản <strong>{{ approvalAccount?.gameName }}</strong>
          của <strong>{{ approvalAccount?.accountName }}</strong>?
        </p>
        <div class="modal-buttons">
          <button @click="confirmApproval" class="confirm-button">Xác nhận</button>
          <button @click="closeApprovalModal" class="cancel-button">Hủy</button>
        </div>
      </div>
    </div>
    <div class="modal-overlay success-modal" v-if="approvalSuccessModalVisible">
      <div class="modal-content">
        <i class="fas fa-check-circle success-icon"></i>
        <h2>Thành công</h2>
        <p>Tài khoản đã được duyệt thành công.</p>
        <div class="modal-buttons">
          <button @click="closeApprovalSuccessModal" class="close-button">Đóng</button>
        </div>
      </div>
    </div>
    <div class="modal-overlay" v-if="viewImagesModalVisible">
      <div class="modal-content image-modal">
        <h2>Hình Ảnh của Tài Khoản</h2>
        <div class="image-viewer">
          <button
            class="arrow-button left-arrow"
            @click="prevImage"
            :disabled="currentImageIndex === 0"
            aria-label="Previous Image"
          >
            &#8592;
          </button>
          <div class="image-container">
            <img
              :src="viewImages[currentImageIndex]"
              alt="Account Image"
              class="gallery-image-large"
            />
          </div>
          <button
            class="arrow-button right-arrow"
            @click="nextImage"
            :disabled="currentImageIndex === viewImages.length - 1"
            aria-label="Next Image"
          >
            &#8594;
          </button>
        </div>
        <div class="modal-buttons">
          <button @click="closeViewImagesModal" class="close-button">Đóng</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import getallgameaccountAPI from '@/api/getallgameaccount.api';
import ManageUpdateForAdminAPI from '@/api/manageforadmin.api';
import type { Lol_Game, NgocRong_Game, Pubg_Game, TocChien_Game, Valorant_Game } from '@/models/addaccount-model';
import LoadingSpinner from '@/components/LoadingSpinner.vue';
const loading = ref(true);
const selectedStatus = ref('All');
const activeTab = ref('lol');
const gameTabs = ref([
  { key: 'lol', name: 'League of Legends' },
  { key: 'ngocRong', name: 'Ngọc Rồng Online' },
  { key: 'pubg', name: 'Pubg' },
  { key: 'valorant', name: 'Valorant' },
  { key: 'tocChien', name: 'Tốc Chiến' },
]);

const lolAccounts = ref<Lol_Game[]>([]);
const ngocRongAccounts = ref<NgocRong_Game[]>([]);
const pubgAccounts = ref<Pubg_Game[]>([]);
const tocChienAccounts = ref<TocChien_Game[]>([]);
const valorantAccounts = ref<Valorant_Game[]>([]);
const errorMessage = ref<string | null>(null);
const approvalModalVisible = ref(false);
const approvalAccount = ref<any>(null);
const approvalSuccessModalVisible = ref(false);
const viewImagesModalVisible = ref(false);
const viewImages = ref<string[]>([]);
const currentImageIndex = ref(0);
const getFullImageUrl = (imageString: string | null): string => {
  const baseUrl = 'https://localhost:7071/';
  const images = imageString ? imageString.split(';').filter(img => img.trim() !== '') : [];
  return images.length > 0 ? `${baseUrl}${images[0]}` : 'https://via.placeholder.com/550x500';
};
const filterByStatus = (accounts: any[]) => {
  return selectedStatus.value === 'All'
    ? accounts
    : accounts.filter(account => account.status === selectedStatus.value);
};

const sortByStatus = (accounts: any[]) => {
  return accounts.slice().sort((a, b) => {
    if (a.status === 'Đang chờ duyệt' && b.status !== 'Đang chờ duyệt') return -1;
    if (a.status !== 'Đang chờ duyệt' && b.status === 'Đang chờ duyệt') return 1;
    return 0;
  });
};
const filteredLolAccounts = computed(() => sortByStatus(filterByStatus(lolAccounts.value)));
const filteredNgocRongAccounts = computed(() => sortByStatus(filterByStatus(ngocRongAccounts.value)));
const filteredPubgAccounts = computed(() => sortByStatus(filterByStatus(pubgAccounts.value)));
const filteredTocChienAccounts = computed(() => sortByStatus(filterByStatus(tocChienAccounts.value)));
const filteredValorantAccounts = computed(() => sortByStatus(filterByStatus(valorantAccounts.value)));

onMounted(async () => {
  loading.value = true;
  try {
    const [lolRes, ngocRongRes, pubgRes, tocChienRes, valorantRes] = await Promise.all([
      getallgameaccountAPI.getAllLOL(),
      getallgameaccountAPI.getAllNgocRong(),
      getallgameaccountAPI.getAllPubg(),
      getallgameaccountAPI.getAllTocChien(),
      getallgameaccountAPI.getAllValorant()
    ]);

    lolAccounts.value = lolRes.data.result?.data || [];
    ngocRongAccounts.value = ngocRongRes.data.result?.data || [];
    pubgAccounts.value = pubgRes.data.result?.data || [];
    tocChienAccounts.value = tocChienRes.data.result?.data || [];
    valorantAccounts.value = valorantRes.data.result?.data || [];
  } catch (error) {
    console.error("Error fetching game accounts:", error);
    errorMessage.value = "Đã xảy ra lỗi khi lấy dữ liệu tài khoản.";
  } finally {
    loading.value = false;
  }
});

const openApprovalModal = (gameType: string, account: any) => {
  const game = gameTabs.value.find(g => g.key === gameType);
  approvalAccount.value = { gameType, gameName: game ? game.name : '', ...account };
  approvalModalVisible.value = true;
};

const closeApprovalModal = () => {
  approvalModalVisible.value = false;
  approvalAccount.value = null;
};

const confirmApproval = async () => {
  if (!approvalAccount.value) return;

  try {
    const { gameType, id } = approvalAccount.value;
    let response;

    switch (gameType) {
      case 'lol':
        response = await ManageUpdateForAdminAPI.ManageUpdateLOL(id);
        break;
      case 'ngocRong':
        response = await ManageUpdateForAdminAPI.ManageUpdateNgocRong(id);
        break;
      case 'pubg':
        response = await ManageUpdateForAdminAPI.ManageUpdatePubg(id);
        break;
      case 'tocChien':
        response = await ManageUpdateForAdminAPI.ManageUpdateTocChien(id);
        break;
      case 'valorant':
        response = await ManageUpdateForAdminAPI.ManageUpdateValorant(id);
        break;
      default:
        throw new Error("Invalid game type");
    }
      approvalModalVisible.value = false;
      approvalSuccessModalVisible.value = true;
      await fetchUpdatedAccounts();
  } catch (error) {
    console.error("Error approving account:", error);
    errorMessage.value = "Đã xảy ra lỗi khi duyệt tài khoản.";
  }
};

const closeApprovalSuccessModal = () => {
  approvalSuccessModalVisible.value = false;
};

const fetchUpdatedAccounts = async () => {
  loading.value = true;
  try {
    const [lolRes, ngocRongRes, pubgRes, tocChienRes, valorantRes] = await Promise.all([
      getallgameaccountAPI.getAllLOL(),
      getallgameaccountAPI.getAllNgocRong(),
      getallgameaccountAPI.getAllPubg(),
      getallgameaccountAPI.getAllTocChien(),
      getallgameaccountAPI.getAllValorant()
    ]);

    lolAccounts.value = lolRes.data.result?.data || [];
    ngocRongAccounts.value = ngocRongRes.data.result?.data || [];
    pubgAccounts.value = pubgRes.data.result?.data || [];
    tocChienAccounts.value = tocChienRes.data.result?.data || [];
    valorantAccounts.value = valorantRes.data.result?.data || [];
  } catch (error) {
    console.error("Error fetching updated accounts:", error);
    errorMessage.value = "Đã xảy ra lỗi khi cập nhật dữ liệu tài khoản.";
  } finally {
    loading.value = false; 
  }
};

const openViewImagesModal = (account: any) => {
  if (account.image && typeof account.image === 'string') {
    const imagesArray = account.image.split(';').map((img: string | null) => getFullImageUrl(img));
    viewImages.value = imagesArray.length > 0 ? imagesArray : ['https://via.placeholder.com/550x500'];
    currentImageIndex.value = 0;
    console.log('Images Array:', viewImages.value);
  } else {
    viewImages.value = ['https://via.placeholder.com/550x500'];
    currentImageIndex.value = 0;
  }
  viewImagesModalVisible.value = true;
};

const closeViewImagesModal = () => {
  viewImagesModalVisible.value = false;
  viewImages.value = [];
};

const nextImage = () => {
  if (currentImageIndex.value < viewImages.value.length - 1) {
    currentImageIndex.value++;
  }
};

const prevImage = () => {
  if (currentImageIndex.value > 0) {
    currentImageIndex.value--;
  }
};

const formatDate = (dateInput: Date | string | null | undefined) => {
  if (!dateInput) return '';

  const date = typeof dateInput === 'string' ? new Date(dateInput) : dateInput;

  return date.toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
};

const formatBalance = (price: number) => {
  if (isNaN(price)) return '0 VNĐ';
  return price.toLocaleString('vi-VN') + ' VNĐ';
};
</script>

<style scoped>
.container {
  width: 100%;
  max-width: 1400px;
  margin: 20px auto;
  background-color: #fff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  font-family: Arial, sans-serif;
}

.title {
  font-size: 24px;
  color: #0a66c2;
  text-align: center;
  margin-bottom: 20px;
}

.filter-section {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
}

.filter-section label {
  font-size: 14px;
  color: #333333;
  font-weight: 600;
}

.filter-section select {
  padding: 8px 12px;
  border-radius: 6px;
  border: 1px solid #ccd6dd;
  background-color: #f3f6f9;
  color: #333333;
  font-size: 14px;
  transition: border-color 0.3s, box-shadow 0.3s;
}

.filter-section select:hover {
  border-color: #0073b1;
  box-shadow: 0 0 5px rgba(0, 115, 177, 0.2);
}

.filter-section select:focus {
  border-color: #005582;
  box-shadow: 0 0 8px rgba(0, 115, 177, 0.4);
  outline: none;
}

.filter-section option {
  padding: 5px;
  font-size: 14px;
}

.tabs {
  display: flex;
  border-bottom: 2px solid #e5e7eb;
  margin-bottom: 20px;
}

.tab-button {
  padding: 10px 20px;
  background-color: #f3f6f9;
  border: none;
  border-top-left-radius: 6px;
  border-top-right-radius: 6px;
  cursor: pointer;
  font-size: 13px;
  color: #333333;
  transition: background-color 0.3s, color 0.3s;
  margin-right: 5px;
}

.tab-button:hover {
  background-color: #e1f5fe;
}

.tab-button.active {
  background-color: #0073b1;
  color: #ffffff;
  font-weight: 600;
}

.account-section {
  margin-bottom: 40px;
}

.table-title {
  margin-bottom: 10px;
  display: flex;
  justify-content: center;
  font-size: 18px;
  color: #0a66c2;
}

.account-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

.account-table th, .account-table td {
  padding: 10px;
  border-bottom: 1px solid #e5e7eb;
  text-align: left;
}

.account-table th {
  background-color: #f3f6f9;
  color: #333333;
  font-weight: 600;
}

.account-table tr:hover {
  background-color: #f1f1f1;
}

.btn-approve {
  padding: 6px 12px;
  background-color: #0a66c2;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
  margin-right: 5px;
}

.btn-approve:hover {
  background-color: #004182;
}

.btn-approve:disabled {
  background-color: #cccccc;
  color: #666666;
  cursor: not-allowed;
}

.btn-view-images {
  padding: 6px 12px;
  background-color: #3662d0;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.btn-view-images:hover {
  background-color: #1a3ba8;
}

.error-message {
  color: #e74c3c;
  font-weight: bold;
  text-align: center;
  margin-top: 20px;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background-color: #ffffff;
  padding: 30px;
  width: 500px; 
  max-width: 90%;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  position: relative;
  animation: fadeIn 0.3s ease-out;
  text-align: center;
}

.modal-content h2 {
  margin-bottom: 20px;
  font-size: 20px;
  color: #0073b1;
  font-weight: 600;
}

.modal-content p {
  font-size: 14px;
  color: #333;
  margin-bottom: 20px;
}

.modal-buttons {
  margin-top: 10px;
  display: flex;
  justify-content: center;
  gap: 15px;
}

.confirm-button {
  padding: 10px 25px;
  background-color: #0073b1;
  color: #ffffff;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.confirm-button:hover {
  background-color: #005580;
}

.cancel-button, .close-button {
  padding: 10px 25px;
  background-color: #dc3545;
  color: #ffffff;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.cancel-button:hover, .close-button:hover {
  background-color: #c82333;
}

.success-modal .modal-content {
  text-align: center;
}

.success-modal .success-icon {
  font-size: 40px;
  color: #28a745;
  margin-bottom: 10px;
}

.image-modal .image-viewer {
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  flex-wrap: nowrap;
}

.image-container {
  max-width: 100%;
  max-height: 80vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.gallery-image-large {
  max-width: 100%;
  max-height: 80vh;
  object-fit: contain;
  border-radius: 6px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.arrow-button {
  background: none;
  border: none;
  font-size: 30px;
  cursor: pointer;
  color: #0073b1;
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  padding: 10px;
  user-select: none;
}

.left-arrow {
  left: 10px;
}

.right-arrow {
  right: 10px;
}

.arrow-button:disabled {
  color: #cccccc;
  cursor: not-allowed;
}

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

@media (max-width: 768px) {
  .container {
    padding: 10px;
  }

  .title {
    font-size: 20px;
  }

  .tabs {
    flex-wrap: wrap;
  }

  .tab-button {
    flex: 1 1 45%;
    margin-bottom: 5px;
  }

  .modal-content {
    width: 90%;
  }

  .gallery-image-large {
    width: 100%;
    height: auto;
  }

  .arrow-button {
    font-size: 24px;
  }

  .account-table th, .account-table td {
    padding: 8px;
    font-size: 12px;
  }

  .btn-approve, .btn-view-images {
    padding: 5px 10px;
    font-size: 12px;
  }

  .filter-section {
    flex-direction: column;
    align-items: flex-start;
  }

  .filter-section label, .filter-section select {
    width: 100%;
  }
}
</style>

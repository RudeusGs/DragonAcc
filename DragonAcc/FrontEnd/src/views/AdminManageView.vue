<template>
  <div class="container">
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
    <div class="account-tables">
      <!-- Table for League of Legends Accounts -->
      <div v-if="filteredLolAccounts.length > 0" class="account-section">
        <h3 class="table-title">League of Legends</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
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
            <tr v-for="(account, index) in lolAccounts" :key="index">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ account.description }}</td>
              <td>{{ account.price }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="handleApproval('lol', account.id)"
                >
                  Duyệt
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="filteredNgocRongAccounts.length > 0" class="account-section">
        <h3 class="table-title">Ngọc Rồng Online</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
              <th>Price</th>
              <th>Server</th>
              <th>Planet</th>
              <th>Created Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(account, index) in ngocRongAccounts" :key="index">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ account.description }}</td>
              <td>{{ account.price }}</td>
              <td>{{ account.server }}</td>
              <td>{{ account.planet }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>      
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="handleApproval('ngocRong', account.id)"
                >
                  Duyệt
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="filteredPubgAccounts.length > 0" class="account-section">
        <h3 class="table-title">Pubg</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
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
            <tr v-for="(account, index) in pubgAccounts" :key="index">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ account.description }}</td>
              <td>{{ account.price }}</td>
              <td>{{ account.humanSkinCount }}</td>
              <td>{{ account.gunSkinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="handleApproval('pubg', account.id)"
                >
                  Duyệt
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="filteredValorantAccounts.length > 0" class="account-section">
        <h3 class="table-title">Valorant</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
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
            <tr v-for="(account, index) in valorantAccounts" :key="index">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ account.description }}</td>
              <td>{{ account.price }}</td>
              <td>{{ account.gunSkinCount }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="handleApproval('valorant', account.id)"
                >
                  Duyệt
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <!-- Table for Tốc Chiến Accounts -->
      <div v-if="filteredTocChienAccounts.length > 0" class="account-section">
        <h3 class="table-title">Tốc Chiến</h3>
        <table class="account-table">
          <thead>
            <tr>
              <th>Account</th>
              <th>Password</th>
              <th>Password Changed</th>
              <th>Description</th>
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
            <tr v-for="(account, index) in tocChienAccounts" :key="index">
              <td>{{ account.accountName }}</td>
              <td>{{ account.password }}</td>
              <td>{{ account.passwordChanged }}</td>
              <td>{{ account.description }}</td>
              <td>{{ account.price }}</td>
              <td>{{ account.championCount }}</td>
              <td>{{ account.skinCount }}</td>
              <td>{{ account.rank }}</td>
              <td>{{ formatDate(account.createdDate) }}</td>
              <td>{{ account.status }}</td>
              <td>
                <button
                  class="btn-approve"
                  :disabled="account.status !== 'Đang chờ duyệt'"
                  @click="handleApproval('tocChien', account.id)"
                >
                  Duyệt
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
  </div>
</template>
  
  <script setup lang="ts">
  import { ref, onMounted, computed } from 'vue';
  import getallgameaccountAPI from '@/api/getallgameaccount.api';
  import ManageUpdateForAdminAPI from '@/api/manageforadmin.api';
  import type { Lol_Game, NgocRong_Game, Pubg_Game, TocChien_Game, Valorant_Game } from '@/models/addaccount-model';

  const selectedStatus = ref('All');
  const lolAccounts = ref<Lol_Game[]>([]);
  const ngocRongAccounts = ref<NgocRong_Game[]>([]);
  const pubgAccounts = ref<Pubg_Game[]>([]);
  const tocChienAccounts = ref<TocChien_Game[]>([]);
  const valorantAccounts = ref<Valorant_Game[]>([]);
  
  const errorMessage = ref<string | null>(null);
    const filterByStatus = (accounts: any[]) => {
  return selectedStatus.value === 'All'
    ? accounts
    : accounts.filter(account => account.status === selectedStatus.value);
};

const filteredLolAccounts = computed(() => filterByStatus(lolAccounts.value));
const filteredNgocRongAccounts = computed(() => filterByStatus(ngocRongAccounts.value));
const filteredPubgAccounts = computed(() => filterByStatus(pubgAccounts.value));
const filteredTocChienAccounts = computed(() => filterByStatus(tocChienAccounts.value));
const filteredValorantAccounts = computed(() => filterByStatus(valorantAccounts.value));
  onMounted(async () => {
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
    }
  });
  
  const handleApproval = (gameType: string, accountId: number | null | undefined) => {
    if (!accountId) {
      errorMessage.value = "Invalid account ID. Please check and try again.";
      return;
    }
  
    approveAccount(gameType, accountId);
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


  const approveAccount = async (gameType: string, accountId: number) => {
    try {
      let response;
      switch (gameType) {
        case 'lol':
          response = await ManageUpdateForAdminAPI.ManageUpdateLOL(accountId);
          break;
        case 'ngocRong':
          response = await ManageUpdateForAdminAPI.ManageUpdateNgocRong(accountId);
          break;
        case 'pubg':
          response = await ManageUpdateForAdminAPI.ManageUpdatePubg(accountId);
          break;
        case 'tocChien':
          response = await ManageUpdateForAdminAPI.ManageUpdateTocChien(accountId);
          break;
        case 'valorant':
          response = await ManageUpdateForAdminAPI.ManageUpdateValorant(accountId);
          break;
        default:
          throw new Error("Invalid game type");
      }
      errorMessage.value = null;
      alert("Account approved successfully!");
    } catch (error) {
      console.error("Error approving account:", error);
      errorMessage.value = "Failed to approve the account. Please try again.";
    }
    window.location.reload();
  };
  </script>
  
  <style scoped>
  .container {
    width: 100%;
    max-width: 1200px;
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
  
  .account-section {
    border-top: 1px solid black ;
    margin-top: 40px;
  }
  
  .table-title {
    margin-top: 10px;
    display: flex;
    justify-content: center;
    font-size: 18px;
    color: #0a66c2;
    margin-bottom: 10px;
  }
  
  .account-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
  }
  
  .account-table th {
    color: #333;
    font-weight: 600;
    padding: 10px;
    text-align: left;
    border-bottom: 2px solid #e5e7eb;
  }
  
  .account-table td {
    padding: 10px;
    border-bottom: 1px solid #e5e7eb;
    color: #555;
  }
  .btn-approve:hover {
    background-color: #004182;
  }
  .btn-approve:disabled {
    background-color: #cccccc;
    color: #666666;
    cursor: not-allowed;
  }
  .btn-approve {
    padding: 6px 12px;
    background-color: #0a66c2;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }
  
  .btn-approve:hover {
    background-color: #004182;
  }
  
  .error-message {
    color: #e74c3c;
    font-weight: bold;
    text-align: center;
    margin-top: 20px;
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .title {
      font-size: 20px;
    }
  
    .account-section {
      padding: 10px;
    }
  
    .account-table th,
    .account-table td {
      padding: 8px;
    }
  
    .btn-approve {
      padding: 5px 10px;
      font-size: 12px;
    }
  }
  .filter-section {
    top: 20px;
    right: 20px;
    display: flex;
    align-items: center;
    gap: 10px;
    font-family: Arial, sans-serif;
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
</style>

  